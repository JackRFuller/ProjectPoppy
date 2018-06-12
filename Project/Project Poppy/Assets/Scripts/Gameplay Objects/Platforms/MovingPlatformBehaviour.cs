using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehaviour : PlatformBehaviour
{
    [SerializeField]
    private GameObject platformPathPrefab;

    [SerializeField] private PlatformPathBehaviour platformPath;

    [SerializeField]
    public Vector3Lerping pathAttributes;
    private int pathIndex = 0;
    private Vector3 startingPosition;
    private Vector3 targetPosition;

    public override void ActivateBehaviour()
    {
        if(pathAttributes.isLerping)
            return;

        startingPosition = transform.position;

        if (pathIndex == 0)
            targetPosition = pathAttributes.pointB;

        if (pathIndex == 1)
            targetPosition = pathAttributes.pointA;

        pathAttributes.timeStarted = Time.time;
        pathAttributes.isLerping = true;
        this.enabled = true;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if(!pathAttributes.isLerping)
            return;

        float percentageComplete = pathAttributes.ReturnLerpProgress();
        Vector3 newPos = Vector3.Lerp(startingPosition, targetPosition,
                                      pathAttributes.lerpCurve.Evaluate(percentageComplete));

        SetPosition(newPos);

        if (percentageComplete >= 1.0f)
        {
            pathIndex++;

            if (pathIndex > 1)
                pathIndex = 0;

            pathAttributes.isLerping = false;
            this.enabled = false;

            m_platformController.BehaviourEnded();
        }
    }


    #region Editor Functions
    public void SetPointA()
    {
        pathAttributes.pointA = transform.position;
    }

    public void SetPointB()
    {
        pathAttributes.pointB = transform.position;
    }

    public void ResetToStartPosition()
    {
        transform.position = pathAttributes.pointA;
    }

    public void CreatePlatformPath()
    {
        if (platformPath)
        {
            DestroyImmediate(platformPath.gameObject);
        }

        GameObject path = Instantiate(platformPathPrefab,this.transform.parent);
        platformPath = path.GetComponent<PlatformPathBehaviour>();
    }

    public void SetPlatformPath()
    {
        platformPath.SetPlatformPath(pathAttributes.pointA, pathAttributes.pointB);
    }

    #endregion

}
