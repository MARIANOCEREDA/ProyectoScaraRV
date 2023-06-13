using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationPauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject PauseMenuPanel;
    [SerializeField] public GameObject ConfigurationPausePanel;

    public void BackToMenu()
    {
        ConfigurationPausePanel.SetActive(false);
        PauseMenuPanel.SetActive(true);
        Debug.Log("Pause Menu Panel Active.");
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

    private float[] parseLimitString(string rawLimits)
    {
        string[] limits = rawLimits.Split(";");

        // Parse string inputs to float
        float lowerLimit = (float)Convert.ToDouble(limits[0]);
        float upperLimit = (float)Convert.ToDouble(limits[1]);

        float[] parsedLimits = { lowerLimit, upperLimit };

        return parsedLimits;
    }

    public void OnResetDirection()
    {
        BluetoothManager.Instance.sendMessage("R");
    }
}
