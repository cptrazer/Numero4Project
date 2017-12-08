using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public LayerMask CollisionMask;
    BoxCollider2D collider2d;
    RaycastOrigins raycastOrigins;

    public int horizontalRays = 4;
    public int verticalRays = 4;

    public float maxClimbAngle = 80;
    public float maxDescendAngle = 75;

    float horizontalRaySpace;
    float verticalRaySpace;
    const float skinWidth = 0.015f;

    public CollisionInfo collisionsBools;


	void Start ()
    {
        collider2d = GetComponent<BoxCollider2D>();
        CalculateRaycast();
    }


    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisionsBools.Reset();

        collisionsBools.velocityOld = velocity;
        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        if(velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        
        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRays; i++)
        {
            Vector2 rayOrigins = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigins += Vector2.up * (horizontalRaySpace * i);
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigins, Vector2.right * directionX, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigins,Vector2.right * directionX, Color.red);

            if (hit2D)
            {
                float slopeAngle = Vector2.Angle(hit2D.normal, Vector2.up);
                if(i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisionsBools.descendingSlope)
                    {
                        collisionsBools.descendingSlope = false;
                        velocity = collisionsBools.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisionsBools.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit2D.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }
               
                //If the player is not climbing slope then the rays are not checked or if the SlopeAngle is more than maxClimbingAngle
                if(!collisionsBools.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit2D.distance - skinWidth) * directionX;
                    rayLength = hit2D.distance;

                    //Prevents stuttering when there's objects on a slope 
                    //Recalculates the velocity Y since it has not been recalculated normally
                    if (collisionsBools.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisionsBools.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisionsBools.left = directionX == -1;
                    collisionsBools.right = directionX == 1;

                   
                }
               
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRays; i++)
        {
            Vector2 rayOrigins = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigins += Vector2.right * (verticalRaySpace * i + velocity.x);
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigins, Vector2.up * directionY, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigins, Vector2.up * directionY, Color.red);

            if (hit2D)
            {
                velocity.y = (hit2D.distance - skinWidth) * directionY;
                rayLength = hit2D.distance;
                if (collisionsBools.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisionsBools.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }
                collisionsBools.below = directionY == -1;
                collisionsBools.above = directionY == 1;
            }
        }
        if (collisionsBools.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisionsBools.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisionsBools.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        //Calculates the Y distance
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            //Calculate the X distance
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisionsBools.below = true;
            collisionsBools.climbingSlope = true;
            collisionsBools.slopeAngle = slopeAngle;
        } 
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigins = (directionX == -1)?raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigins, -Vector2.up, Mathf.Infinity, CollisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0  && slopeAngle <= maxDescendAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisionsBools.slopeAngle = slopeAngle;
                        collisionsBools.descendingSlope = true;
                        collisionsBools.below = true;
                           
                    }
                }
            }
        }
    }



    //Updating the raycast origins so that it moves with the player
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    //Amount of rays and the calculation of the distance between the rays
    void CalculateRaycast()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRays = Mathf.Clamp(horizontalRays, 2, int.MaxValue);
        verticalRays = Mathf.Clamp(verticalRays, 2, int.MaxValue);

        horizontalRaySpace = bounds.size.y / (horizontalRays - 1);
        verticalRaySpace = bounds.size.x / (verticalRays - 1);
    }


    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
