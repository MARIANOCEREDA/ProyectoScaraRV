using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConnectBTButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject button;

    public void OnConnectBluetooth()
    {
        BluetoothManager.Instance.OnConnect();

        if (BluetoothManager.Instance.getConnectionStatus() == BTConnectionStatus.CONNECTED)
        {
            Debug.Log("Setting button color to blue ...");
            button.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            Debug.Log("Setting button color to red ...");
            button.GetComponent<Image>().color = Color.red;
        }
    }
}
