using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeamHandler : Entity
{
	private ScreenwrappingBehaviour m_screenWrapBehaviour;
	private TrailRenderer m_trailRenderer;

	private Rigidbody2D m_rigidBody;
	private Vector2 m_velocity;
	private Vector2 m_movementDirection;
	private const float m_speed = 10;

	//Screenwrapping Variables	
    private bool m_isWrappingX = false;
    private bool m_isWrappingY = false;

	private GameObject m_connectedLightBeam;
	private LightBeamHandler m_connectedLightBeamHandler;

	private string[] m_interactableTags = new string[] {"Character"};

	private Camera m_mainCamera;

	

	public void StartMoving(Vector2 direction)
	{	
		if(m_mainCamera == null)
			m_mainCamera = Camera.main;

		if(m_trailRenderer == null)
			m_trailRenderer = GetComponent<TrailRenderer>();

		if(m_screenWrapBehaviour == null)
			m_screenWrapBehaviour = GetComponent<ScreenwrappingBehaviour>();

		if(m_rigidBody == null)
			m_rigidBody = GetComponent<Rigidbody2D>();

		m_trailRenderer.Clear();

		if(!IsObjectActive())
			ShowObject();

		this.enabled = true;
		m_movementDirection = direction;
		m_velocity = m_movementDirection * m_speed;
		m_rigidBody.velocity = m_velocity;
	}

	public void TurnOffLightBeam()
	{
		m_rigidBody.velocity = Vector2.zero;

		if(m_connectedLightBeamHandler)
			m_connectedLightBeamHandler.TurnOffLightBeam();

		HideObject();
	}

	void Update()
	{
		CheckForEdgeOfScreen();
	}

	void CheckForEdgeOfScreen()
	{
		if (CheckIfLightBeamIsAtEdgeOfScreen())
		{
			m_isWrappingX = false;
			m_isWrappingY = false;
			return;
		}

		Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(this.transform.position);
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

			//newPosition.x += offset;
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

		newPosition = Camera.main.ViewportToWorldPoint(viewPortPosition);  

		if(m_isWrappingY)
		{
			newPosition.y += offset;
		}

		if(m_isWrappingX)
		{
			newPosition.x += offset;
		}

		if(m_connectedLightBeam == null)
		{
			m_connectedLightBeam = Instantiate(this.gameObject,newPosition,Quaternion.identity);
			m_connectedLightBeamHandler = m_connectedLightBeam.GetComponent<LightBeamHandler>();
		} 

		m_connectedLightBeam.transform.position = newPosition;
		m_connectedLightBeamHandler.StartMoving(m_movementDirection);
       
	   	m_rigidBody.velocity = Vector2.zero;
		this.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "PowerNode")
		{
			other.gameObject.SendMessage("HitByLightBeam",SendMessageOptions.DontRequireReceiver);
			m_rigidBody.velocity = Vector2.zero;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.tag != "ScreenBorder")
		{
			Debug.Log(other.gameObject.name);
			m_rigidBody.velocity = Vector2.zero;

			other.gameObject.SendMessage("HitByLightBeam",SendMessageOptions.DontRequireReceiver);
		}
	}

	private bool CheckIfLightBeamIsAtEdgeOfScreen()
	{
		Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
        bool _isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return _isVisible;
	}
	
}
