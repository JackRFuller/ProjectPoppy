using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMovementHandler : HammerBehaviourHandler
{
	private Rigidbody2D m_rb;
	private float m_movementSpeed = 10;
	private Vector2 m_movementDirection;

	protected override void Start()
	{
		base.Start();
		m_rb = GetComponent<Rigidbody2D>();		
	}	

	public void InitiateMovement(Vector2 intendedDirection)
	{
		m_movementDirection = intendedDirection;
		SetRotation(GetHammerRotation(intendedDirection));
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		Vector2 direction = m_movementDirection * m_movementSpeed;

		Vector2 rayOrigin = transform.position;
		Ray2D collisionRay = new Ray2D(rayOrigin, m_movementDirection);

        RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, 0.1f);
		Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.blue);		

		if(hit)
		{
			if(hit.collider.tag == "MoveableObject")
			{				
				hit.collider.SendMessage("HitByProjectile",m_movementDirection,SendMessageOptions.DontRequireReceiver);
			}
			HideObject();
		}
		else
		{
			m_rb.velocity = direction;
		}

		hammerView.ScreenwrappingBehaviour.ScreenWrap();			
	}

	private Vector3 GetHammerRotation(Vector2 direction)
	{
		Vector3 rotation = Vector3.zero;

		if(direction.x < 0)
		{
			rotation.z = 180.0f;
		}
		if(direction.y > 0)
		{
			rotation.z = 90.0f;
		}
		if(direction.y < 0)
		{
			rotation.z = -90.0f;
		}

		return rotation;
	}
}
