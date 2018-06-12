using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeleportController))]
public class KeyholeHandler : MonoBehaviour 
{
	[Header("Key Hole Objects")]
	[SerializeField]
	private GameObject[] m_keyHoleSprites;
	[SerializeField]
	private SpriteRenderer[] m_keyHoleFill;

	public void InitKeyHoles(int keyHoleCount)
	{
		for(int i = 0; i < keyHoleCount; i++)
		{
			m_keyHoleSprites[i].SetActive(true);
		}

		for(int i = 0; i < m_keyHoleFill.Length; i++)
		{
			m_keyHoleFill[i].enabled = false;
		}
	}	

	public void ActivateKeyHole(int keyIndex)
	{
		m_keyHoleFill[keyIndex].enabled = true;
	}

	public void DeactivateKeyHole(int keyIndex)
	{
		m_keyHoleFill[keyIndex].enabled = false;
	}
}
