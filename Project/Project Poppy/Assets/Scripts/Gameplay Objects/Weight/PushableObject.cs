using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : Controller2D
{
    private PlayerController m_player;

    [SerializeField]
    private float gravity = -6f;

    private Vector3 velocity;
    [SerializeField]
    private LayerMask interactMask;

    private float input;

    private bool m_pushedByProjectile;

    public void SetDirectionInput(float _input)
    {
        input = _input;
    }

    private void HitByProjectile(Vector2 direction)
    {
        float orientation = GetObjectOrientation();

        if(direction.x != 0)
        {
            if(orientation == 0 || orientation == 180)
            {
                input = direction.x * 10;
                m_pushedByProjectile = true;
            }
        }
        if(direction.y != 0)
        {
             if(orientation == 90 || orientation == 270)
            {
                input = direction.y * 10;
                m_pushedByProjectile = true;
            }
        }
    }

    private void Update()
    {
        if(collisions.right || collisions.left)
        {
            input = 0;
            m_pushedByProjectile = false;
        }

        if (collisions.above || collisions.below)
        {
            velocity.y = 0;
        }

        float targetVelocity = input;

        velocity.x = targetVelocity;
        velocity.y += gravity * Time.deltaTime;
        Move(velocity * Time.deltaTime);
        screenwrappingBehaviour.ScreenWrap();

        if(!m_pushedByProjectile)
        {
            velocity.x = 0;
            input = 0;
        }
      
    }

    public bool HasDetectedPlayer()
    {
        CollisionOrientationInfo orientationInfo = new CollisionOrientationInfo();

        for (int j = 0; j < 2; j++)
        {
            orientationInfo.orientation = GetObjectOrientation();
            

            if (j == 0)
                orientationInfo.sideDirection = 1;
            else
            {
                orientationInfo.sideDirection = -1;
            }
            
            orientationInfo.rayLength = 0.25f;
            orientationInfo.SetInfoBasedOnOrientation();

            for (int i = 0; i < horizontalRayCount; i++)
            {
                //If we're moving left or if we're moving right
                Vector2 rayOrigin = (orientationInfo.sideDirection == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += orientationInfo.rayOriginDirection * (horizontalRaySpacing * i);
                Ray2D collisionRay = new Ray2D(rayOrigin, orientationInfo.collisionRayDirection * orientationInfo.sideDirection);

                RaycastHit2D hit = Physics2D.Raycast(collisionRay.origin, collisionRay.direction, orientationInfo.rayLength, interactMask);
                Debug.DrawRay(collisionRay.origin, collisionRay.direction, Color.green, orientationInfo.rayLength);

                if (hit.transform != null)
                {
                    float distance = Vector2.Distance(hit.transform.position, this.transform.position);
                   
                    if (distance > 0.80f)
                    {
                        if(m_player == null)
                            m_player = hit.transform.GetComponent<PlayerController>();

                        if(m_player.MovementController.collisions.fullyGrounded)
                            return true;
                    }
                }
            }
        }
        return false;
    }
    

}
