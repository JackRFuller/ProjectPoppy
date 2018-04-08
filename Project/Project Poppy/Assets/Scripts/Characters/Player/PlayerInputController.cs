using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerMovementController playerMovement;

    private void Start()
    {
        playerMovement = this.GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerMovement.SetDirectionInput(directionalInput);
    }

}
