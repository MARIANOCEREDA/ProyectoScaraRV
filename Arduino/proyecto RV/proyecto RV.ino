/*
    Proyecto RV

    This sketch implements the following tasks:
    - Read data from MPU sensors in order to calculate its slope angle.
    - Connect via bluetooth and send data from the sensors.
    - Connect via bluetooth serial and received incomming data.

    The circuit:
    * OUTPUT
    *  - PIN 3 -> Vibrador
    *  - PIN 8 -> RX bluetooth serial HC-06
    *  - PIN 6 -> Start button
    * INPUT:
    *  - PIN 4 -> MPU 1
    *  - PIN 5 -> MPU 2
    *  - PIN 7 -> TX bluetooth serial HC-06

    http://url/of/online/tutorial.cc

*/

#include <Wire.h>
#include <MPU6050.h>
#include <SoftwareSerial.h>

#define PIN_START 6
#define PIN_RX_BT_SERIAL 7
#define PIN_TX_BT_SERIAL 8
#define PIN_VIBRATOR 5
#define PIN_EF_MOVE_UP 3
#define PIN_EF_MOVE_DOWN 4
#define BT_SERIAL_BAUDRATE 115200
#define SERIAL_BAUDRATE 115200


SoftwareSerial bluetoothSerial(PIN_RX_BT_SERIAL, PIN_TX_BT_SERIAL);  // RX, TX creamos la comunicación serial con el modulo bluetooth

MPU6050 mpu1;
MPU6050 mpu2;

const int ANGLE_SENSITIVITY = 2;  // deg
const int THRESHOLD_SENSITIVITY = 3;

// Timers
unsigned long timer = 0;
float timeStep = 0.01;

// start button
bool start = false;

// angulo previo, servira para futuros calculos
float xprev1, xprev2;
String angulo1 = "0";
String angulo2 = "0";
String efectfin = "0";
char limit = "0";
byte arriba = 0;
byte abajo = 0;

// Pitch, Roll and Yaw
float pitch1 = 0;
float roll1 = 0;
float yaw1 = 0;
float pitch2 = 0;
float roll2 = 0;
float yaw2 = 0;

float pitch[] = { 0, 0 };
float yaw[] = { 0, 0 };
float roll[] = { 0, 0 };


// Function declaration
void printAngles(bool, bool, bool);
void onStart();


void setup() {
  pinMode(PIN_EF_MOVE_UP, INPUT);  // pines de efector final y vibración
  pinMode(PIN_EF_MOVE_DOWN, INPUT);

  pinMode(PIN_VIBRATOR, OUTPUT);  // vibration pin

  Serial.begin(SERIAL_BAUDRATE);
  bluetoothSerial.begin(BT_SERIAL_BAUDRATE);
  bluetoothSerial.println("Conectado");

  // Inicializamos ambos MPU6050
  while (!mpu1.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G, 0x68) | !mpu2.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G, 0x69)) {
    Serial.println("Could not find a valid MPU6050 sensor, check wiring!");
    delay(500);
  }

  mpu1.calibrateGyro();  // calibración del giroscopo
  mpu2.calibrateGyro();

  mpu1.setThreshold(THRESHOLD_SENSITIVITY);  // sentibilidad del threshold (es la sensibilidad del angulo) lo dejamos en 3 por defecto. si no queremos threshold comentamos o seteamos a 0
  mpu2.setThreshold(THRESHOLD_SENSITIVITY);

  attachInterrupt(digitalPinToInterrupt(PIN_START), onStart, RISING);
}

void loop() {

  if (start) {
    arriba = digitalRead(PIN_EF_MOVE_UP);
    abajo = digitalRead(PIN_EF_MOVE_DOWN);

    timer = millis();

    Vector norm1 = mpu1.readNormalizeGyro();  // Leemos valores normalizados
    Vector norm2 = mpu2.readNormalizeGyro();

    // Calculamos Pitch, Roll and Yaw usando la libreria
    //pitch1 = pitch1 + norm1.YAxis * timeStep;
    //roll1 = roll1 + norm1.XAxis * timeStep;
    yaw1 = yaw1 + norm1.ZAxis * timeStep;
    yaw2 = yaw2 + norm2.ZAxis * timeStep;

    //Analizamos angulos previos y si varia por 2° cambiamos el valor.

    if ((yaw1 >= xprev1 + ANGLE_SENSITIVITY))
    {
      xprev1 = yaw1;
      angulo1 = "1";
    } else if (yaw1 <= xprev1 - ANGLE_SENSITIVITY) {
      xprev1 = yaw1;
      angulo1 = "-1";
    } else {
      angulo1 = "0";
    }
    if ((yaw2 >= xprev2 + ANGLE_SENSITIVITY)) {
      xprev2 = yaw2;
      angulo2 = "1";
    } else if (yaw2 <= xprev2 - ANGLE_SENSITIVITY) {
      xprev2 = yaw2;
      angulo2 = "-1";
    } else {
      angulo2 = "0";
    }

    // Logica del efector final
    if (arriba) {
      efectfin = "1";
    } else if (abajo) {
      efectfin = "-1";
    }
    if ((!arriba && !abajo) || (arriba && abajo)) {
      efectfin = "0";
    }

    //Enviamos y recibimos datos por bluetooth
    bluetoothSerial.print(angulo1 + ";" + angulo2 + ";" + efectfin);
    bluetoothSerial.println();

    printAngles(true, false, true);

    if (bluetoothSerial.available()) {
      limit = bluetoothSerial.read();
      Serial.println(limit);
    } else {
      limit = '0';
    }
    if (limit == '1') {
      digitalWrite(PIN_VIBRATOR, HIGH);
    } else {
      digitalWrite(PIN_VIBRATOR, LOW);
    }

    delay(30);
  }
}


/*
* @name printAngles
* 
* @brief print the yaw angles depending on the incoming parameters
*
* @param roll {bool} - indicates if roll angles wants to be printed.
* @param pitch {bool} - indicates if pitch angles wants to be printed.
* @pasram yaw {bool} - indicates if yaw angles wants to be printed.
*/
void printAngles(bool roll, bool pitch, bool yaw) {

  if (yaw) {
    Serial.print(" Yaw 1 = ");
    Serial.println(yaw1);
    Serial.print(" Yaw 2 = ");
    Serial.println(yaw2);
  }

  if (pitch) {
    Serial.print(" Pitch 1 = ");
    Serial.println(pitch1);
    Serial.print(" Pitch 2 = ");
    Serial.println(pitch2);
  }

  if (roll) {
    Serial.print(" Roll 1 = ");
    Serial.println(roll1);
    Serial.print(" Roll 2 = ");
    Serial.println(roll2);
  }
}

/*
* @name OnStart
* 
* @brief Callback function executed when the interruption to start is raised.
*/
void onStart() {

  if (!start) {
    start = true;
  } else if (start) {
    start = false;
  }

}

