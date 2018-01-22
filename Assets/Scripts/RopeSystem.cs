using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour {

    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public PlayerMovement playerMovement;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;

    //float resetGravity;
    public LineRenderer ropeRenderer;
    public LayerMask vineLayerMask;
    public float ropeMaxDistance = 20f;
    private List<Vector2> ropePositions = new List<Vector2>();
    private bool distanceSet;


    void Awake()
    {

        
    }

    private void Start()
    {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    void Update () {
        //The 360 cursor detection and shooting of raycast

        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if(aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        playerPosition = transform.position;

        if(playerMovement.vineActivate == true)
        {
            HandleInput(aimDirection);
            UpdateRopePosition();
           
        }
        else if (!playerMovement.vineActivate)
        {
            ResetRope();
        }
 
	}


    public void HandleInput(Vector2 aimDirection)
    {
        
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Click 0");
            if (ropeAttached) return;
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxDistance, vineLayerMask);

            //If the rope attaches 
            if (hit.collider != null)
            {
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    //If the List of RopePositions doesn't contain the hit.point position (position of impact of raycast) then do the following and add it into the list
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
                    ropePositions.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    ropeJoint.enabled = true;
                    ropeJoint.breakForce = 1;
                    ropeHingeAnchorSprite.enabled = true;
                    playerMovement.gravity = (200f * Time.deltaTime) * 10f;
                    //playerMovement.transform.position = new Vector3(hit.point.x * Time.deltaTime, hit.point.y * Time.deltaTime, 0);

                }
            }
            else
            {
                //If it doesn't hit then set the following to false
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
            }

        }

        if (Input.GetMouseButton(1))
        {
            ResetRope();
        }
    }

    private void UpdateRopePosition()
    {
        if (!ropeAttached)
        {
            return;
        }

        ropeRenderer.positionCount = ropePositions.Count + 1;

        for(var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if(i != ropeRenderer.positionCount - 1)
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                if(i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if(ropePositions.Count == 1)
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                else if(i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }

    public void ResetRope()
    {
        Debug.Log("Click 1");

        ropeJoint.enabled = false;
        ropeAttached = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        playerMovement.gravity = playerMovement.initialGravity;

    }
}
