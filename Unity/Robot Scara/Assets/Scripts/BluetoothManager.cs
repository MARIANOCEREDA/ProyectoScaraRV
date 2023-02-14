using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using UnityEngine;

public class BluetoothManager : MonoBehaviour
{
    /* BluetoothManager
     
    This class manages the bluetooth communication via a serial port.
    The .NET built-in class SerialPort is used.
    SerialPort Doumentation: https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-7.0
    Usage example: https://www.c-sharpcorner.com/UploadFile/eclipsed4utoo/communicating-with-serial-port-in-C-Sharp/

     */

    private int BAUDRATE = 115200;
    private int READ_TIMEOUT = 10000;
    private int WRITE_TIMEOUT = 10000;
    private string port = "COM3";
    private string deviceName = "btDevice";
    private SerialPort _serialPort;
    string message;

    void Start()
    {
        Connect();
    }
    
    void Update()
    {
        try
        {
            if (_serialPort.IsOpen)
            {
                message = _serialPort.ReadLine();
                _serialPort.BaseStream.Flush();
                Debug.Log("Message from Serial: " + message + "\n");
            }
        }
        catch(Exception e) 
        {
            Debug.LogError("Error when trying to read from Serial Port: "+ port + " - " + e);
        }
    }

    void Connect()
    {
        try
        {
            Debug.Log("Connecting to Bluetooth via Serial Port:  " + deviceName + "...");
            _serialPort = new SerialPort(port, BAUDRATE, Parity.None, 8, StopBits.One);

            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                setConfiguration();
            }
            Debug.Log("Connected to Serial Port�: " + port );
        }
        catch (Exception err)
        {
            Debug.Log("Error when trying to Open Serial Communication: " + err);
            throw err;
        }

    }
    void Disconnect()
    {
        try
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            else
            {
                Debug.Log("The connection is not open.");
            }

        }
        catch (IOException err)
        {
            Debug.Log("Error when trying to close the connection: " + err);
        }
    }

    void setConfiguration()
    {
        _serialPort.ReadTimeout = READ_TIMEOUT;
        _serialPort.WriteTimeout = WRITE_TIMEOUT;
        _serialPort.Handshake = Handshake.None;
    }
}