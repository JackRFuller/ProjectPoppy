using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInputController : PlayerBehaviourHandler
{
    private PlayerBellInventoryController bellInventory;

    private Interactable currentInteractable;
    private bool isInteracting;
    public bool IsInteracting { get { return isInteracting;} }

    private bool m_isFiringLightGun;
    public bool IsFiringLightGun {get {return m_isFiringLightGun;}}

    private Vector2 m_directionalInput;

    protected override void Start()
    {
        base.Start();

        bellInventory = GetComponent<PlayerBellInventoryController>();        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MoveInput();

        InteractInput();
     
        //BellInput();

        //LightGunInput();

        HammerInput();
    }

    private void MoveInput()
    {
        m_directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(playerController.MovementController)
            playerController.MovementController.SetDirectionInput(m_directionalInput);
    }  
    
    private void InteractInput()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(m_isFiringLightGun)
                return;

            isInteracting = true;

            if (currentInteractable != null)
            {
                currentInteractable.OnPlayerInteract();
            }
        }
        else
        {
            isInteracting = false;
        }
    }

    private void HammerInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(m_directionalInput == Vector2.zero)
            {

            }
            else
            {
                if(m_directionalInput.x != 0 && m_directionalInput.y != 0)
                    return;

                float orientation = playerController.MovementController.GetObjectOrientation();

                if(orientation == 0 && m_directionalInput.y == -1)
                {
                    return;
                }
                else if(orientation == 180 && m_directionalInput.y == 1)
                {
                    return;
                }
                else if(orientation == 90 && m_directionalInput.x == 1)
                {
                    return;
                }
                else if(orientation == 270 && m_directionalInput.x == -1)
                {
                    return;
                }

                Vector2 throwDirection = m_directionalInput;                
                playerController.HammerActionHandler.ThrowHammer(throwDirection);
            }
        }
    }

    private void BellInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            bellInventory.RingBell(0);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            bellInventory.StopRingingBell();
        }
    }

    private void LightGunInput()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(isInteracting)
                return;

            playerController.LightGunHandler.FireLightGun();
            m_isFiringLightGun = true;
            playerController.AnimationController.SetPlayerCastingLightBeam(m_isFiringLightGun);
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            m_isFiringLightGun = false;
            playerController.LightGunHandler.TurnOffLightGun();
            playerController.AnimationController.SetPlayerCastingLightBeam(m_isFiringLightGun);
        }
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
        Debug.Log("Set");
    }

    public void LightBeamInteruppted()
    {
        m_isFiringLightGun = false;
        playerController.LightGunHandler.TurnOffLightGun();        
    }
}
