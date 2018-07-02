using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightDetectionHandler : MonoBehaviour
{
	private PlayerController m_player;

	[SerializeField]
	private m_Side m_detectionSide;
	private enum m_Side {Left, Right}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			if(m_player == null)
				m_player = other.transform.GetComponent<PlayerController>();

			Debug.Log("Detected Player");
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		
	}
}
