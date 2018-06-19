using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourHandler : Entity
{
	protected PlayerController playerController;

	protected virtual void Start()
	{
		playerController = GetComponent<PlayerController>();
	}	
}
