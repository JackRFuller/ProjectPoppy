using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Behaviour : Entity
{
    public virtual void ActivateBehaviour()
    {

    }
}

[System.Serializable]
public class Vector3Lerping
{
    [HideInInspector] public Vector3 pointA;
    [HideInInspector] public Vector3 pointB;
    [HideInInspector] public float timeStarted;
    [HideInInspector] public bool isLerping = false;
    [SerializeField] private float lerpSpeed = 1;
    public AnimationCurve lerpCurve;


    public float ReturnLerpProgress()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percenatageComplete = timeSinceStarted / lerpSpeed;

        return percenatageComplete;
    }
}
