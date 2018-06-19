using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNodeHandler : Entity
{
	[SerializeField]
	private SpriteRenderer powerNodeFillSprite;

	[SerializeField]
	private PlatformController[] m_platformControllers;

	private void Start()
	{
		powerNodeFillSprite.enabled = false;
	}

	private void HitByLightBeam()
	{
		powerNodeFillSprite.enabled = true;

		for(int i = 0; i < m_platformControllers.Length; i++)
		{
			m_platformControllers[i].ActivateBehaviour();
		}
	}
	
}
