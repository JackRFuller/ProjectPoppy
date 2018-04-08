using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovementController : MonoBehaviour
{
    private Controller2D controller;

    private float gravity = -6f;
    private Vector3 velocity;

    private Vector2 directionInput;

    //X Direction smoothing
    private float velocityXSmoothing;
    private float accelerationTimeGrounded = .1f;

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

    public void SetDirectionInput(Vector2 input)
    {
        directionInput = input;
    }

    private void Update()
    {
        if(movementState == MovementState.Free)
        {
            if(controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            float xInput = directionInput.x;
            float yInput = directionInput.y;

            float targetVelocityX = 0;

            if (controller.GetObjectOrientation() == 0)
            {
                targetVelocityX = xInput * 6f; 
            }
            else if (controller.GetObjectOrientation() == 180)
            {
                targetVelocityX = -xInput * 6f;
            }
            else if (controller.GetObjectOrientation() == 90)
            {
                targetVelocityX = yInput * 6f;
            }
            else if (controller.GetObjectOrientation() == 270)
            {
                targetVelocityX = -yInput * 6f;
            }

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,accelerationTimeGrounded);
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
