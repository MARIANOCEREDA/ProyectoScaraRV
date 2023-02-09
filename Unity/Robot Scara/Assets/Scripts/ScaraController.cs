using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ScaraController : MonoBehaviour
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

    public void MoveJoint(int jointIndex, MovementDirection direction)
    {
        Debug.Log("Moving joint number ... " + jointIndex);
        StopAllJointMovement();
        Joint joint = joints[jointIndex];
        UpdateMovementState(direction, joint.robotPart);
    }

    // HELPERS

    static void UpdateMovementState(MovementDirection direction, GameObject robotPart)
    {
        Debug.Log("Updating robot part movement ... " + robotPart);
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.movementState = direction;
        jointController.ExecuteMovement();
    }
}