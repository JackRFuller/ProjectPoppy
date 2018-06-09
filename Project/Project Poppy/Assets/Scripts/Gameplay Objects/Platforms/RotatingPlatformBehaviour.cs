using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformBehaviour : Behaviour
{
    public Vector3Lerping lerpingAttributes;

    public override void ActivateBehaviour()
    {
        if(lerpingAttributes.isLerping)
            return;
        
        lerpingAttributes.pointA = transform.rotation.eulerAngles;
        lerpingAttributes.pointB = new Vector3(0,0,transform.eulerAngles.z + 90f);
        lerpingAttributes.timeStarted = Time.time;

        lerpingAttributes.isLerping = true;
    }

    private void Update()
    {
        RotatePlatform();
    }

    private void RotatePlatform()
    {
        if(!lerpingAttributes.isLerping)
            return;

        float percentageComplete = lerpingAttributes.ReturnLerpProgress();
        Vector3 newRot = Vector3.Lerp(lerpingAttributes.pointA, lerpingAttributes.pointB,
                                      lerpingAttributes.lerpCurve.Evaluate(percentageComplete));

        SetRotation(newRot);

        if (percentageComplete >= 1.0f)
        {
            lerpingAttributes.isLerping = false;
        }

    }
}
