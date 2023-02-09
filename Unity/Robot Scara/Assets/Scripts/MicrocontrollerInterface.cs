using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrocontrollerInterface : MonoBehaviour
{
    public GameObject robot;
    ScaraController scaraController;
    public float[] speeds;
    public int moveJointNumber = 2;
    public int target;

    // Update is called once per frame

    private void Start()
    {
        scaraController = robot.GetComponent<ScaraController>();
    }
    void Update()
    {
        SetJointSpeed(scaraController.joints[moveJointNumber].robotPart, speeds[1]);
        SetJointTarget(scaraController.joints[moveJointNumber].robotPart, target);
        MovementDirection direction = GetRotationDirection(target);
        scaraController.MoveJoint(moveJointNumber, direction);
    }

    void SetJointSpeed(GameObject robotPart, float speed)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.SetArticulationSpeed(speed);
    }

    void SetJointTarget(GameObject robotPart, float target)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.SetArticulationTarget(target);
    }

    static MovementDirection GetRotationDirection(int inputVal)
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
}
