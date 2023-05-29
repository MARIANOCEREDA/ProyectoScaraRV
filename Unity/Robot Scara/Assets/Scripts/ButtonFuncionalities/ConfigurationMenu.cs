using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ConfigurationMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject MainMenuPanel;
    [SerializeField] public GameObject ConfigurationMenuPanel;

    public void BackToMenu()
    {
        ConfigurationMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        Debug.Log("Menu Panel Active.");
    }

    public void OnSetJ1(string j1Limit)
    {
        // Parse the incomming raw message with format 'lowerLimit;upperLimit'
        float[] floatLimits = parseLimitString(j1Limit);

        // Set the limits
        if (floatLimits[1] < 150 && floatLimits[1] > -150)
        {
            ScaraController.Instance.joints[0].robotPart.GetComponent<ArticulationJointController>().SetLimits(floatLimits[0], floatLimits[1]);
        }
        else
        {
            Debug.LogError("Joint limits must be between: [-150,150]");
        }
        
    }

    public void OnSetJ2(string j2Limit)
    {
        float[] floatLimits = parseLimitString(j2Limit);

        // Set the limits
        if (floatLimits[1] < 100 && floatLimits[1] > -200)
        {
            ScaraController.Instance.joints[1].robotPart.GetComponent<ArticulationJointController>().SetLimits(floatLimits[0], floatLimits[1]);
        }
        else
        {
            Debug.LogError("Joint limits must be between: [-200,100]");
        }
    }

    public void OnSetEf(string efLimit)
    {
        float[] floatLimits = parseLimitString(efLimit);

        // Set the limits
        if (floatLimits[1] < 0.055 && floatLimits[0] > -0.124)
        {
            ScaraController.Instance.joints[2].robotPart.GetComponent<ArticulationJointController>().SetLimits(floatLimits[0], floatLimits[1]);
        }
        else
        {
            Debug.LogError("Joint limits must be between: [-0.124,0.055]");
        }

    }

    public void OnSetBluetoothPort(string port)
    {
        Debug.Log("Bluetooth Port set to: " + port);
        BluetoothManager.Instance.SetComPort(port);
    }

    private float[] parseLimitString(string rawLimits)
    {
        string[] limits = rawLimits.Split(";");

        // Parse string inputs to float
        float lowerLimit = (float)Convert.ToDouble(limits[0]);
        float upperLimit = (float)Convert.ToDouble(limits[1]);

        float[] parsedLimits = {lowerLimit, upperLimit};

        return parsedLimits;
    }

}
