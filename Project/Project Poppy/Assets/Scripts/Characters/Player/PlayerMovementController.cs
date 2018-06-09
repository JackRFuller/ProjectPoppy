using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : Controller2D
{
    private PlayerController playerController;
    private float playerMovementSpeed = 1.5f;
    private float gravity = -6f;
    private Vector3 velocity;

    private Vector2 directionInput;

    //X Direction smoothing
    private float velocityXSmoothing;
    private float accelerationTimeGrounded = .05f;

    private PlatformController currentPlatform;
    private Transform originalParent;

    private MovementState movementState;
    private enum MovementState
    {
        Free,
        Frozen,
    }

    private Vector2 lastGroundedPoint;
    private Camera mainCamera;

    private bool isFalling;

    private Transform lastPushedObject;
    private bool hasPushingInput;

    private Transform lastInteractedObject;
    private float lastXInput;
    private bool isSuccessfullyInteracting;
    private bool isMovingObject;
    private bool hasUpdatedColliderSizeAndPosition;
    private float moveableObjectSide; // Deteremines what side of the player the object is on

    protected override void Start()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();
        mainCamera = Camera.main;

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
            if(collisions.above || collisions.below)
            {
                velocity.y = 0;
            }

            float xInput = directionInput.x;
            float yInput = directionInput.y;

            float targetVelocityX = 0;

            if(collisions.below)
            {
                if (GetObjectOrientation() == 0)
                {
                    targetVelocityX = xInput;
                }
                else if (GetObjectOrientation() == 180)
                {
                    targetVelocityX = -xInput;
                }
                else if (GetObjectOrientation() == 90)
                {
                    targetVelocityX = yInput;
                }
                else if (GetObjectOrientation() == 270)
                {
                    targetVelocityX = -yInput;
                }
            }

            //Used for animation control
            float playerInput = Mathf.Abs(targetVelocityX);

            if(targetVelocityX != 0)
                lastXInput = targetVelocityX;

            targetVelocityX *= playerMovementSpeed;            

            bool isPushing = false;

            if (GetIfPlayerIsPushingObject(targetVelocityX))
            {
                if (Mathf.Abs(targetVelocityX) > 0)
                {
                    if(!hasUpdatedColliderSizeAndPosition)
                    {
                       float newSize = playerController.Interactable.MovementController.ObjCollider.size.x;
                       UpdateColliderSizeAndPosition(newSize,moveableObjectSide);  
                       hasUpdatedColliderSizeAndPosition = true;
                    }
                     
                    isPushing = GetPositionInRelationToPushableObject(targetVelocityX);
                    isMovingObject = true;
                    targetVelocityX *= 0.5f;
                    playerController.Interactable.MovementController.SetDirectionInput(targetVelocityX);
                }
                
            }
            else
            {
                if(hasUpdatedColliderSizeAndPosition)
                {
                    ResetColliderSizeAndPosition();
                    hasUpdatedColliderSizeAndPosition = false;
                }
                isMovingObject = false;
                DetectedInteractable();
            }

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,accelerationTimeGrounded);  

            
            playerController.AnimationController.SetPlayerMovementSpeed(playerInput,isMovingObject,isPushing);

            if(!isMovingObject)
                playerController.AnimationController.SetPlayerMeshRotationBasedOnVelocity(velocity);

        }

        if(collisions.below)
        {
            if (isFalling)
            {
                isFalling = false;
                playerController.AnimationController.SetPlayerFalling(isFalling);
                StartCoroutine(LandingCooldown());
            }
        }
        else
        {
            if (!isFalling)
            {
                StartCoroutine("FallingCooldown");
            }
        }

        //Check if fully grounded
        if (collisions.fullyGrounded)
        {
            lastGroundedPoint = this.transform.position;
        }        

        velocity.y += gravity * Time.deltaTime;
        Move(velocity * Time.deltaTime);
        screenwrappingBehaviour.ScreenWrap();
    }

    private bool GetIfPlayerIsPushingObject(float targetVelocity)
    {
        if (playerController.Interactable)
        {
            if (playerController.Interactable.MovementController != null)
            {
                if (playerController.Interactable.MovementController.HasDetectedPlayer())
                {
                    if (playerController.InputController.IsInteracting)
                    {
                       return true;
                    }
                }
            }
        }

        return false;
    }

    private bool GetPositionInRelationToPushableObject(float targetVelocityX)
    {
        if (GetObjectOrientation() == 0 || GetObjectOrientation() == 180)
        {
            if (transform.position.x < playerController.Interactable.transform.position.x)
            {
                if (targetVelocityX > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (targetVelocityX > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        else if (GetObjectOrientation() == 90 || GetObjectOrientation() == 270)
        {
            if (transform.position.y < playerController.transform.position.y)
            {
                if (targetVelocityX > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (targetVelocityX > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        Debug.LogError("No Reasonable Orientation Found");
        return false;
    }

    private void DetectedInteractable()
    {
        CollisionOrientationInfo orientationInfo = new CollisionOrientationInfo();

        orientationInfo.orientation = GetObjectOrientation();

        orientationInfo.sideDirection = Mathf.Sign(velocity.x);
        orientationInfo.rayLength = 2 + skinWidth;

        orientationInfo.SetInfoBasedOnOrientation();
       
        //If we're moving left or if we're moving right
        Vector2 rayOrigin = (lastXInput == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
        Vector2 rayOffset = (lastXInput == -1) ? new Vector2(-0.1f, -0.1f) : new Vector2(0.1f, 0.1f);
        rayOrigin += (orientationInfo.rayOriginDirection + rayOffset);
        Ray2D collisionRay = new Ray2D(rayOrigin, orientationInfo.collisionRayDirection * orientationInfo.sideDirection);

        RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, orientationInfo.rayLength);
        Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);

        if (hit.transform != null)
        {
            if (lastInteractedObject == null || lastInteractedObject != hit.transform)
            {
                lastInteractedObject = hit.transform;
                playerController.SetInteractable(hit.transform.GetComponent<Interactable>());
                moveableObjectSide = lastXInput;
            }           
        }
        else
        {
            playerController.SetInteractable(null);
        }    
    }

    public void SetPushing(bool hasInput)
    {
        hasPushingInput = hasInput;
    }   

    private IEnumerator FallingCooldown()
    {
        isFalling = true;
        playerController.AnimationController.SetPlayerFalling(isFalling);
        yield return new WaitForSeconds(1f);
        movementState = MovementState.Frozen;
    }

    private IEnumerator LandingCooldown()
    {
        yield return new WaitForSeconds(1.3f);
        movementState = MovementState.Free;
    }

    private void FreezePlayerMovement()
    {
        movementState = MovementState.Frozen;
        velocity = Vector3.zero;
        Move(velocity * Time.deltaTime);

    }

    private void UnFreezePlayerMovement()
    {
        CalculateRaySpacing();
        movementState = MovementState.Free;
    }

    public void SetPlayerPosition(Transform newTransform)
    {
        this.transform.position = newTransform.position;
        this.transform.rotation = newTransform.rotation;

        Transform mesh = newTransform.GetChild(0);
        playerController.AnimationController.SetPlayerMeshRotation(mesh);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DynamicObject" && collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            this.transform.SetParent(collision.transform);

            currentPlatform = collision.GetComponent<PlatformController>();

            if(!currentPlatform)
                return;

            if (currentPlatform.CanRotate || currentPlatform.CanMove)
            {

            }

            //currentPlatform = collision.GetComponent<DynamicPlatform>();

            //if (!currentPlatform)
            //    return;

            //currentPlatform.platformBehaviourTriggered += FreezePlayerMovement;
            //currentPlatform.platformBehaviourEnded += UnFreezePlayerMovement;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DynamicObject" && collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            this.transform.SetParent(originalParent);

            //if (!currentPlatform)
            //    return;

            //currentPlatform.platformBehaviourTriggered -= FreezePlayerMovement;
            //currentPlatform.platformBehaviourEnded -= UnFreezePlayerMovement;

            //currentPlatform = null;
        }
    }
}
