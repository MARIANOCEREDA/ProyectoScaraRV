using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckJointLimit : MonoBehaviour
{
    // Start is called before the first frame update
    private ArticulationBody articulation;
    public GameObject jointLimitUI;
    //public GameObject jointPositionUI;

    private UpdateJointLimitColor jointLimitColor;
    //private DisplayJointPosition jointPositionText;

    private float currentPos, upperLimit, lowerLimit;
    private bool limitReached;

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
        jointLimitColor = jointLimitUI.GetComponent<UpdateJointLimitColor>();
        limitReached = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        var drive = articulation.xDrive;

        currentPos = drive.target;
        upperLimit = drive.upperLimit;
        lowerLimit = drive.lowerLimit;

        if(currentPos < lowerLimit && limitReached == false)
        {
            limitReached = true;
            Debug.Log("Lower Limit reached!");
            jointLimitColor.changeBackgroundColor();
            BluetoothManager.Instance.sendMessage("1");
        }
        else if (currentPos > upperLimit && limitReached == false)
        {
            Debug.Log("Upper Limit reached!");
            jointLimitColor.changeBackgroundColor();
            limitReached = true;
            BluetoothManager.Instance.sendMessage("1");
        }

        if(limitReached && currentPos > lowerLimit && currentPos < upperLimit && limitReached == true)
        {
            limitReached = false;
            Debug.Log("Reset color to white.");
            jointLimitColor.resetBackgroundColor();
            BluetoothManager.Instance.sendMessage("0");
        }
        
    }
}
