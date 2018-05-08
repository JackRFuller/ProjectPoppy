using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PuzzleController2D))]
public class PuzzleObjectMovementController : MonoBehaviour
{  
    private PuzzleController2D controller;
    private DynamicPlatform currentPlatform;
    private BoxCollider2D objCollider;
    public BoxCollider2D GetBoxCollider2D { get { return objCollider; } }

    private Transform originalParent;
    
    private float gravity = -6f;
    private Vector3 velocity;

    private Vector2 lastGroundedPoint;
    private Camera mainCamera;

    private bool isFalling;

    //Screen Wrapping
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private int wrappingCount = 0;

    private void Start()
    {
        controller = GetComponent<PuzzleController2D>();
        objCollider = this.GetComponent<BoxCollider2D>();
        mainCamera = Camera.main;

        originalParent = this.transform.parent.root;
       
    }
   

    private void Update()
    {        
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (!controller.collisions.below)
            PlayerDisconnectFromWeight();

        //Check if fully grounded
        if (controller.collisions.fullyGrounded)
        {
            lastGroundedPoint = this.transform.position;
            wrappingCount = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        ScreenWrap();
    }   
    
    public void PlayerPushingWeight()
    {
        if (!controller.collisions.above || !controller.collisions.below)
        {
            this.gameObject.layer = 9;
        }
        else
        {
            PlayerDisconnectFromWeight();
        }
            
    }

    public void PlayerDisconnectFromWeight()
    {
        this.gameObject.layer = 10;
        this.transform.SetParent(originalParent);
    }

    private void ScreenWrap()
    {
        if (isVisible())
        {
            isWrappingX = false;
            isWrappingY = false;

            return;
        }

        if (isWrappingY && isWrappingX)
            return;


        Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(this.transform.position);
        Vector3 newPosition = this.transform.position;       

        if (!isWrappingX && viewPortPosition.x > 1 || viewPortPosition.x < 0)
        {
            newPosition.x = -newPosition.x + 1.5f;
            isWrappingX = true;
            wrappingCount++;
        }

        if (!isWrappingY && viewPortPosition.y > 1 || viewPortPosition.y < 0)
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
            wrappingCount++;
        }

        if (wrappingCount > 1)
        {
            newPosition = lastGroundedPoint;
        }


        this.transform.position = newPosition;
    }

    private bool isVisible()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(this.transform.position);
        bool _isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return _isVisible;
    }    

    private void FreezePlayerMovement()
    {       
        velocity = Vector3.zero;
        controller.Move(velocity * Time.deltaTime);

    }

    private void UnFreezePlayerMovement()
    {
        controller.CalculateRaySpacing();       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DynamicPlatform")
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

