using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : Behaviour
 {
	 protected PlatformController m_platformController;

	// Use this for initialization
	protected virtual void Start ()
	{
		m_platformController = GetComponent<PlatformController>();
	}
	
	
}
