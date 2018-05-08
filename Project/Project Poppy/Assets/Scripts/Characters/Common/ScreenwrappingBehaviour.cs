using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenwrappingBehaviour : MonoBehaviour
{
    private Camera mainCamera;

    //Screen Wrapping
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private int wrappingCount = 0;    

    private void Start()
    {
        mainCamera = Camera.main;
    }   

    public void ScreenWrap()
    {
        if (isVisible())
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingY && isWrappingX)
            return;

        Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(this.transform.position);
        Vector3 newPosition = this.transform.position;


        if (!isWrappingX && viewPortPosition.x > 1 || viewPortPosition.x < 0)
        {
            newPosition.x = -newPosition.x + 1.5f;
            isWrappingX = true;           
        }

        if (!isWrappingY && viewPortPosition.y > 1 || viewPortPosition.y < 0)
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;            
        }       

        this.transform.position = newPosition;        
    }

    private bool isVisible()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(this.transform.position);
        bool _isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return _isVisible;
    }
}
