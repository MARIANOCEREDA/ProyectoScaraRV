using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndEffectorController : MonoBehaviour
{
    public MovementDirection rotationState = MovementDirection.None;
    public float speed = 300.0f;

    private ArticulationBody articulation;


    // LIFE CYCLE

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    void FixedUpdate()
    {
        if (rotationState != MovementDirection.None)
        {
            float translationChange = (float)rotationState * speed * Time.fixedDeltaTime;
            float translationGoal = GetCurrentPosition() + translationChange;
            TranslateTo(translationGoal);
        }


    }

    // MOVEMENT HELPERS

    float GetCurrentPosition()
    {
        float currentPosition = articulation.jointPosition[0];
        return currentPosition;
    }

    void TranslateTo(float target)
    {
        var drive = articulation.xDrive;
        drive.target = target;
        articulation.xDrive = drive;
    }

}

