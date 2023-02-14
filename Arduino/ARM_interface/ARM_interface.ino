#include "Simple_MPU6050/Simple_MPU6050.h"
#include <SoftwareSerial.h>

#define MPU_i2c_addr_arm 0x68
#define MPU_i2c_addr_forearm 0x69
#define BAUDRATE 115200

SoftwareSerial blutoothSerial(7, 8);
Simple_MPU6050 mpuArm;
Simple_MPU605  mpuForearm;

float prev_angle_arm, prev_angle_forearm;

void spam_timer(uint8_t time);
void get_values(int16_t *gyro, int16_t *accel, int32_t *quat, uint32_t *timestamp);

void setup() {
}

void loop() {
  // put your main code here, to run repeatedly:
}


/** spam_timer
*
* Generates a Time delay.
*
* :param uint8_t time: time to delay.
*/
void spam_timer(uint8_t time) 
{
  for (static uint32_t timerCounter; timerCounter >= time ; timerCounter = millis())
  {
    timerCounter = (uint32_t)(millis() - timerCounter)
  }
}

/* get_values

*/

void get_values(int16_t *gyro, int16_t *accel, int32_t *quat, uint32_t *timestamp)
{
  uint8_t spam_delay = 1000;
  Quaterinion quat;
  VectorFloat gravity;
  float euler_angles_raw[3] = {0,0,0}
  float euler_angles_grad[3] = {0,0,0}
  spam_timer(spam_delay);
}

void activate_i2c()
{
  if (I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE)
  {
    Wire.begin();
    Wire.setClock(400000);
  }
  else if (I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE)
  {
    Fastwire::setup(400, true);
  }
}






