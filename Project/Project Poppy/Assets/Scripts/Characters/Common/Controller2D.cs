using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;

    private BoxCollider2D objCollider;
    private RayCastOrigins raycastOrigins;

    private Bounds bounds;
    private const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;

    private void Start()
    {
        objCollider = this.GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();


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

        for (int i = 0; i < verticalRayCount; i++)
        {
            //If we're moving down or if we're moving up
            Vector2 rayOrigin = (gravityDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += rayOriginDirection * (verticalRaySpacing * i + velocity.x);
            Ray2D collisionRay = new Ray2D(rayOrigin, collisionRayDirection * gravityDirection);

            RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);                               
            Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);
                
            if(hit)
            {
                velocity.y = (hit.distance - skinWidth) * gravityDirection;
                rayLength = hit.distance;
            } 
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
                velocity.x = (hit.distance - skinWidth) * sideDirection;
                rayLength = hit.distance;
            } 
        }
    }

    private void UpdateRaycastOrigins()
    {
        bounds = objCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        SetRaycastOriginsBasedOnRotation();
    }

    private void SetRaycastOriginsBasedOnRotation()
    {
        float orientation = GetObjectOrientation();

        if(orientation == 0)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        else if(orientation == 180)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.max.x, bounds.max.y);
            raycastOrigins.bottomRight = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.min.y);
        }
        else if(orientation == 90)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.max.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.topRight = new Vector2(bounds.min.x, bounds.max.y);
        }
        else if(orientation == 270)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.bottomRight = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.max.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.min.y);
        }

    }

    public void CalculateRaySpacing()
    {
        bounds = objCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        float orientation = GetObjectOrientation();

        if(orientation == 0 || orientation == 180)
        {
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
        if(orientation == 90 || orientation == 270)
        {
            horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
        }
      
    }

    struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public float GetObjectOrientation()
    {
        float rotation = Mathf.Abs(Mathf.RoundToInt(this.transform.eulerAngles.z));
        return rotation;
    }
}
