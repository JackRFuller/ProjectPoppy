using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{ 
    public CollisionInfo collisions;
    private Transform hitTransform;
    public Transform GetHitTransform { get { return hitTransform; } }

    private float colliderXOriginal;

    protected override void Start()
    {
        base.Start();

        colliderXOriginal = objCollider.size.x;
    }

    public void UpdateCollider(bool increasingSize,float XToAdd)
    {
        if(increasingSize)
        {
            objCollider.size = new Vector2(objCollider.size.x + XToAdd, 1);
        }
        else
        {
            objCollider.size =  new Vector2(colliderXOriginal,1);
        }

        CalculateRaySpacing();
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

            if (hit)
            {
                collisions.below = gravityDirection == -1;
                collisions.above = gravityDirection == 1;

                groundedCount++;
                velocity.y = (hit.distance - skinWidth) * gravityDirection;

                if(i == verticalRayCount)
                {
                    rayLength = hit.distance;
                }
            } 

        }
        
        if (groundedCount == verticalRayCount)
        {
            collisions.fullyGrounded = true;
        }
        else
        {
            collisions.fullyGrounded = false;
        }
        
    }

    /// <summary>
    /// Based off of Horizontal Collisions from tutorial
    /// </summary>
    private void SideOfObjectCollisions(ref Vector3 velocity)
    {
        float orientation = GetObjectOrientation();
        float sideDirection = 0;
        float rayLength = 0;

        sideDirection = Mathf.Sign(velocity.x);
        rayLength = Mathf.Abs(velocity.x) + skinWidth;

        Vector2 rayOriginDirection = Vector2.zero;
        Vector2 collisionRayDirection = Vector2.zero;

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

        int collisionCount = 0;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            //If we're moving left or if we're moving right
            Vector2 rayOrigin = (sideDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += rayOriginDirection * (horizontalRaySpacing * i);
            Ray2D collisionRay = new Ray2D(rayOrigin, collisionRayDirection * sideDirection);

            RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);
            Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);

            if (hit)
            {
                collisionCount++;

                hitTransform = hit.transform;

                if (collisionCount == horizontalRayCount)
                {
                    
                }    
                
                
                velocity.x = (hit.distance - skinWidth) * sideDirection;
                rayLength = hit.distance;

                collisions.left = sideDirection == -1;
                collisions.right = sideDirection == 1;
                
               
            }             
        }

        if(collisionCount < horizontalRayCount)
        {
            //hitTransform = null;
        }
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
}
