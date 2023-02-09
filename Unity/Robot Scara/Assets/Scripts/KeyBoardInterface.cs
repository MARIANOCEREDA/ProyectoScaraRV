using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 This module manages the interface between the Microcontroller and
the Unity VR model.

author: Mariano Cereda and Facundo Cardenas
 */

public class KeyBoardInterface : MonoBehaviour
{
    public GameObject robot;
    public float[] speeds;

    void Update()
    {
        ScaraController scaraController = robot.GetComponent<ScaraController>();
        for (int i = 0; i < scaraController.joints.Length; i++)
        {
            float inputVal = Input.GetAxis(scaraController.joints[i].inputAxis);
            SetJointSpeed(scaraController.joints[i].robotPart, speeds[i]);
            if (Mathf.Abs(inputVal) > 0)
            {
                MovementDirection direction = GetRotationDirection(inputVal);
                scaraController.MoveJoint(i, direction);
                return;
            }
        }
        scaraController.StopAllJointMovement();
    }

    // HELPERS

    void SetJointSpeed(GameObject robotPart, float speed)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.SetArticulationSpeed(speed);
    }

    static MovementDirection GetRotationDirection(float inputVal)
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



