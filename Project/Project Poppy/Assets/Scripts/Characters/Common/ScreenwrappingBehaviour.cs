using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenwrappingBehaviour : MonoBehaviour
{
    private Camera mainCamera;
    private Controller2D controller;

    //Screen Wrapping
    private bool m_isWrappingX = false;
    private bool m_isWrappingY = false;
    private int wrappingCount = 0;    

    private void Start()
    {
        controller = GetComponent<Controller2D>();
        mainCamera = Camera.main;
    }
    
    public void ScreenWrap()
    {
        if(Manager.Instance.LevelManager.CameraController.ScrollingBehaviour.IsScrolling)
            return;

        if(isVisible())
        {
			m_isWrappingX = false;
			m_isWrappingY = false;
			return;
		}

        if (m_isWrappingY && m_isWrappingX)
            return;

        
        if(controller)
        {
            if(controller.collisions.fullyGrounded)
                return;
        }       

		Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(this.transform.position);
        Vector3 newPosition = this.transform.position;

		float offset = 0.1f;

        if (!m_isWrappingX && viewPortPosition.x > 1 || viewPortPosition.x < 0)
        {
			if(viewPortPosition.x < 0)
			{
				viewPortPosition.x = 1f;
				offset = -offset;
			}
			else
			{
				viewPortPosition.x = 0f;
			}			
            m_isWrappingX = true;           
        }

        if (!m_isWrappingY && viewPortPosition.y > 1 || viewPortPosition.y < 0)
        {
			if(viewPortPosition.y < 0)
			{
				viewPortPosition.y = 1f;
				offset = -offset;
			}
			else
			{
				viewPortPosition.y = 0f;
			}           
            m_isWrappingY = true;            
        } 

		newPosition = mainCamera.ViewportToWorldPoint(viewPortPosition);  

		if(m_isWrappingY)
		{
			newPosition.y += offset;
		}

		if(m_isWrappingX)
		{
			newPosition.x += offset;
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
