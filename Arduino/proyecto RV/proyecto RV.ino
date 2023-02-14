#include <Wire.h>
#include <MPU6050.h>
#include <SoftwareSerial.h>

SoftwareSerial bluetoothSerial(7, 8); // RX, TX creamos la comunicación serial con el modulo bluetooth

MPU6050 mpu1;
MPU6050 mpu2;

// Timers
unsigned long timer = 0;
float timeStep = 0.01;

// angulo previo, servira para futuros calculos
float xprev1; 
float xprev2;

// Pitch, Roll and Yaw
float pitch1 = 0;
float roll1 = 0;
float yaw1 = 0;
float pitch2 = 0;
float roll2 = 0;
float yaw2 = 0;

void setup() 
{
  //pinMode(3, OUTPUT); // para el vibrador
  Serial.begin(115200);
  bluetoothSerial.begin(115200);
  bluetoothSerial.println("Conectado");

  // Inicializamos ambos MPU6050
  while(!mpu1.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G, 0x68) | !mpu2.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G, 0x69))
  {
    Serial.println("Could not find a valid MPU6050 sensor, check wiring!");
    delay(500);
  }
  
  mpu1.calibrateGyro(); //calibración del giroscopo
  mpu2.calibrateGyro();

  mpu1.setThreshold(3); // sentibilidad del threshold (es la sensibilidad del angulo) lo dejamos en 3 por defecto. si no queremos threshold comentamos o seteamos a 0
  mpu2.setThreshold(3);  
}

void loop()
{
  timer = millis();

  Vector norm1 = mpu1.readNormalizeGyro(); // Leemos valores normalizados
  Vector norm2 = mpu2.readNormalizeGyro();

  // Calculamos Pitch, Roll and Yaw usando la libreria
  //pitch1 = pitch1 + norm1.YAxis * timeStep;
  //roll1 = roll1 + norm1.XAxis * timeStep;
  yaw1 = yaw1 + norm1.ZAxis * timeStep;
  yaw2 = yaw2 + norm2.ZAxis * timeStep;
  if ((yaw1 >= xprev1 + 2) | (yaw1 <= xprev1 - 2)){
       xprev1=yaw1;
       //analogWrite(3, 120);
       bluetoothSerial.print("1" + (String)yaw1);
       bluetoothSerial.println();
  } 
  if ((yaw2 >= xprev2 + 2) | (yaw2 <= xprev2 - 2)){
       xprev2=yaw2;
       //analogWrite(3, 120);
       bluetoothSerial.print("2" + (String)yaw2);
       bluetoothSerial.println();
  }  

  // Output rpy
  //Serial.print(" Pitch1 = "); Serial.print(pitch1); 
  //Serial.print(" Roll1 = ");  Serial.print(roll1);  
  Serial.print(" Yaw 1 = "); Serial.println(yaw1);
  Serial.print(" Yaw 2 = "); Serial.println(yaw2);

  // Wait to full timeStep period
  delay((timeStep*1000) - (millis() - timer));
}