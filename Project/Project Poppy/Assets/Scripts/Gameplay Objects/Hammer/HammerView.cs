using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerView : Entity
{
    private HammerMovementHandler m_movementHandler;

    private ScreenwrappingBehaviour screenwrappingBehaviour;
    

    public HammerMovementHandler MovementHandler {get{return m_movementHandler;}}
    public ScreenwrappingBehaviour ScreenwrappingBehaviour {get {return screenwrappingBehaviour;}}    

    private void Start()
    {
        m_movementHandler = GetComponent<HammerMovementHandler>();
        screenwrappingBehaviour = GetComponent<ScreenwrappingBehaviour>();
    }

    public void InitiateHammer(Vector2 direction)
    {
        
        if(m_movementHandler == null)
             m_movementHandler = GetComponent<HammerMovementHandler>();

        ShowObject();
        m_movementHandler.InitiateMovement(direction);
    }
	
}
