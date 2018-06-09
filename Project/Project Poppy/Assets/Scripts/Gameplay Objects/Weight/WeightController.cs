using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightController : Interactable
{
    protected override void Start()
    {
        base.Start();

        movementController = GetComponent<PushableObject>();
    }

    private void Update()
    {
        DetectPlayer();
    }
   
    public void DetectPlayer()
    {
        if (movementController.HasDetectedPlayer())
        {
            ShowInteractPrompt();
        }
        else
        {
            HideInteractPrompt();
        }
    }

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();

        Debug.Log("Interacting");
    }
}
