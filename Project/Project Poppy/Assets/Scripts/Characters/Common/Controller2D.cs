using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScreenwrappingBehaviour))]
public class Controller2D : RaycastController
{
    protected ScreenwrappingBehaviour screenwrappingBehaviour;
    public CollisionInfo collisions;
    private Transform hitTransform;
    public Transform GetHitTransform { get { return hitTransform; } }

    private float colliderXOriginal;

    protected override void Start()
    {
        base.Start();

        screenwrappingBehaviour = GetComponent<ScreenwrappingBehaviour>();
        colliderXOriginal = objCollider.size.x;
    }

    protected void UpdateColliderSizeAndPosition(float sizeToAdd, float movementDirection)
    {
        Vector2 newOffset = Vector2.zero;
        Vector2 newSize = new Vector2(objCollider.size.x + sizeToAdd,objCollider.size.y);
        
        newOffset.x = newSize.x - (sizeToAdd - 0.1f);

        if(movementDirection == -1)
        {
            newOffset = -newOffset;
        }
        else
        {
            //newOffset.x = newSize.x + sizeToAdd;
        }

        objCollider.size = newSize;
        objCollider.offset = newOffset;      
    }

    protected void ResetColliderSizeAndPosition()
    {
        objCollider.size = new Vector2(colliderXOriginal,objCollider.size.y);
        objCollider.offset = Vector2.zero;
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();

        collisions.Reset();

        if (velocity.x != 0)
            SideOfObjectCollisions(ref velocity);

        if (velocity.y != 0)
            TopAndBottomOfObjectCollisions(ref velocity);

        transform.Translate(velocity);
    }

    /// <summary>
    /// Based off of Vertical Collisions from tutorial
    /// </summary>
    private void TopAndBottomOfObjectCollisions(ref Vector3 velocity)
    {
        float orientation = GetObjectOrientation();
        float gravityDirection = 0;
        float rayLength = 0;

        gravityDirection = Mathf.Sign(velocity.y);
        rayLength = Mathf.Abs(velocity.y) + skinWidth;

        Vector2 rayOriginDirection = Vector2.zero;
        Vector2 collisionRayDirection = Vector2.zero;

        if(orientation == 0)
        {
            rayOriginDirection = Vector2.right;
            collisionRayDirection = Vector2.up;
        }
        else if(orientation == 90)
        {
            rayOriginDirection = Vector2.up;
            collisionRayDirection = Vector2.left;
        }
        else if (orientation == 180)
        {
            rayOriginDirection = Vector2.left;
            collisionRayDirection = Vector2.down;
        }
        else if (orientation == 270)
        {
            rayOriginDirection = Vector2.down;
            collisionRayDirection = Vector2.right;
        }

        int groundedCount = 0;

        for (int i = 0; i < verticalRayCount; i++)
        {
            //If we're moving down or if we're moving up
            Vector2 rayOrigin = (gravityDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += rayOriginDirection * (verticalRaySpacing * i + velocity.x);
            Ray2D collisionRay = new Ray2D(rayOrigin, collisionRayDirection * gravityDirection);

            RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);                               
            Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);

            if (hit.transform != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    collisions.below = gravityDirection == -1;
                    collisions.above = gravityDirection == 1;

                    groundedCount++;
                    velocity.y = (hit.distance - skinWidth) * gravityDirection;

                    if (i == verticalRayCount)
                    {
                        rayLength = hit.distance;
                    }
                }
            }
        }
        collisions.fullyGrounded = groundedCount == verticalRayCount;
    }

    /// <summary>
    /// Based off of Horizontal Collisions from tutorial
    /// </summary>
    private void SideOfObjectCollisions(ref Vector3 velocity)
    {
        CollisionOrientationInfo orientationInfo = new CollisionOrientationInfo();

        orientationInfo.orientation = GetObjectOrientation();

        orientationInfo.sideDirection = Mathf.Sign(velocity.x);
        orientationInfo.rayLength = Mathf.Abs(velocity.x) + skinWidth;

        orientationInfo.SetInfoBasedOnOrientation();

        int collisionCount = 0;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            //If we're moving left or if we're moving right
            Vector2 rayOrigin = (orientationInfo.sideDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += orientationInfo.rayOriginDirection * (horizontalRaySpacing * i);
            Ray2D collisionRay = new Ray2D(rayOrigin, orientationInfo.collisionRayDirection * orientationInfo.sideDirection);

            RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, orientationInfo.rayLength, collisionMask);
            //Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);

            if (hit.transform != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    velocity.x = (hit.distance - skinWidth) * orientationInfo.sideDirection;
                    orientationInfo.rayLength = hit.distance;

                    collisions.left = orientationInfo.sideDirection == -1;
                    collisions.right = orientationInfo.sideDirection == 1;
                }
            }
        }
    }

    public void TeleportPlayer(Vector3 targetPosition, Vector3 targetRotation)
    {
        SetPosition(targetPosition);
        SetRotation(targetRotation);

        CalculateRaySpacing();
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool fullyGrounded;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    public struct CollisionOrientationInfo
    {
        public float orientation;
        public float sideDirection;
        public float rayLength;
        public Vector2 rayOriginDirection;
        public Vector2 collisionRayDirection;

        public void SetInfoBasedOnOrientation()
        {
            if (orientation == 0)
            {
                rayOriginDirection = Vector2.up;
                collisionRayDirection = Vector2.right;
            }
            else if (orientation == 90)
            {
                rayOriginDirection = Vector2.left;
                collisionRayDirection = Vector2.up;
            }
            else if (orientation == 180)
            {
                rayOriginDirection = Vector2.down;
                collisionRayDirection = Vector2.left;
            }
            else if (orientation == 270)
            {
                rayOriginDirection = Vector2.right;
                collisionRayDirection = Vector2.down;
            }
        }

    }
}
