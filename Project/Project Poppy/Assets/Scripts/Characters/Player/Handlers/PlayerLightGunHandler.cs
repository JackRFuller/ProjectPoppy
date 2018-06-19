using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightGunHandler : PlayerBehaviourHandler
{
	[SerializeField]
	private Transform m_lightBeamOrigin;

	[SerializeField]
	private GameObject m_lightBeamPrefab;
	private LightBeamHandler m_lightBeamHandler;
	private GameObject m_lightBeam;

	public void FireLightGun()
	{
		//Check if we're grounded
		if(!playerController.MovementController.collisions.below)
			return;

		if(m_lightBeam == null)
		{
			m_lightBeam = Instantiate(m_lightBeamPrefab,m_lightBeamOrigin.position,Quaternion.identity);
			m_lightBeamHandler = m_lightBeam.GetComponent<LightBeamHandler>();
		}
		else
		{
			m_lightBeam.transform.position = m_lightBeamOrigin.transform.position;
		}

		StartCoroutine(WaitToStartLightBeam());
	}	

	IEnumerator WaitToStartLightBeam()
	{
		yield return new WaitForSeconds(0.5f);
		Vector2 direction = ReturnLightDirection();
		m_lightBeamHandler.StartMoving(direction);
	}

	public void TurnOffLightGun()
	{
		m_lightBeamHandler.TurnOffLightBeam();
	}

	private Vector2 ReturnLightDirection()
	{
		Vector2 direction = Vector2.zero;

		float playerOrientation = playerController.MovementController.GetObjectOrientation();
		float playerDirection = playerController.MovementController.LastDirectionInput;

		if(playerOrientation == 0 || playerOrientation == 180)
		{
			if(playerOrientation == 180)
				playerDirection = -playerDirection;
			
			direction.x = playerDirection;
		}
		else
		{	
			if(playerOrientation == 270)		
				playerDirection = -playerDirection;

			direction.y = playerDirection;
		}

		return direction;
	}
}
