using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
	protected GameEventView m_gameEventView;
	protected PlayerController m_player;

	//Components	
	protected Animator m_gameEventAnimController;
	protected AudioSource m_audio;

	protected virtual void Start()
	{
		m_gameEventAnimController = GetComponent<Animator>();
		m_audio = GetComponent<AudioSource>();

		m_gameEventView = Manager.Instance.GameEventView;
		m_player = Manager.Instance.LevelManager.PlayerController;
	}

	public virtual void StartEvent()
	{

	}

	protected IEnumerator Wait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}

	protected void PlayAudio(AudioClip audioClip)
	{
		m_audio.clip = audioClip;
		m_audio.Play();
	}
}
