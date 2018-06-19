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
     
        BellInput();

        LightGunInput();
    }

    private void MoveInput()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(playerController.MovementController)
            playerController.MovementController.SetDirectionInput(directionalInput);
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
