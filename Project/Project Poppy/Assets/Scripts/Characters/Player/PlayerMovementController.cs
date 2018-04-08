using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovementController : MonoBehaviour
{
    private Controller2D controller;

    private float gravity = -6f;
    private Vector3 velocity;

    private DynamicPlatform currentPlatform;
    private Transform originalParent;

    private MovementState movementState;
    private enum MovementState
    {
        Free,
        Frozen,
    }

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        originalParent = this.transform.root;
        movementState = MovementState.Free;
    }

    private void Update()
    {
        if(movementState == MovementState.Free)
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            if (controller.GetObjectOrientation() == 0)
            {
                velocity.x = xInput * 6f; 
            }
            else if (controller.GetObjectOrientation() == 180)
            {
                velocity.x = -xInput * 6f;
            }
            else if (controller.GetObjectOrientation() == 90)
            {
                velocity.x = yInput * 6f;
            }
            else if (controller.GetObjectOrientation() == 270)
            {
                velocity.x = -yInput * 6f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void FreezePlayerMovement()
    {
        movementState = MovementState.Frozen;
        velocity = Vector3.zero;
        controller.Move(velocity * Time.deltaTime);

    }

    private void UnFreezePlayerMovement()
    {
        controller.CalculateRaySpacing();
        movementState = MovementState.Free;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DynamicPlatform")
        {
            this.transform.SetParent(collision.transform);
            currentPlatform = collision.GetComponent<DynamicPlatform>();

            currentPlatform.platformBehaviourTriggered += FreezePlayerMovement;
            currentPlatform.platformBehaviourEnded += UnFreezePlayerMovement;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DynamicPlatform")
        {
            this.transform.SetParent(originalParent);            

            currentPlatform.platformBehaviourTriggered -= FreezePlayerMovement;
            currentPlatform.platformBehaviourEnded -= UnFreezePlayerMovement;

            currentPlatform = null;
        }
    }
}
