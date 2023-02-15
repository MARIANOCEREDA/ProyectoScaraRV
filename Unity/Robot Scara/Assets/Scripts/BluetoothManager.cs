using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BluetoothManager : MonoBehaviour
{
    /* BluetoothManager
     
    This class manages the bluetooth communication via a serial port.
    The .NET built-in class SerialPort is used.
    SerialPort Doumentation: https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-7.0
    Usage example: https://www.c-sharpcorner.com/UploadFile/eclipsed4utoo/communicating-with-serial-port-in-C-Sharp/

     */
    public GameObject robot;
    private int BAUDRATE = 115200;
    private int READ_TIMEOUT = 10000;
    private int WRITE_TIMEOUT = 10000;
    private string port = "COM3";
    private string deviceName = "HC-06";
    private SerialPort _serialPort;
    string message;
    public float[] speeds;
    float[] inAngles = new float[3];

    private void Start()
    {
        OnConnect();
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
                parseIncomingMessage(message);
            }
        }
        catch(Exception e) 
        {
            Debug.LogError("Error when trying to read from Serial Port: "+ port + " - " + e);
        }

        ScaraController scaraController = robot.GetComponent<ScaraController>();
        for (int i = 0; i < scaraController.joints.Length; i++)
        {
            SetJointSpeed(scaraController.joints[i].robotPart, speeds[i]);
            if (inAngles[i] != 0)
            {
                MovementDirection directionArm = GetRotationDirection(inAngles[i]);
                scaraController.MoveJoint(i, directionArm);
            }
            else
            {
                scaraController.MoveJoint(i, MovementDirection.None);
            }
        }
        return;
    }

    void SetJointSpeed(GameObject robotPart, float speed)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.SetArticulationSpeed(speed);
    }

    static MovementDirection GetRotationDirection(float inputVal)
    {
        if (inputVal > 0)
        {
            return MovementDirection.Positive;
        }
        else if (inputVal < 0)
        {
            return MovementDirection.Negative;
        }
        else
        {
            return MovementDirection.None;
        }
    }

    public void OnConnect()
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
            Debug.Log("Connected to Serial Port�: " + port);
        }
        catch (Exception err)
        {
            Debug.Log("Error when trying to Open Serial Communication: " + err);
            throw err;
        }
    }
    public void OnDisconnect()
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

    void parseIncomingMessage(string rawMessage)
    {
        string[] angleValues = rawMessage.Split(";");
        if (rawMessage != null)
        {
            inAngles[0] = float.Parse(angleValues[0]);
            inAngles[1] = float.Parse(angleValues[1]);
            inAngles[2] = float.Parse(angleValues[2]);

            Debug.Log("Angles {" + inAngles[0] + " ; " + inAngles[1] + " ; " + inAngles[2] + "}");
        }
    }
}