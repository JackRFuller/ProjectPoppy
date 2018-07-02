using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovementBehaviour : CameraBehaviour
{
    public UnityAction startedMoving;
    public UnityAction reachDestination;

    [SerializeField]
    private Vector3Lerping lerpingAttributes;

    private const float xOffset = 35.0f;
    private const float yOffset = 16.0f;

    /// <summary>
    /// Starts Camera Scrolling
    /// </summary>
    /// <param name="direction">0 = right, 1 = left, 2 = top, 3 = bottom</param>
    public void InitCameraScrolling(Vector2 direction)
    {
        lerpingAttributes.pointA = this.transform.position;
        Vector3 targetPosition = this.transform.position;

        Vector2 currentLevelIndex = Manager.Instance.LevelManager.CurrentLevelIndex;

        if (currentLevelIndex.x != direction.x)
        {
            if(direction.x > currentLevelIndex.x)
                targetPosition.x = this.transform.position.x + xOffset;
            else
            {
                targetPosition.x = this.transform.position.x - xOffset;
            }
        }
        else if(currentLevelIndex.y != direction.y)
        {
            if(direction.y > currentLevelIndex.y)
                targetPosition.y = this.transform.position.y + yOffset;
            else
            {
                targetPosition.y = this.transform.position.y - yOffset;
            }
        }

        lerpingAttributes.pointB = targetPosition;
        lerpingAttributes.timeStarted = Time.time;

        TurnOnComponent();

        if(startedMoving != null)
            startedMoving();
      
        Debug.Log(lerpingAttributes.pointB);


        //Debug.Log(lerpingAttributes.pointA + " + " + lerpingAttributes.pointB);
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        float percentageComplete = lerpingAttributes.ReturnLerpProgress();
        Vector3 newPos = Vector3.Lerp(lerpingAttributes.pointA,lerpingAttributes.pointB,lerpingAttributes.lerpCurve.Evaluate(percentageComplete));

        SetPosition(newPos);

        if (percentageComplete >= 1.0f)
        {
            SetPosition(lerpingAttributes.pointB);

            TurnOffComponent();

            if (reachDestination != null)
                reachDestination();
        }
    }


}
