using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventView : MonoBehaviour
{
	[SerializeField]
	private IntroEventHandler m_introEvent;

	public IntroEventHandler IntroEventHandler {get {return m_introEvent;}}	

	private void Start()
	{
		StartCoroutine(TriggerIntro());
	}

	private IEnumerator TriggerIntro()
	{
		yield return new WaitForSeconds(2.0f);
		m_introEvent.StartEvent();
	}

	
}
