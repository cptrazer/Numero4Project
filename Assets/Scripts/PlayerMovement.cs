using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private GameObject Player;

    Controller controller;
 
    private bool facing = false;

    //Maximum Jump Height
    public float jumpHeight = 4f;
    public float timeToJumpApex = 0.5f;
    public float playerSpeed = 8;
    public float accelerationTimeAirborne = 1f;
    public float acceleraationTimeGrounded = 0.2f;


    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmooth;

    private void Awake()    
    {
        controller = GetComponent<Controller>();
    }

     void Start()
    {

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity " + gravity + "Jump Velocity " + jumpVelocity);

    }

    void Update () {
        PlayerMoves();

    }

    void PlayerMoves()
    {
        //Check whether there is ground below or not. Stops the gravity from accumilating 
        if (controller.collisionsBools.above || controller.collisionsBools.below)
        {
            velocity.y = 0;
        }

        //Controls
        //Left Right movement
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionsBools.below)
        {
            velocity.y = jumpVelocity;
        }
     
        float targetVelocity = move.x * playerSpeed;
        //acceleration
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmooth, (controller.collisionsBools.below) ? acceleraationTimeGrounded : accelerationTimeAirborne);
        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
     
        /*
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down") && IsDucking == false)
        {
            Duck();
            IsDucking = true;
        }
        else if (Input.GetKeyUp("s") || Input.GetKeyUp("down") && IsDucking == true)
        {
            NotDuck();
            IsDucking = false;
        }
        **/

        if(targetVelocity < 0.0f && facing == false)
        {
            FlipPlayer();
        }
        else if(targetVelocity > 0.0f && facing == true)
        {
            FlipPlayer();
        }


    }


    void NotDuck()
    {
        Vector2 duckScale = Player.transform.localScale;
        duckScale.y += 1;

        transform.localScale = duckScale;
    }
    void Duck()
    {
        Vector2 duckScale = Player.transform.localScale;
        duckScale.y -= 1;
        transform.localScale = duckScale;
    }

    void FlipPlayer()
    {
        facing = !facing;
        Vector2 LocalScale = Player.transform.localScale;
        LocalScale.x *= -1;
        transform.localScale = LocalScale;
    }
}
