using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovementController : MonoBehaviour
{
    private ScreenwrappingBehaviour screenWrapController;
    private PlayerAnimationController playerAnimController;
    private Controller2D controller;

    private float playerMovementSpeed = 1.5f;
    private float gravity = -6f;
    private Vector3 velocity;

    private Vector2 directionInput;

    //X Direction smoothing
    private float velocityXSmoothing;
    private float accelerationTimeGrounded = .05f;

    private DynamicPlatform currentPlatform;
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

    //Screen Wrapping
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private int wrappingCount = 0;

    private Transform lastPushedObject;
    private bool hasPushingInput;
    private PuzzleObjectMovementController puzzleObjController;


    private void Start()
    {
        controller = GetComponent<Controller2D>();
        screenWrapController = this.GetComponent<ScreenwrappingBehaviour>();
        playerAnimController = this.GetComponent<PlayerAnimationController>();
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
            if(controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            float xInput = directionInput.x;
            float yInput = directionInput.y;

            float targetVelocityX = 0;

            if(controller.collisions.below)
            {
                if (controller.GetObjectOrientation() == 0)
                {
                    targetVelocityX = xInput * playerMovementSpeed;
                }
                else if (controller.GetObjectOrientation() == 180)
                {
                    targetVelocityX = -xInput * playerMovementSpeed;
                }
                else if (controller.GetObjectOrientation() == 90)
                {
                    targetVelocityX = yInput * playerMovementSpeed;
                }
                else if (controller.GetObjectOrientation() == 270)
                {
                    targetVelocityX = -yInput * playerMovementSpeed;
                }
            }

            bool isPushing = false;

            if (Mathf.Abs(targetVelocityX) > 0)
            {              
                if (hasPushingInput)
                {
                    Debug.Log("Pushing Input");
                    if (controller.GetHitTransform != null)
                    {
                        if (controller.GetHitTransform.tag == "Weight")
                        {
                            Debug.Log("Found Crate");
                            if (puzzleObjController == null)
                            {
                                puzzleObjController = controller.GetHitTransform.GetComponent<PuzzleObjectMovementController>();
                                controller.UpdateCollider(true, puzzleObjController.GetBoxCollider2D.size.x);
                                lastPushedObject = controller.GetHitTransform;                               
                            }

                            if(lastPushedObject)
                                lastPushedObject.transform.parent = this.transform;

                            puzzleObjController.PlayerPushingWeight();
                            isPushing = true;
                            targetVelocityX *= 0.5f;
                        }
                    }
                }

                else
                {
                    if(puzzleObjController != null)
                    {                        
                        lastPushedObject.transform.parent = null;
                        puzzleObjController.PlayerDisconnectFromWeight();
                        puzzleObjController = null;                        
                    }
                    
                }

            }

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,accelerationTimeGrounded);  
           
            playerAnimController.SetPlayerMeshRotationBasedOnVelocity(velocity);

            playerAnimController.SetPlayerMovementSpeed(velocity.x,isPushing);

        }

        if(controller.collisions.below)
        {
            if (isFalling)
            {
                isFalling = false;
                playerAnimController.SetPlayerFalling(isFalling);
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
        if (controller.collisions.fullyGrounded)
        {
            lastGroundedPoint = this.transform.position;
            wrappingCount = 0;
        }        

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        screenWrapController.ScreenWrap();
    }

    public void SetPushing(bool hasInput)
    {
        hasPushingInput = hasInput;
    }   

    private IEnumerator FallingCooldown()
    {
        isFalling = true;
        playerAnimController.SetPlayerFalling(isFalling);
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
        controller.Move(velocity * Time.deltaTime);

    }

    private void UnFreezePlayerMovement()
    {
        controller.CalculateRaySpacing();
        movementState = MovementState.Free;
    }

    public void SetPlayerPosition(Transform newTransform)
    {
        this.transform.position = newTransform.position;
        this.transform.rotation = newTransform.rotation;

        Transform mesh = newTransform.GetChild(0);
        playerAnimController.SetPlayerMeshRotation(mesh);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DynamicPlatform")
        {
            this.transform.SetParent(collision.transform);
            currentPlatform = collision.GetComponent<DynamicPlatform>();

            if (!currentPlatform)
                return;

            currentPlatform.platformBehaviourTriggered += FreezePlayerMovement;
            currentPlatform.platformBehaviourEnded += UnFreezePlayerMovement;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DynamicPlatform")
        {
            this.transform.SetParent(originalParent);

            if (!currentPlatform)
                return;

            currentPlatform.platformBehaviourTriggered -= FreezePlayerMovement;
            currentPlatform.platformBehaviourEnded -= UnFreezePlayerMovement;

            currentPlatform = null;
        }
    }
}
