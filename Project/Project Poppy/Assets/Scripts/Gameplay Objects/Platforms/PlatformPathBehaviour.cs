using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPathBehaviour : Entity
{
    [SerializeField] private Transform pathPointA;
    [SerializeField] private Transform pathPointB;
    [SerializeField] private Transform path;

    public void SetPlatformPath(Vector3 pointA, Vector3 pointB)
    {
        pathPointA.position = pointA;
        pathPointB.position = pointB;

        Vector3 midPoint = (pointA + pointB) * 0.5f;
        path.position = midPoint;

        float pathSize = Vector3.Distance(pointA, pointB);
        Vector3 pathLength = Vector3.zero;
        Vector3 pathRotation = Vector3.zero;

        if (pointA.x != pointB.x)
        {
            pathRotation.z = 90.0f;
        }
        else if (pointA.y != pointB.y)
        {
            pathRotation.z = 0f;
        }

        Vector3 pathPosition = new Vector3(this.transform.position.x,this.transform.position.y,1);
        pathLength = new Vector3(0.15f,pathSize,1);
        path.localScale = pathLength;
        path.rotation = Quaternion.Euler(pathRotation);
        SetPosition(pathPosition);
    }
}
