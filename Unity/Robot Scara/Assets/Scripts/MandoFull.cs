using UnityEngine;
using System;
using System.IO.Ports; //incluimos el namespace Sustem.IO.Ports


public class MandoFull: MonoBehaviour {


	SerialPort serialPort = new SerialPort("COM5", 115200); //Inicializamos el puerto serie

	void Start () {

		serialPort.Open(); //Abrimos una nueva conexión de puerto serie
		serialPort.ReadTimeout = 1; //Establecemos el tiempo de espera cuando una operación de lectura no finaliza
	}

	void Update () {
		if (serialPort.IsOpen) //comprobamos que el puerto esta abierto
		{
			try //utilizamos el bloque try/catch para detectar una posible excepción.
			{

				string value = serialPort.ReadLine(); //leemos una linea del puerto serie y la almacenamos en un string
				print(value); //printeamos la linea leida para verificar que leemos el dato que manda nuestro Arduino
				string[] vec6 = value.Split(','); //Separamos el String leido valiendonos 
				                                  //de las comas y almacenamos los valores en un array.
			}

			catch
			{
					
			}

		}

	}

}