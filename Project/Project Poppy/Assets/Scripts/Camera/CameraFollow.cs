using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Controller2D target;
  
    void LateUpdate()
    {
        float orientation = target.GetObjectOrientation();

        Vector3 newPosition = Vector3.zero;
        if (orientation == 0 || orientation == 180)
        {
            newPosition = new Vector3(target.transform.position.x, this.transform.position.y, -10);
        }
        else
        {
            newPosition = new Vector3(this.transform.position.x, target.transform.position.y, -10);
        }
         
        //this.transform.position = newPosition;
    }    
}
