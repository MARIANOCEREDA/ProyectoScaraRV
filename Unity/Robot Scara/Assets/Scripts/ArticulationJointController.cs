using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float movementSpeed = 300.0f;
    private ArticulationBody articulation;


    // LIFE CYCLE

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    void FixedUpdate()
    {
        float change = (float)movementState * movementSpeed * Time.fixedDeltaTime;
        float goal = CurrentPrimaryAxisRotation() + change;
        MoveTo(goal);
    }

    // Setters

    public void SetMovementSpeed(List<float> targetsList)
    {
        articulation.SetDriveTargetVelocities(targetsList);
    }

    public void SetMovementTargets(List<float> targetsList)
    {
        articulation.SetDriveTargets(targetsList);
        Debug.Log("Target Position Y: " + targetsList.ToArray()[1]);
    }

    // MOVEMENT HELPERS

    float CurrentPrimaryAxisRotation()
    {
        float currentRads = articulation.jointPosition[0];
        float currentMovement = Mathf.Rad2Deg * currentRads;
        return currentMovement;
    }

    public void MoveTo(float goal)
    {
        var drive = articulation.xDrive;
        drive.target = goal;
        articulation.xDrive = drive;
    }
}