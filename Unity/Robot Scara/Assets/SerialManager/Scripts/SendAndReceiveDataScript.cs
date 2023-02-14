using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAndReceiveDataScript : MonoBehaviour
{
    public int pot;
    
    void Start()
    {
       SerialManagerScript.WhenReceiveDataCall  += ReceiveData;
    }

    private void ReceiveData(string incomingS)
    {
        int.TryParse(incomingS, out pot); // transformamos la información de llegada en entero 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SerialManagerScript.SendInfo("l");
        }
    }
}

