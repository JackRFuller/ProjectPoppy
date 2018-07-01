using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntroEventHandler : GameEvent
{
	[SerializeField]
	private bool m_introEnabled = false;
	public bool IntroEnabled {get {return m_introEnabled;}}

	[Header("Designer Controls")]
	[SerializeField]
	private float m_waitForBellRing = 1.0f;
	[SerializeField]
	private float m_waitToFadeFromBlackTime = 0.5f;
	[SerializeField]
	private float m_waitToGetUpTime = 2.0f;
	[SerializeField]
	private float m_waitToEnablePlayer= 10f;

	[Header("Audio")]
	[SerializeField]
	private AudioClip bellRing;

	//For Debug Purposes - find better way later
	[SerializeField]
	private Animator m_playerAnim;

	protected override void Start()
	{
		base.Start();

		if(!m_introEnabled)
		{			
			m_player.AnimationController.SetDebugIntroStance(true);				
		}
		else
		{
			m_player.AnimationController.SetDebugIntroStance(false);	
			m_gameEventAnimController.SetBool("BlackOverlay",true);	
		}		
	}

	public override void StartEvent()
	{
		if(!m_introEnabled)
		{	
			return;
		}
		else
		{
			m_player.AnimationController.SetDebugIntroStance(false);	
		}			

		StartCoroutine("RunIntroEvent");
	}

	private IEnumerator RunIntroEvent()
	{	
		Debug.Log("Intro Started");		
		
		m_player.MovementController.FreezePlayerMovement();
		yield return Wait(m_waitForBellRing);
		PlayAudio(bellRing);
		yield return Wait(m_waitToFadeFromBlackTime);
		m_gameEventAnimController.SetTrigger("FadeFromBlack");
		yield return Wait(m_waitToGetUpTime);
		m_player.AnimationController.PlayerStandUp();
		yield return Wait(m_waitToEnablePlayer);
		m_player.MovementController.UnFreezePlayerMovement();
	}
}
