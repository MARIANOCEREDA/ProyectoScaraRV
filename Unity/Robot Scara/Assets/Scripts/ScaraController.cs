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

    void Start()
    {

    }

    void Update()
    {
        // Assign movement direction to the joints
        float[] target = { 20, -50, -80, 0.11f };
        float[] jointTargets = { 0, 0, 0 };
        List<float> jointTargetsList = new List<float>(jointTargets);
        MovementDirection movementDirection;

        // Move Joints
        movementDirection = MovementDirection.Negative;
        for (int i = 0; i < joints.Length - 1; i++)
        {
            float inputVal = Input.GetAxis(joints[i].inputAxis);
            if (Mathf.Abs(inputVal) > 0)
            {
                MovementDirection direction = movementDirection;
                MoveJoint(i, direction, jointTargetsList);
                return;
            }
        }

    }


    // CONTROL

    public void StopAllJointMovement(List<float> target)
    {
        for (int i = 0; i < joints.Length - 1; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            UpdateMovementState(MovementDirection.None, robotPart, target) ;
        }
    }

    public void MoveJoint(int jointIndex, MovementDirection direction, List<float> target)
    {
        //StopAllJointMovement(target);
        Joint joint = joints[jointIndex];
        UpdateMovementState(direction, joint.robotPart, target);
    }

    // HELPERS

    static void UpdateMovementState(MovementDirection direction, GameObject robotPart, List<float> target)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        //jointController.SetMovementTargets(target);
        jointController.movementState = direction;
    }
}