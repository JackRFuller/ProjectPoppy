using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerMovementController playerMovement;
    private PlayerBellInventoryController bellInventory;

    private Interactable currentInteractable;

    private void Start()
    {
        bellInventory = GetComponent<PlayerBellInventoryController>();
        playerMovement = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        MoveInput();

        InteractInput();

        //PushInput();

        BellInput();
    }

    private void MoveInput()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerMovement.SetDirectionInput(directionalInput);
    }  
    
    private void InteractInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.OnPlayerInteract();
        }
    }

    private void PushInput()
    {
        if(Input.GetKey(KeyCode.E))
        {
            playerMovement.SetPushing(true);
        }
        else
        {
            playerMovement.SetPushing(false);
        }
    }

    private void BellInput()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Debug.Log(("Ringing Life Bell"));
            bellInventory.RingBell(0);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Debug.Log(("Stopped Life Bell"));
            bellInventory.StopRingingBell();
        }

    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }
}
