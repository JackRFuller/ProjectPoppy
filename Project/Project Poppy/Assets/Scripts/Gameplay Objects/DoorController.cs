using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Attributes")]
    [SerializeField]
    private int doorID;

    [SerializeField]
    private int numberOfKeysNeeded;
    private int numberOfCurrentKeys;


    [Header("Door Components")]
    [SerializeField]
    private Transform doorTransform;
    [SerializeField]
    private Transform doorOpenPosition;
    private Vector3 doorClosedPosition;
    private BoxCollider2D doorCollider;
    
    private Vector3 startPosition;
    private Vector3 targetPosition;   
    private float timeStarted;

    [Header("Opening Attributes")]
    [SerializeField]
    private float openingSpeed = 2;
    [SerializeField]
    private AnimationCurve openingCurve;
    private bool isOpening;

    [Header("Key Components")]
    [SerializeField]
    private SpriteRenderer[] keys;
    [SerializeField]
    private GameObject keyHoleObjects;

    [Header("Target Attributes")]
    [SerializeField]
    private bool targetDoorIsExternal = true;
    [SerializeField]
    private int targetLevelID;
    [SerializeField]
    private int targetDoorID;
    private LevelManager levelManager;

    private bool rightNumberOfKeys;

    private DoorState doorState = DoorState.Static;
    private enum DoorState
    {
        Moving,
        Static
    }

    private void Start()
    {
        doorCollider = this.GetComponent<BoxCollider2D>();

        if(numberOfKeysNeeded > 0)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].enabled = false;
            }

            doorClosedPosition = doorTransform.position;

            doorCollider.enabled = false;
            this.enabled = false;
        }
        else
        {
            doorCollider.enabled = true;
            keyHoleObjects.SetActive(false);
            doorTransform.localPosition = doorOpenPosition.localPosition;
        }
    }

    public void AddKey()
    {
        if (numberOfCurrentKeys == numberOfKeysNeeded)
            return;

        keys[numberOfCurrentKeys].enabled = true;

        numberOfCurrentKeys++;

        if(numberOfCurrentKeys == numberOfKeysNeeded)
        {
            rightNumberOfKeys = true;
            InitiateDoorMoving(true);
        }
    }

    public void RemoveKey()
    {
        numberOfCurrentKeys--;
        keys[numberOfCurrentKeys].enabled = false;

        if (rightNumberOfKeys)
        {
            rightNumberOfKeys = false;
            InitiateDoorMoving(rightNumberOfKeys);
        }
    }

    private void InitiateDoorMoving(bool _isOpening)
    {
        startPosition = doorTransform.position;

        isOpening = _isOpening;

        if (_isOpening)
        {            
            targetPosition = doorOpenPosition.position;
        }
        else
        {
            doorCollider.enabled = false;
            targetPosition = doorClosedPosition;
        }
        
        timeStarted = Time.time;
        this.enabled = true;
        doorState = DoorState.Moving;

    }

    private void Update()
    {
        if(doorState == DoorState.Moving)
            MoveDoor();
    }

    private void MoveDoor()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / openingSpeed;

        Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, openingCurve.Evaluate(percentageComplete));
        doorTransform.position = newPos;

        if(percentageComplete >= 1.0f)
        {
            if(isOpening)
            {
                doorCollider.enabled = true;
            }
            this.enabled = false;
            doorState = DoorState.Static;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Character")
        {
            collision.gameObject.SetActive(false);

            //If target door is outside of this level
            if (targetDoorIsExternal)
            {
                if (levelManager == null)
                    levelManager = this.transform.root.GetComponent<LevelManager>();

                //levelManager.StartMovementToNextLevel(targetLevelID, targetDoorID - 1);
            }
            else
            {

            }
        }
    }
}
