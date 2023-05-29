using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/**
 * Singleton instance of the robot.
 **/
public class ScaraController : MonoBehaviour
{
    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;
    private int N_ROTATION_JOINTS = 2;

    private static ScaraController instance;
    public static ScaraController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScaraController>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<ScaraController>();
                    singletonObject.name = typeof(ScaraController).ToString();
                    //DontDestroyOnLoad(singletonObject);
                }

            }
            return instance;
        }
    }


    private void Awake()
    {
        // Ensure that only one instance of the class exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        /*else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }*/
    }

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }
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

    /*public void SetProperties(GameObject robotPart, float lowerLimit = -90.0f, float upperLimit = 90.0f)
    {
        Debug.Log("Setting properties ... " + robotPart);
        ArticulationBody joint = robotPart.GetComponent<ArticulationBody>();
        joint.linearLockX = ArticulationDofLock.LimitedMotion;
        joint.linearLockY = ArticulationDofLock.LimitedMotion;

        var jointDrive = joint.xDrive;
        jointDrive.lowerLimit = lowerLimit;
        jointDrive.upperLimit = upperLimit;

        joint.xDrive = jointDrive;
    }*/

    public int GetNRotationJoints()
    {
        return N_ROTATION_JOINTS;
    }

}