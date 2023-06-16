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

#define PIN_START 2
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

const int ANGLE_SENSITIVITY = 1;  // deg
const int THRESHOLD_SENSITIVITY = 3;

// Timers
unsigned long timer = 0;
float timeStep = 0.01;

// start button
bool start = false;
bool reset_direction = false;

// right and left
String right = "1";
String left = "-1";

// Uart
char serial_msg = "";

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
float relative_yaw2 = 0;

// Current angles to reset to zero when desired

float pitch[] = { 0, 0 };
float yaw[] = { 0, 0 };
float roll[] = { 0, 0 };


// Declaracion de funciones
void printData();
void onStart();
void InitMpu();
void ParseIncomingMessage();
void ResetMpuAngles();


void setup() {
  pinMode(PIN_EF_MOVE_UP, INPUT);  // pines de efector final
  pinMode(PIN_EF_MOVE_DOWN, INPUT);

  pinMode(PIN_VIBRATOR, OUTPUT);  // pin vibrador

  Serial.begin(SERIAL_BAUDRATE);
  bluetoothSerial.begin(BT_SERIAL_BAUDRATE);
  bluetoothSerial.println("Conectado");

  // Inicializamos ambos MPU6050
  InitMpu();

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
    relative_yaw2 = yaw2 - yaw1;

    //Analizamos angulos previos y si varia por 1° cambiamos el valor.
    if ((yaw1 >= xprev1 + ANGLE_SENSITIVITY))
    {
      xprev1 = yaw1;
      angulo1 = right;
    } else if (yaw1 <= xprev1 - ANGLE_SENSITIVITY) {
      xprev1 = yaw1;
      angulo1 = left;
    } else {
      angulo1 = "0";
    }
    if ((relative_yaw2 >= xprev2 + ANGLE_SENSITIVITY)) {
      xprev2 = relative_yaw2;
      angulo2 = right;
    } else if (relative_yaw2 <= xprev2 - ANGLE_SENSITIVITY) {
      xprev2 = relative_yaw2;
      angulo2 = left;
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
    bluetoothSerial.print(":"+ angulo1 + ";" + angulo2 + ";" + efectfin);
    bluetoothSerial.println();

    if (bluetoothSerial.available()) {
      ParseIncomingMessage();
    }

    if (limit == '1') {
      digitalWrite(PIN_VIBRATOR, HIGH);
    } else if (limit == '0'){
      digitalWrite(PIN_VIBRATOR, LOW);
    }

    if(reset_direction){
      ResetMpuAngles();
      reset_direction = false;
    }

    printData();

    delay(30);
  }
}


/*
* @name printAngles
* 
* @brief print the yaw angles depending on the incoming parameters
*
*/

void printData() {
    Serial.print("Yaw 1 = ");
    Serial.print(yaw1);
    Serial.print(" - Yaw 2 = ");
    Serial.print(yaw2);
    Serial.print(" - Reset = ");
    Serial.print(reset_direction);
    Serial.print(" - Limit = ");
    Serial.println(limit);
}

void ParseIncomingMessage(){
  serial_msg = bluetoothSerial.read();
  Serial.print(serial_msg);

  switch (serial_msg) {
    case '1':
      limit = serial_msg;
      break;

    case '0':
      limit = serial_msg;
      break;

    case 'R':
      reset_direction = true;
      break;
    
    default: break;
  }
}

void ResetMpuAngles(){

  // anlgles
  yaw1 = 0;
  yaw2 = 0;

  // previous angles
  xprev1 = 0;
  xprev2 = 0;

  // reset direction
  //right = "-1";
  //left = "1";
}

void InitMpu(){
  while (!mpu1.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G, 0x68) | !mpu2.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G, 0x69)) {
    Serial.println("Could not find a valid MPU6050 sensor, check wiring!");
    delay(500);
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
    Serial.println("Status: STARTED");
  } else if (start) {
    start = false;
    Serial.println("Status: STOPPED");
  }

}
