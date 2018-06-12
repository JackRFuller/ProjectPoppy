using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : Controller
{
    [HideInInspector][SerializeField]private bool canActivate;
    public bool CanActivate { get { return canActivate; } }
    [HideInInspector] [SerializeField] private bool canMove;
    public bool CanMove { get { return canMove;} }
    [HideInInspector] [SerializeField] private bool canRotate;
    public bool CanRotate { get { return canRotate;} }

    //Behaviour
    private ActivatingPlatformBehaviour activatingPlatformBehaviour;
    private RotatingPlatformBehaviour rotatingPlatformBehaviour;
    private MovingPlatformBehaviour movingPlatformBehaviour;

    private int m_numberOfActionsInitiated = 0;

    public UnityAction platformBehaviourTriggered;
    public UnityAction platformBehaviourEnded;

    private void Start()
    {
        if (canActivate)
            activatingPlatformBehaviour = GetComponent<ActivatingPlatformBehaviour>();

        if (canMove)
            movingPlatformBehaviour = GetComponent<MovingPlatformBehaviour>();

        if (canRotate)
            rotatingPlatformBehaviour = GetComponent<RotatingPlatformBehaviour>();
    }
    protected override void HitByLifeBell()
    {
        Debug.Log(gameObject.name + " Hit By Life Bell");

        if (canActivate)
            activatingPlatformBehaviour.ActivateBehaviour();
       
        if(canRotate || canMove)
        {         
            if(platformBehaviourTriggered != null)
                platformBehaviourTriggered();

            if(canRotate)
            {
                m_numberOfActionsInitiated++;          
                rotatingPlatformBehaviour.ActivateBehaviour();
            }
             if(canMove)
            {
                m_numberOfActionsInitiated++;    
                movingPlatformBehaviour.ActivateBehaviour();
            }
        } 
    }  

    public void BehaviourEnded()
    {
        if(m_numberOfActionsInitiated > 0)
            m_numberOfActionsInitiated--;

        if(platformBehaviourEnded != null)
            platformBehaviourEnded();
    }

    #region EditorBehaviours
    public void AddOrRemoveBehaviours(int behaviourIndex, bool isAdding)
    {
        switch (behaviourIndex)
        {
            case 0:
                if (isAdding)
                {
                    activatingPlatformBehaviour = gameObject.AddComponent<ActivatingPlatformBehaviour>();
                    canActivate = true;
                }
                else
                {
                    DestroyImmediate(activatingPlatformBehaviour);
                    canActivate = false;
                }
                break;
            case 1:
                if (isAdding)
                {
                    rotatingPlatformBehaviour = gameObject.AddComponent<RotatingPlatformBehaviour>();
                    canRotate = true;
                }
                else
                {
                    DestroyImmediate(rotatingPlatformBehaviour);
                    canRotate = false;
                }
                break;
            case 2:
                if (isAdding)
                {
                    movingPlatformBehaviour = gameObject.AddComponent<MovingPlatformBehaviour>();
                    canMove = true;
                }
                else
                {
                    DestroyImmediate(movingPlatformBehaviour);
                    canMove = false;
                }
                break;
        }
    }
    #endregion

}


