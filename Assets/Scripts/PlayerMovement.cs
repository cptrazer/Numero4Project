using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private GameObject Player;

    Controller controller;
    
    public float angle = 0;
    public Vector3 dir = Vector3.zero;

    private bool facing = false;
    //Maximum Jump Height
    public float jumpHeight = 4f;
    public float timeToJumpApex = 0.5f;
    public float playerSpeed = 8;
    public float accelerationTimeAirborne = 1f;
    public float acceleraationTimeGrounded = 0.2f;
    public float amountOfWater = 0;
    public float amountOfOil = 0;
    
    public string currentPowerUp = "none";
    [SerializeField]
    Material colorWheel;

    string[] powers = { "none", "Fire", "Water", "Oil", "Vine" };
    int currentPowerIndex = 0;


    float gravity;
    float jumpVelocity;
    public Vector3 velocity;
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

        CalculateAngle();
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

        if(Input.GetKeyDown(KeyCode.Q))
        {
            currentPowerIndex--;
            if(currentPowerIndex < 0)
            {
                currentPowerIndex = powers.Length - 1;
            }
            currentPowerUp = powers[currentPowerIndex];
            if(currentPowerUp == "none")
            {
                colorWheel.color = Color.white;
            }
            else if(currentPowerUp == "Fire")
            {
                colorWheel.color = Color.red;
            }
            else if(currentPowerUp == "Water")
            {
                colorWheel.color = Color.blue;
            }
            else if(currentPowerUp == "Oil")
            {
                colorWheel.color = Color.yellow;
            }
            else//when "Vine"
            {
                colorWheel.color = Color.green;
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            currentPowerIndex++;
            if(currentPowerIndex > powers.Length - 1)
            {
                currentPowerIndex = 0;
            }
            currentPowerUp = powers[currentPowerIndex];
            if (currentPowerUp == "none")
            {
                colorWheel.color = Color.white;
            }
            else if (currentPowerUp == "Fire")
            {
                colorWheel.color = Color.red;
            }
            else if (currentPowerUp == "Water")
            {
                colorWheel.color = Color.blue;
            }
            else if (currentPowerUp == "Oil")
            {
                colorWheel.color = Color.yellow;
            }
            else//when "Vine"
            {
                colorWheel.color = Color.green;
            }
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

    public void CalculateAngle()
    {
        Ray mousecoordinate = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        dir = mousecoordinate.origin - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //Debug.Log(angle);
    }
}
