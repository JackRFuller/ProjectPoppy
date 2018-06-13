using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerMovementController playerMovement;
    private PlayerBellInventoryController bellInventory;

    private Interactable currentInteractable;
    private bool isInteracting;
    public bool IsInteracting { get { return isInteracting;} }

    private void Start()
    {
        bellInventory = GetComponent<PlayerBellInventoryController>();
        playerMovement = GetComponent<PlayerMovementController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if(Input.GetKey(KeyCode.Space))
        {
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

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
        Debug.Log("Set");
    }
}
