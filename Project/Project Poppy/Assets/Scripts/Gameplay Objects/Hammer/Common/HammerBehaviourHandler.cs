using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBehaviourHandler : Entity
{
	protected HammerView hammerView;

	protected virtual void Start()
	{
		hammerView = GetComponent<HammerView>();
	}
	
}
