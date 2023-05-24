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
    public float[] speeds;

    void Update()
    {

        if (ScaraController.Instance != null)
        {
            //ScaraController scaraController = robot.GetComponent<ScaraController>();
            for (int i = 0; i < ScaraController.Instance.joints.Length; i++)
            {
                float inputVal = Input.GetAxis(ScaraController.Instance.joints[i].inputAxis);
                SetJointSpeed(ScaraController.Instance.joints[i].robotPart, speeds[i]);
                if (Mathf.Abs(inputVal) > 0)
                {
                    MovementDirection direction = GetRotationDirection(inputVal);
                    ScaraController.Instance.MoveJoint(i, direction);
                    return;
                }
            }
            ScaraController.Instance.StopAllJointMovement();
        }
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



