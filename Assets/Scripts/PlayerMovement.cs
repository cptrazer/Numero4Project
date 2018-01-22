using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    GameObject waterDrop;
    [SerializeField]
    GameObject oilDrop;
    [SerializeField]
    GameObject vineObject;

    [SerializeField]
    GameObject greyCharacter;
    [SerializeField]
    GameObject redCharacter;
    [SerializeField]
    GameObject blueCharacter;
    [SerializeField]
    GameObject greenCharacter;
    [SerializeField]
    GameObject yellowCharacter;

    GameObject vineSub;

    public Controller controller;
    
    public float angle = 0;
    public Vector3 dir = Vector3.zero;

    private bool facing = false;
    //Maximum Jump Height
    public float jumpHeight = 4f;
    public float timeToJumpApex = 0.5f;
    public float playerSpeed = 8;
    public float fireSpeed = 20;
    public float oilSpeed = 15;
    public float accelerationTimeAirborne = 1f;
    public float accelerationTimeGrounded = 0.2f;
    public float amountOfWater = 0;
    public float amountOfOil = 0;
    public float force = 50;

    //Wall Jumping //////////////
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallStickTime = 0.5f;
    float timeToUnstick;
    bool glueTime;
    ////////////////////////////
    float slippery = 0.1f;

    public float wallSlidingMax = 3f;
    public string currentPowerUp = "none";
    [SerializeField]
    Material colorWheel;
    //Power Ups 
    string[] powers = { "none", "Fire", "Water", "Glue", "Vine" };
    int currentPowerIndex = 0;
    public float initialGravity;
    float initialSpeed;
    public float gravity;
    float jumpVelocity;
    bool hasUsedFire;
    public Vector3 velocity;
    float velocityXSmooth;
    [HideInInspector]
    public Vector2 StartPos;
    public bool vineActivate;

    private void Awake()    
    {
        controller = GetComponent<Controller>();
    }

     void Start()
    {

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        vineActivate = false;
        StartPos = new Vector2(Player.transform.position.x, Player.transform.position.y + 10);
        initialSpeed = playerSpeed;
        initialGravity = gravity;
    }

    void Update ()
    {
        PlayerMoves();
        CalculateAngle();        
        CycleWheel();
        accelerationTimeGrounded = slippery;
        PowerUp();

        print("Gravity " + gravity + "Jump Velocity " + jumpVelocity);
        if (Player.transform.position.y < -100 || Player.transform.position.y > 500)
        {
            Player.transform.position = StartPos;
        }

    }

    void PlayerMoves()
    {       
        //Left Right movement
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool wallJump = false;
        int wallDirX;
        if (controller.collisionsBools.left)
        {
            wallDirX = -1;
        }
        else
        {
            wallDirX = 1;
        }
        //acceleration
        float targetVelocity = move.x * playerSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmooth, (controller.collisionsBools.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        //Sticking to the walls
        if ((controller.collisionsBools.left || controller.collisionsBools.right) && !controller.collisionsBools.below && velocity.y < 0 && glueTime)
        {
            wallJump = true;

            if(velocity.y < -wallSlidingMax)
            {
                velocity.y = -wallSlidingMax;
            }
            if(timeToUnstick > 0)
            {
                velocityXSmooth = 0;
                velocity.x = 0;
                if(move.x != wallDirX && move.x != 0)
                {
                    timeToUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToUnstick = wallStickTime;
                }
            }
            else
            {
                timeToUnstick = wallStickTime;
            }
        }

        //Touching ground check
        if ((controller.collisionsBools.above && velocity.y > 0) || (controller.collisionsBools.below && velocity.y < 0))
        {
            velocity.y = 0;

            if(controller.collisionsBools.below && hasUsedFire == true)
            {
                hasUsedFire = false;
            }
        }


        //Jumping system as well as WallJumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallJump && glueTime)
            {
                if(wallDirX == move.x)
                {
                    velocity.x = wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                    print("WALL CLIMB");
                }
                else if(move.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                    print("WALL Jump");
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                    print("WALL LEAP");
                }
            }
            if (controller.collisionsBools.below)
            {
                velocity.y = jumpVelocity;
                Debug.Log("CONTROLLER BELOW TRUE");
            }

        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Flipping the player character
        if (targetVelocity < 0.0f && facing == false)
        {
            FlipPlayer();
        }
        else if(targetVelocity > 0.0f && facing == true)
        {
            FlipPlayer();
        }
        
        //Passive Powers
        if (currentPowerUp == "Fire")
        {
            playerSpeed = fireSpeed;
        }
        else if(currentPowerUp != "Fire")
        {
            playerSpeed = initialSpeed;
        }
        if(currentPowerUp == "Vine")
        {
            vineActivate = true;
        }
        if(currentPowerUp != "Vine")
        {
            vineActivate = false;
        }
        if(currentPowerUp == "Glue")
        {
            glueTime = true;
        }
        if(currentPowerUp != "Glue")
        {
            glueTime = false;
        }
        
    }

    void PowerUp()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentPowerUp == "Fire" && hasUsedFire == false)
            {
                Fire();
                hasUsedFire = true;
            }
            else if (currentPowerUp == "Water" && amountOfWater < 20)
            {
                //shoot Water in calculated angle
                Ray mousecoordinate = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                dir = mousecoordinate.origin - transform.position;
                dir.z = 0;

                //Force that shoots the projectile
                float shootForce = 1000; // needs to be tweaked
                GameObject projectile = Instantiate(waterDrop, transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(dir.normalized * shootForce);
                amountOfWater++;
            }
        }
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
        Debug.DrawLine(transform.position, mousecoordinate.origin, Color.red);

        dir = mousecoordinate.origin - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            angle += 360;
        }
    }

    void CycleWheel()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPowerIndex--;
            if (currentPowerIndex < 0)
            {
                currentPowerIndex = powers.Length - 1;
            }
            currentPowerUp = powers[currentPowerIndex];
            if (currentPowerUp == "none")
            {
                colorWheel.color = Color.white;
                greyCharacter.SetActive(true);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(false);
            }
            else if (currentPowerUp == "Fire")
            {
                colorWheel.color = Color.red;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(true);
                blueCharacter.SetActive(false);
            }
            else if (currentPowerUp == "Water")
            {
                colorWheel.color = Color.blue;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(true);
            }
            else if (currentPowerUp == "Glue")
            {
                colorWheel.color = Color.yellow;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(true);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(false);
            }
            else//when "Vine"
            {
                colorWheel.color = Color.green;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(true);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentPowerIndex++;
            if (currentPowerIndex > powers.Length - 1)
            {
                currentPowerIndex = 0;
            }
            currentPowerUp = powers[currentPowerIndex];
            if (currentPowerUp == "none")
            {
                colorWheel.color = Color.white;
                greyCharacter.SetActive(true);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(false);
            }
            else if (currentPowerUp == "Fire")
            {
                colorWheel.color = Color.red;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(true);
                blueCharacter.SetActive(false);
            }
            else if (currentPowerUp == "Water")
            {
                colorWheel.color = Color.blue;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(true);
            }
            else if (currentPowerUp == "Glue")
            {
                colorWheel.color = Color.yellow;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(false);
                yellowCharacter.SetActive(true);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(false);
            }
            else//when "Vine"
            {
                colorWheel.color = Color.green;
                greyCharacter.SetActive(false);
                greenCharacter.SetActive(true);
                yellowCharacter.SetActive(false);
                redCharacter.SetActive(false);
                blueCharacter.SetActive(false);
            }
        }

    }

    void Fire()
    {
        
        Ray mousecoordinate = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        dir = mousecoordinate.origin - transform.position;
        dir.z = 0;

        Debug.DrawRay(transform.position, -dir.normalized * force, Color.red, 3);
        velocity -= dir.normalized * force;

        Debug.Log(Player.GetComponent<PlayerMovement>().velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Slippery Surface" )
        {
            Debug.Log("SLIP");
            
            //Changes the acceleration speed
            if (collision.gameObject.name == "Frozen Ground(Clone)")
            {
                slippery = 2f;
            }
            else if (collision.gameObject.name == "Oil Track(Clone)")
            {
                slippery = 6f;
                playerSpeed += 5f;

            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Slippery Surface")
        {
            slippery = 0.2f;
        }
    }
}
