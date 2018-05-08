using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicPlatform : PlatformHandler
{
    public UnityAction platformBehaviourTriggered;
    public UnityAction platformBehaviourEnded;

    //Enabling Attributes
    [SerializeField]
    private bool canEnable;
    [SerializeField]
    private EnabledState enabledState;
    [SerializeField]
    private Material disabledMaterial;
    private enum EnabledState
    {
        Enabled,
        Disabled,
    }

    //Rotating Attributes
    [SerializeField]
    private bool canRotate;


    //Moving Attributes
    [SerializeField]
    private bool canMove;
    private Vector2 startingMovingPosition;
    private Vector2 targetMovingPosition;

    protected override void Start()
    {
        this.gameObject.tag = "DynamicPlatform";

        base.Start();

        if(canEnable)
        {
            DetermineStartingState();
        }
    }

    private void DetermineStartingState()
    {
       switch(enabledState)
        {
            case EnabledState.Disabled:
                DisablePlatform();
                break;

            case EnabledState.Enabled:
                EnablePlatform();
                break;
        }
    }

    private void EnablePlatform()
    {
        platformRenderer.material = platformStartingMaterial;
        platformCollider.enabled = true;
        enabledState = EnabledState.Enabled;
    }

    private void DisablePlatform()
    {
        platformRenderer.material = disabledMaterial;
        platformCollider.isTrigger = true;
        enabledState = EnabledState.Disabled;
    }

    private void HitByLifeBell()
    {
        if (canEnable)
        {
            switch (enabledState)
            {
                case EnabledState.Disabled:
                    EnablePlatform();
                    break;

                case EnabledState.Enabled:
                    DisablePlatform();
                    break;
            }
        }       
    }   
}
