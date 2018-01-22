using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Controller
{

    public LayerMask passengerMask;

    public float outerEdge = 10;
    public float innerEdge = 0;
    public float speed = 10;
    Vector3 dir;
    PlayerMovement PlayerMovementRef;
    string currentEdge = "inner";

    // Use this for initialization
   public override void Start()
    {
        base.Start();

        if (this.tag == "Horizontal")
        {
            innerEdge += transform.position.x;
            outerEdge += transform.position.x;
            dir = Vector3.right;
        }
        else if (this.tag == "Vertical")
        {
            innerEdge += transform.position.y;
            outerEdge += transform.position.y;
            dir = Vector3.up;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        UpdateRaycastOrigins();


        Vector3 velocity = dir * Time.deltaTime * speed;

        MovePassenger(velocity);

        if (currentEdge == "inner")
        {
            transform.Translate(velocity);
            if ((transform.position.x >= outerEdge && this.tag == "Horizontal") || (transform.position.y >= outerEdge && this.tag == "Vertical"))
            {
                currentEdge = "outer";
            }
        }
        else
        {
            transform.Translate(-velocity);
            if ((transform.position.x <= innerEdge && this.tag == "Horizontal") || (transform.position.y <= innerEdge && this.tag == "Vertical"))
            {
                currentEdge = "inner";
            }
        }

    }


    void MovePassenger(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);
        
        //Vertical Movements 
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;
            for (int i = 0; i < verticalRays; i++)
            {
                Vector2 rayOrigins = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigins += Vector2.right * (verticalRaySpace * i);
                RaycastHit2D hit2D = Physics2D.Raycast(rayOrigins, Vector2.up * directionY, rayLength, passengerMask);
                //Debug.DrawRay(rayOrigins, Vector3.up * directionY, Color.green);

                if (hit2D)
                {
                    if (!movedPassengers.Contains(hit2D.transform))
                    {
                        movedPassengers.Add(hit2D.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit2D.distance - skinWidth) * directionY;

                        hit2D.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }

        if(directionY != -1)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRays; i++)
            {
                Vector2 rayOrigins = raycastOrigins.topLeft + Vector2.right * (verticalRaySpace * i); 
                RaycastHit2D hit2D = Physics2D.Raycast(rayOrigins, Vector2.up, rayLength, passengerMask);

                Debug.DrawRay(rayOrigins, Vector3.up, Color.red);

                if (hit2D)
                {
                    Debug.Log("Player hit");
                    if (!movedPassengers.Contains(hit2D.transform))
                    {
                        movedPassengers.Add(hit2D.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        hit2D.transform.Translate(new Vector3(pushX, pushY));
                    }

                }
            }
        }

    }
}
