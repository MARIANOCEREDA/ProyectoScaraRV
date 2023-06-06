using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using UnityEngine;
/*using UnityEngine.UI;
using UnityEditor.PackageManager;*/

public enum BTConnectionStatus { CONNECTED, DISCONNECTED }

public class BluetoothManager : MonoBehaviour
{
    /* BluetoothManager
     
    This class manages the bluetooth communication via a serial port.
    The .NET built-in class SerialPort is used.
    SerialPort Doumentation: https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-7.0
    Usage example: https://www.c-sharpcorner.com/UploadFile/eclipsed4utoo/communicating-with-serial-port-in-C-Sharp/

     */

    private BTConnectionStatus status;
    private int BAUDRATE = 115200;
    private int READ_TIMEOUT = 100000;
    private int WRITE_TIMEOUT = 100000;
    private string port = "COM6";
    private string deviceName = "HC-06";
    private SerialPort _serialPort;
    string message;
    public float[] speeds;
    float[] inAngles = new float[3];

    private static BluetoothManager instance;

    public static BluetoothManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<BluetoothManager>();

                if (instance == null )
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<BluetoothManager>();
                    singletonObject.name = typeof(BluetoothManager).ToString();
                    //DontDestroyOnLoad(singletonObject);
                }

            }
            return instance;
        }
    }


    private void Awake()
    {
        // Ensure that only one instance of the class exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        /*else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }*/
        //robot = GameObject.Find("Robot");
    }

    private void Start()
    {
        //OnConnect();
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
                parseIncomingMessage(message.ToString());
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error when trying to read from Serial Port: "+ port + " - " + e);
        }

        if (ScaraController.Instance != null)
        {
            //ScaraController scaraController = robot.GetComponent<ScaraController>();
            for (int i = 0; i < ScaraController.Instance.joints.Length; i++)
            {
                SetJointSpeed(ScaraController.Instance.joints[i].robotPart, speeds[i]);
                if (inAngles[i] != 0)
                {
                    MovementDirection directionArm = GetRotationDirection(inAngles[i]);
                    ScaraController.Instance.MoveJoint(i, directionArm);
                }
                else
                {
                    ScaraController.Instance.MoveJoint(i, MovementDirection.None);
                }
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
            Debug.Log("Connected to Serial Port: " + port);
        }
        catch (Exception err)
        {
            Debug.LogError("Error when trying to Open Serial Communication: " + err);
            //throw err;
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
                Debug.LogError("The connection is not open.");
            }

        }
        catch (IOException err)
        {
            Debug.LogError("Error when trying to close the connection: " + err);
        }
    }

    private void OnDestroy()
    {
       OnDisconnect();
    }

    public BTConnectionStatus getConnectionStatus()
    {
        if (_serialPort.IsOpen)
        {
            status = BTConnectionStatus.CONNECTED;
        }
        else
        {
            status = BTConnectionStatus.DISCONNECTED;
        }
        return status;
    }

    void setConfiguration()
    {
        _serialPort.ReadTimeout = READ_TIMEOUT;
        _serialPort.WriteTimeout = WRITE_TIMEOUT;
        _serialPort.Handshake = Handshake.None;
    }

    void parseIncomingMessage(string rawMessage)
    {
        if (rawMessage.StartsWith(':'))
        {
            string[] angleValues = rawMessage.Substring(1,rawMessage.Length - 1).Split(";");
            if (rawMessage != null)
            {
                inAngles[0] = float.Parse(angleValues[0]);
                inAngles[1] = float.Parse(angleValues[1]);
                inAngles[2] = float.Parse(angleValues[2]);

                Debug.Log("Angles {" + inAngles[0] + " ; " + inAngles[1] + " ; " + inAngles[2] + "}");
            }
        }
        else
        {
            Debug.LogError("Incomming message incorrect format: " + rawMessage);
        }
    }

    public void sendMessage(string message)
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.WriteLine(message);
            _serialPort.BaseStream.Flush();
        }
        else
        {
            Debug.LogError("Serial Port " + port + " is not Open.");
        }
    }

    public void SetComPort(string portName)
    {
        if (portName.Contains("COM"))
        {
            port = portName;
        }
        else
        {
            Debug.LogError("Incorrect port name. Format must be: COM<PORT_NUMBER>, not: " + portName);
        }
        
    }
}