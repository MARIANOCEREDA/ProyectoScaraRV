using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection { None = 0, Positive = 1, Negative = -1 };

public class ArticulationJointController : MonoBehaviour
{
    /*
     *  ArticulationJointController
     *  
     *  It controls the movements of every joint.
     *  
     *  Documentation about Articulation Body element: https://docs.unity3d.com/Manual/class-ArticulationBody.html 
     * **/
    public MovementDirection movementState = MovementDirection.None;
    private float rotationSpeed = 300.0f;
    private float translationSpeed = 0.2f;
    public float target = 0;
    private ArticulationBody articulation;


    // LIFE CYCLE

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    public void ExecuteMovement()
    {
        if (movementState != MovementDirection.None)
        {
            Debug.Log("Joint position: " + articulation.jointPosition[0]);
            float change;
            float goal;
            switch (articulation.jointType)
            {
                case ArticulationJointType.RevoluteJoint:
                    change = (float)movementState * rotationSpeed * Time.fixedDeltaTime;
                    goal = CurrentPrimaryAxisRotation() + change;
                    MoveTo(goal);
                    break;

                case ArticulationJointType.PrismaticJoint:
                    change = (float)movementState * translationSpeed * Time.fixedDeltaTime;
                    goal = CurrentPrimaryAxisTranslation() + change;
                    MoveTo(goal);
                    break;

                default: break;
            }
        }
    }

    // MOVEMENT HELPERS

    public float CurrentPrimaryAxisRotation()
    {
        float currentRads = articulation.jointPosition[0];
        float currentMovement = Mathf.Rad2Deg * currentRads;
        return currentMovement;
    }

    public float CurrentPrimaryAxisTranslation()
    {
        float currentPost = articulation.jointPosition[0];
        return currentPost;
    }

    public void SetArticulationSpeed(float speed)
    {
        if (articulation.jointType == ArticulationJointType.RevoluteJoint)
        {
            rotationSpeed = speed;
        }
        else if (articulation.jointType == ArticulationJointType.RevoluteJoint)
        {
            translationSpeed = speed;
        }
    }

    public void SetArticulationTarget(float targetPoint)
    {
        target = targetPoint;
    }

    public void MoveTo(float goal)
    {
        var drive = articulation.xDrive;
        drive.target = goal;
        articulation.xDrive = drive;
    }
}