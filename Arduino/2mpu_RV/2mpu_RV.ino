
#include "Simple_MPU6050.h"					// incluye libreria Simple_MPU6050
#include <SoftwareSerial.h>

#define MPU1    0x68			// direccion I2C con AD0 en LOW o sin conexion
#define MPU2    0x69			// direccion I2C con AD0 en HIGH


SoftwareSerial bluetoothSerial(7, 8); // RX, TX creamos la comunicación serial con el modulo bluetooth
Simple_MPU6050 mpu1;				// crea objeto con nombre mpu1
Simple_MPU6050 mpu2;				// crea objeto con nombre mpu2

float xprev1;  // angulo previo, servira para futuros calculos
float xprev2;

// #define OFFSETS  -5114,     484,    1030,      46,     -14,       6  // Colocar valores personalizados

#define spamtimer(t) for (static uint32_t SpamTimer; (uint32_t)(millis() - SpamTimer) >= (t); SpamTimer = millis())
// spamtimer funcion para generar demora al escribir en monitor serie sin usar delay(), al usar interrupciones y mostrar gran cantidad de datos no es recomendable hacer bloqueos al flujo del programa

#define printfloatx(Name,Variable,Spaces,Precision,EndTxt) print(Name); {char S[(Spaces + Precision + 3)];Serial.print(F(" ")); Serial.print(dtostrf((float)Variable,Spaces,Precision ,S));}Serial.print(EndTxt);
// printfloatx funcion para mostrar en monitor serie datos para evitar el uso se multiples print(), al printear muchos datos y doble mejora el rendiimiento

// mostrar_valores funcion que es llamada cada vez que hay datos disponibles desde el sensor
void obtener_y_mostrar_valores1 (int16_t *gyro, int16_t *accel, int32_t *quat, uint32_t *timestamp) {	
  uint8_t SpamDelay = 1000;			// demora para escribir en monitor serie de 100 mseg
  Quaternion q;					// variable necesaria para calculos posteriores
  VectorFloat gravity;				// variable necesaria para calculos posteriores
  float ypr1[3] = { 0, 0, 0 };			// array para almacenar valores de yaw, pitch, roll
  float xyz1[3] = { 0, 0, 0 };			// array para almacenar valores convertidos a grados de yaw, pitch, roll
  spamtimer(SpamDelay) {			// si han transcurrido al menos 10000 mseg entonces proceder
    mpu1.GetQuaternion(&q, quat);		// funcion para obtener valor para calculo posterior
    mpu1.GetGravity(&gravity, &q);		// funcion para obtener valor para calculo posterior
    mpu1.GetYawPitchRoll(ypr1, &q, &gravity);	// funcion obtiene valores de yaw, ptich, roll
    mpu1.ConvertToDegrees(ypr1, xyz1);		// funcion convierte a grados sexagesimales
    Serial.printfloatx(F("Yaw 1")  , xyz1[0], 9, 4, F(",   "));  // muestra en monitor serie rotacion de eje Z, yaw
    Serial.printfloatx(F("Pitch 1"), xyz1[1], 9, 4, F(",   "));  // muestra en monitor serie rotacion de eje Y, pitch
    Serial.printfloatx(F("Roll 1") , xyz1[2], 9, 4, F(",   "));  // muestra en monitor serie rotacion de eje X, roll
    Serial.println();				// salto de linea    
    if ((xyz1[0] >= xprev1 + 2) | (xyz1[0] <= xprev1 - 2)){
       xprev1=xyz1[0];
       analogWrite(3, 120);
       bluetoothSerial.print("1" + (String)xyz1[0]);
       bluetoothSerial.println();
    }
  }
}
void obtener_y_mostrar_valores2 (int16_t *gyro, int16_t *accel, int32_t *quat, uint32_t *timestamp) {	
  uint8_t SpamDelay = 1000;			// demora para escribir en monitor serie de 100 mseg
  Quaternion q;					// variable necesaria para calculos posteriores
  VectorFloat gravity;				// variable necesaria para calculos posteriores
  float ypr2[3] = { 0, 0, 0 };			// array para almacenar valores de yaw, pitch, roll
  float xyz2[3] = { 0, 0, 0 };			// array para almacenar valores convertidos a grados de yaw, pitch, roll
  spamtimer(SpamDelay) {			// si han transcurrido al menos 100 mseg entonces proceder
    mpu2.GetQuaternion(&q, quat);		// funcion para obtener valor para calculo posterior
    mpu2.GetGravity(&gravity, &q);		// funcion para obtener valor para calculo posterior
    mpu2.GetYawPitchRoll(ypr2, &q, &gravity);	// funcion obtiene valores de yaw, ptich, roll
    mpu2.ConvertToDegrees(ypr2, xyz2);		// funcion convierte a grados sexagesimales    
    Serial.printfloatx(F("Yaw 2")  , xyz2[0], 9, 4, F(",   "));  // muestra en monitor serie rotacion de eje Z, yaw
    Serial.printfloatx(F("Pitch 2"), xyz2[1], 9, 4, F(",   "));  // muestra en monitor serie rotacion de eje Y, pitch
    Serial.printfloatx(F("Roll 2") , xyz2[2], 9, 4, F(",   "));  // muestra en monitor serie rotacion de eje X, roll
    Serial.println();				// salto de linea  
    if ((xyz2[0] >= xprev2 + 2) | (xyz2[0] <= xprev2 - 2)){
       xprev2=xyz2[0];
       analogWrite(3, 120);
       bluetoothSerial.print("2" + (String)xyz2[0]);
       bluetoothSerial.println();
    }  
  }
}
void setup() {
  
  pinMode(3, OUTPUT);
  uint8_t val;
#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE	// activacion de bus I2C a 400 Khz
  Wire.begin();
  Wire.setClock(400000);
#elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
  Fastwire::setup(400, true);
#endif
  
  Serial.begin(115200);			// inicializacion de monitor serie a 115200 bps
  Serial.println(F("Inicio:"));		// muestra texto estatico
  bluetoothSerial.begin(9600);
  bluetoothSerial.println("Conectado");
#ifdef OFFSETS								// si existen OFFSETS
  Serial.println(F("Usando Offsets predefinidos"));			// texto estatico
  mpu1.SetAddress(MPU1).load_DMP_Image(OFFSETS);	// inicializacion de sensor
  mpu2.SetAddress(MPU2).load_DMP_Image(OFFSETS);  

#else										// sin no existen OFFSETS
  Serial.println(F(" No se establecieron Offsets, haremos unos nuevos.\n"	// muestra texto estatico
                   " Colocar el sensor en un superficie plana y esperar unos segundos\n"
                   " Colocar los nuevos Offsets en #define OFFSETS\n"
                   " para saltar la calibracion inicial \n"
                   " \t\tPresionar cualquier tecla y ENTER"));
  while (Serial.available() && Serial.read());		// lectura de monitor serie
  while (!Serial.available());   			// si no hay espera              
  while (Serial.available() && Serial.read()); 		// lecyura de monitor serie
  mpu1.SetAddress(MPU1).CalibrateMPU().load_DMP_Image();	// inicializacion de sensor y calibra los offsets
  mpu2.SetAddress(MPU2).CalibrateMPU().load_DMP_Image();
#endif
  mpu1.on_FIFO(obtener_y_mostrar_valores1);		// llamado a funcion mostrar_valores si memoria FIFO tiene valores (FIFO es una memoria del mpu fist input first output)
  mpu2.on_FIFO(obtener_y_mostrar_valores2);
}

void loop() {

  mpu1.dmp_read_fifo();		// funcion que evalua si existen datos nuevos en el sensor y llama a funcion mostrar_valores si es el caso
  mpu2.dmp_read_fifo();
}	