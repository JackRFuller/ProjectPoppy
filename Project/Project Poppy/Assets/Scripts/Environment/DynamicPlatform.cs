using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DynamicPlatform : MonoBehaviour
{
    public UnityAction platformBehaviourTriggered;
    public UnityAction platformBehaviourEnded;

    //Moving Attributes
    [SerializeField]
    private bool canMove;
    private Vector2 startingMovingPosition;
    private Vector2 targetMovingPosition;
    

}
