using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    [SerializeField]
    protected LayerMask collisionMask;

    protected int horizontalRayCount = 4;
    protected int verticalRayCount = 4;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    protected BoxCollider2D objCollider;
    protected RayCastOrigins raycastOrigins;

    protected const float skinWidth = .015f;
    protected Bounds bounds;

    protected virtual void Start()
    {
        objCollider = this.GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    protected void UpdateRaycastOrigins()
    {
        bounds = objCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        SetRaycastOriginsBasedOnRotation();
    }

    private void SetRaycastOriginsBasedOnRotation()
    {
        float orientation = GetObjectOrientation();

        if (orientation == 0)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        else if (orientation == 180)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.max.x, bounds.max.y);
            raycastOrigins.bottomRight = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.min.y);
        }
        else if (orientation == 90)
        {
            raycastOrigins.bottomLeft = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.max.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.topRight = new Vector2(bounds.min.x, bounds.max.y);
        }
        else if (orientation == 270)
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

        if (orientation == 0 || orientation == 180)
        {
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
        if (orientation == 90 || orientation == 270)
        {
            horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
        }
    }

    protected struct RayCastOrigins
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
