using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputController inputController;
    public PlayerInputController InputController { get { return inputController;} }
    private PlayerMovementController movementController;
    public PlayerMovementController MovementController
    {
        get
        {
            return movementController;
        }
    }
    private PlayerAnimationController animationController;
    public PlayerAnimationController AnimationController
    {
        get { return animationController; }
    }

    private Interactable interactable;
    public Interactable Interactable { get { return interactable;} }

    private void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponent<PlayerAnimationController>();
        inputController = GetComponent<PlayerInputController>();
    }

    public void SetInteractable(Interactable _interactable)
    {
        interactable = _interactable;
    }
}
