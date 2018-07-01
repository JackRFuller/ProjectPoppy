using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHammerThrowHandler : MonoBehaviour
{
	private PlayerController m_playerController;

	[SerializeField]
	private GameObject m_hammerPrefab;

	[SerializeField]
	private Transform m_hammerSpawnPoint;
	private HammerView m_hammerView;

	// Use this for initialization
	void Start () 
	{
		m_playerController = GetComponent<PlayerController>();
	}	

	public void ThrowHammer(Vector2 direction)
	{
		if(m_hammerView == null)
		{
			GameObject hammer = Instantiate(m_hammerPrefab, m_hammerSpawnPoint.position, Quaternion.identity);
			m_hammerView = hammer.GetComponent<HammerView>();
		}

		m_hammerView.transform.position = m_hammerSpawnPoint.position;
		m_hammerView.InitiateHammer(direction);
	}

	public void SlamHammer()
	{

	}


	
}
