using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class TeleportController : Entity 
{
	private PlayerController characterController;
	private BoxCollider2D telelportCollider;
	private KeyholeHandler keyHoleHandler;
	
	[Header("Teleport Attributes")]
	[SerializeField]
	private TeleportController m_pairedTeleporter;

	[Header("Key Attributes")]
	[SerializeField]
	private int m_keysNeededToUnlock;
	public int KeysNeededToUnlock {get {return m_keysNeededToUnlock;}}
	private int m_currentNumberOfKeys;	

	[Header("Door Attributes")]
	[SerializeField]
	private Transform m_doorTransform;
	[SerializeField]
	private Transform m_doorOpenTransform;
	public Vector3Lerping m_lerpingAttributes;

	private bool m_teleporterDeactivated;

	private void Start()
	{
		keyHoleHandler = GetComponent<KeyholeHandler>();
		telelportCollider = GetComponent<BoxCollider2D>();		

		keyHoleHandler.InitKeyHoles(m_keysNeededToUnlock);

		m_lerpingAttributes.startingPoint = m_doorTransform.position;		
	}

	//Used by the paired teleporter to stop the player being instantly teleported between the two
	public void DeactivateTeleport()
	{
		m_teleporterDeactivated = true;
	}

	public void KeyActivated()
	{
		keyHoleHandler.ActivateKeyHole(m_currentNumberOfKeys);

		m_currentNumberOfKeys++;

		if(m_currentNumberOfKeys == m_keysNeededToUnlock)
		{
			InitDoorMovement(true);
		}
	}

	public void KeyDeactivated()
	{		
		m_currentNumberOfKeys--;
		keyHoleHandler.DeactivateKeyHole(m_currentNumberOfKeys);

		if(m_currentNumberOfKeys < m_keysNeededToUnlock)
		{
			InitDoorMovement(false);
		}
	}

	private void InitDoorMovement(bool isOpening)
	{		
		m_lerpingAttributes.pointA = m_doorTransform.position;

		if(isOpening)
		{
			m_lerpingAttributes.pointB = m_doorOpenTransform.position;			
		}
		else
		{
			m_lerpingAttributes.pointB = m_lerpingAttributes.startingPoint;
		}
		
		m_lerpingAttributes.timeStarted = Time.time;
		m_lerpingAttributes.isLerping = true;
	}

	private void Update()
	{
		MoveDoor();
	}

	private void MoveDoor()
	{
		if(!m_lerpingAttributes.isLerping)
			return;

		float percentageComplete = m_lerpingAttributes.ReturnLerpProgress();
		Vector3 newDoorPosition = Vector3.Lerp(m_lerpingAttributes.pointA,m_lerpingAttributes.pointB,m_lerpingAttributes.lerpCurve.Evaluate(percentageComplete));

		m_doorTransform.position = newDoorPosition;

		if(percentageComplete >= 1.0f)
		{
			m_lerpingAttributes.isLerping = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(m_teleporterDeactivated)
			return;

		if(other.tag == "Character")
		{
			if(m_currentNumberOfKeys == m_keysNeededToUnlock)
			{
					if(characterController == null)
				characterController = other.transform.GetComponent<PlayerController>();			

				TeleportCharacter();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Character")
		{
			if(m_teleporterDeactivated)
				m_teleporterDeactivated = false;
		}
	}

	private void TeleportCharacter()
	{
		Vector3 targetPosition = m_pairedTeleporter.transform.position;
		Vector3 targetRotation = m_pairedTeleporter.transform.eulerAngles;

		m_pairedTeleporter.DeactivateTeleport();

		characterController.MovementController.TeleportPlayer(targetPosition,targetRotation);
	}
}
