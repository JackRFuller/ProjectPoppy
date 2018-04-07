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

        if(velocity.x != 0)
            SideOfObjectCollisions(ref velocity);

        if(velocity.y != 0)
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

        if (orientation == 0 || orientation == 180)
        {
            gravityDirection = Mathf.Sign(velocity.y);
            rayLength = Mathf.Abs(velocity.y) + skinWidth;
        }

        for (int i = 0; i < verticalRayCount; i++)
        {
            if(orientation == 0)
            {
                //If we're moving down or if we're moving up
                Vector2 rayOrigin = (gravityDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                Ray2D collisionRay = new Ray2D(rayOrigin, Vector2.up * gravityDirection);

                RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);                               
                Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);
                
                if(hit)
                {
                    velocity.y = (hit.distance - skinWidth) * gravityDirection;
                    rayLength = hit.distance;
                }

            }

            if(orientation == 180)
                {
                    //If we're moving down or if we're moving up
                    Vector2 rayOrigin = (gravityDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                    rayOrigin += Vector2.left * (verticalRaySpacing * i + velocity.x);
                    Ray2D collisionRay = new Ray2D(rayOrigin, -Vector2.up * gravityDirection);
                    RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);

                    Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.blue);

                    if (hit)
                    {
                        velocity.y = (hit.distance - skinWidth) * gravityDirection;
                        rayLength = hit.distance;
                    }
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

        if (orientation == 0 || orientation == 180)
        {
            sideDirection = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            if (orientation == 0)
            {
                //If we're moving left or if we're moving right
                Vector2 rayOrigin = (sideDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                Ray2D collisionRay = new Ray2D(rayOrigin, Vector2.right * sideDirection);

                RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);
                Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);

                if (hit)
                {
                    velocity.x = (hit.distance - skinWidth) * sideDirection;
                    rayLength = hit.distance;
                }

            }

            if (orientation == 180)
            {
                //If we're moving left or if we're moving right
                Vector2 rayOrigin = (sideDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.down * (horizontalRaySpacing * i);
                Ray2D collisionRay = new Ray2D(rayOrigin, Vector2.left * sideDirection);

                RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, rayLength, collisionMask);
                Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.red);

                if (hit)
                {
                    velocity.x = (hit.distance - skinWidth) * sideDirection;
                    rayLength = hit.distance;
                }
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

        }
        else if(orientation == 270)
        {

        }

    }

    void CalculateRaySpacing()
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
