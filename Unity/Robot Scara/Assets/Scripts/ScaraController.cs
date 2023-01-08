using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotController : MonoBehaviour
{
    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;


    // CONTROL

    public void StopAllJointMovement()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            UpdateMovementState(MovementDirection.None, robotPart);
        }
    }

    public void RotateJoint(int jointIndex, MovementDirection direction)
    {
        StopAllJointMovement();
        Joint joint = joints[jointIndex];
        UpdateMovementState(direction, joint.robotPart);
    }

    // HELPERS

    static void UpdateMovementState(MovementDirection direction, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.rotationState = direction;
    }

}