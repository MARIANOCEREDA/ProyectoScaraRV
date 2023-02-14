using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class SerialManagerScript : MonoBehaviour
{
    public String com; //1

    public delegate void SerialEvent(string incomingString); //3
    public static event SerialEvent WhenReceiveDataCall; //3

    private bool abort; //1
    private static SerialPort port;	//1
    private Thread serialThread;  //1
    private SynchronizationContext mainThread;  //6

    private char incomingChar; //2
    private string incomingString;    //2

    void OnEnable()
    {
        port = new SerialPort(com, 9600); //1
        port.Open();//1
        port.DiscardOutBuffer(); //1
        port.DiscardInBuffer(); //1
        port.ReadTimeout = 300; //5
        mainThread = SynchronizationContext.Current;  //6

        if(mainThread == null)  //6
        {
            mainThread = new SynchronizationContext();  //6
        }

        serialThread = new Thread(Receive); //1

        if (port.IsOpen)	//1
        {
            serialThread.Start();
        }
    }

    void Receive() //1
    {
        while (true)
        {
            if (abort)	//1
            {
                serialThread.Abort();
                break;
            }

            try //2
            {
                incomingChar = (char)port.ReadChar();                                               
            }

            catch (Exception e) { } //2

            if (!incomingChar.Equals('\n')) //2
            {
                incomingString += incomingChar;
            }

            else //3
            {
                //todo esto se ejecuta en el hilo principal pero se llama desde el secundario//
                mainThread.Send((object state) => //6
                {
                    if (WhenReceiveDataCall != null)
                    {
                        WhenReceiveDataCall(incomingString);
                    }                    
                }, null);
                ///////////////////////////////////////////////////////////////////////////////
                incomingString = "";
            }
        }
    }

    public static void SendInfo(string infoToSend) //4
    {
        port.Write(infoToSend);
    }

    void OnDestroy() //5
    {
        OnApplicationQuit(); //5
    }

    private void OnApplicationQuit() //1
    {
        try
        {
            abort = true;
            port.DiscardOutBuffer();
            port.DiscardInBuffer();
            port.Close();
        }
        catch (Exception e) { }        
    }
}
