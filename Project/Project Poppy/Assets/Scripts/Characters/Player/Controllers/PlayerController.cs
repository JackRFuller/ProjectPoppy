using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputController inputController;
    private PlayerAnimationController animationController;
    private PlayerLightGunHandler m_lightGunHandler;
    private PlayerHammerThrowHandler m_hammerActionHandler;
    private Interactable interactable;

    
    private PlayerMovementController movementController;
    public PlayerMovementController MovementController{get{return movementController;}}
    public PlayerAnimationController AnimationController {get { return animationController; }}    
    public PlayerLightGunHandler LightGunHandler { get {return m_lightGunHandler;}}
    public PlayerInputController InputController { get { return inputController;} }
    public PlayerHammerThrowHandler HammerActionHandler {get {return m_hammerActionHandler;}}
    public Interactable Interactable { get { return interactable;} }

    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponent<PlayerAnimationController>();
        inputController = GetComponent<PlayerInputController>();
        m_lightGunHandler = GetComponent<PlayerLightGunHandler>();
        m_hammerActionHandler = GetComponent<PlayerHammerThrowHandler>();
    }

    public void SetInteractable(Interactable _interactable)
    {
        interactable = _interactable;
    }
}
