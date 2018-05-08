using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour
{
    private CameraController cameraController;

    [Header("Moving Curves")]
    [SerializeField]
    private AnimationCurve movingCurve;
    [SerializeField]
    private AnimationCurve rotatingCurve;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private const float movementSpeed = 3;

    private Vector3 startingRotation;
    private Vector3 targetRotation;
    private const float rotationSpeed = 2.5f;

    private float timeStarted;

    private MovementState movementState = MovementState.Fixed;
    private enum MovementState
    {
        Fixed,
        Rotating,
        Moving,
    }

    public void InitMovement(Vector3 targetPos, Vector3 targetRot)
    {
        if (cameraController == null)
            cameraController = this.GetComponent<CameraController>();

        startingPosition = this.transform.position;
        startingRotation = this.transform.eulerAngles;

        targetPosition = new Vector3(targetPos.x,targetPos.y,this.transform.position.z);
        targetRotation = targetRot;

        timeStarted = Time.time;

        float tempZ = -targetRot.z;
        Vector3 tempRot = new Vector3(0, 0, 360 - tempZ);


        Debug.Log(tempRot);
        Debug.Log(this.transform.eulerAngles);

        if (tempRot == this.transform.eulerAngles)
        {
            movementState = MovementState.Moving;
        }
        else
        {
            movementState = MovementState.Rotating;
        }

       
        this.enabled = true;
    }

    private void Update()
    {
        switch(movementState)
        {            
            case MovementState.Rotating:
                RotateCamera();               
                break;
            case MovementState.Moving:
                MoveCamera();
                break;
        }
    }

    private void RotateCamera()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percenatgeComplete = timeSinceStarted / rotationSpeed;

        Vector3 newRot = Vector3.Lerp(startingRotation, targetRotation, rotatingCurve.Evaluate(percenatgeComplete));
        this.transform.rotation = Quaternion.Euler(newRot);

        if(percenatgeComplete >= 1.0f)
        {
            timeStarted = Time.time;
            movementState = MovementState.Moving;
        }
    }

    private void MoveCamera()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percenatgeComplete = timeSinceStarted / movementSpeed;

        Vector3 newPos = Vector3.Lerp(startingPosition, targetPosition, movingCurve.Evaluate(percenatgeComplete));
        this.transform.position = newPos;

        if(percenatgeComplete >= 1.0f)
        {
            movementState = MovementState.Fixed;
            cameraController.CameraIsInNewPosition();
            this.enabled = false;
        }
    }




}
