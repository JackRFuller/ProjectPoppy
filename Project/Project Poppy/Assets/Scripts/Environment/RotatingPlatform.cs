using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : DynamicPlatform
{
    private Vector3 targetPosition;
    private Vector3 startingPosition;

    private float speed = 0.5f;
    private float timeStarted;
    private bool isMoving;


	// Update is called once per frame
	void Update ()
    {
        if(!isMoving)
        {
            if (Input.GetKey(KeyCode.R))
                InitiateAction();
        }
        else
        {
            PerformAction();
        }
    }
       

    private void InitiateAction()
    {
        if(platformBehaviourTriggered != null)
            platformBehaviourTriggered();

        startingPosition = this.transform.eulerAngles;
        targetPosition = new Vector3(startingPosition.x,
                                     startingPosition.y,
                                     startingPosition.z + 180);

        timeStarted = Time.time;
        isMoving = true;
    }

    private void PerformAction()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / speed;

        Vector3 newRot = Vector3.Lerp(startingPosition, targetPosition, percentageComplete);
        this.transform.rotation = Quaternion.Euler(newRot);

        if (percentageComplete >= 1.0f)
        {
            isMoving = false;

            if (platformBehaviourEnded != null)
                platformBehaviourEnded();
        }
    }
}
