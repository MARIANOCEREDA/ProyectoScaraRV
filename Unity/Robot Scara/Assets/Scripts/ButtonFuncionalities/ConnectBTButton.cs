using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConnectBTButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;
    public Color connectedColor, disconnectedColor;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnConnectBluetooth()
    {
        BluetoothManager.Instance.OnConnect();

        if(BluetoothManager.Instance.getConnectionStatus() == BTConnectionStatus.CONNECTED)
        {
            Debug.Log("Setting button color to blue ...");
            image.color = connectedColor;
        }
        else
        {
            Debug.Log("Setting button color to red ...");
            image.color = disconnectedColor;
        }
    }
}
