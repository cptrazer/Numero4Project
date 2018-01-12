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

    public Controller controller;
    
    public float angle = 0;
    float currentAngle = 0;
    public Vector3 dir = Vector3.zero;

    private bool facing = false;
    //Maximum Jump Height
    public float jumpHeight = 4f;
    public float timeToJumpApex = 0.5f;
    public float playerSpeed = 8;
    public float accelerationTimeAirborne = 1f;
    public float accelerationTimeGrounded = 0.2f;
    public float amountOfWater = 0;
    public float amountOfOil = 0;
    public float force = 20;
    bool hasUsedFire = false;
    float slippery = 0.2f;

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

    void Update ()
    {
        CalculateAngle();
        PlayerMoves();
        accelerationTimeGrounded = slippery;
    }

    void PlayerMoves()
    {
        //Check whether there is ground below or not. Stops the gravity from accumilating 
        
        if (((controller.collisionsBools.above && velocity.y > 0) || (controller.collisionsBools.below && velocity.y < 0)) && Input.GetButtonDown("Fire1") == false)
        {
            velocity.y = 0;
            if(controller.collisionsBools.below && hasUsedFire == true)
            {
                hasUsedFire = false;
            }
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

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmooth, (controller.collisionsBools.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
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

        if (Input.GetButtonDown("Fire1") )
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

                float shootForce = 1000; // needs to be tweaked
                GameObject projectile = Instantiate(waterDrop, transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(dir.normalized * shootForce);
                amountOfWater++;
            }
            else if (currentPowerUp == "Oil" && amountOfOil < 20)
            {
                //shoot Oil in calculated angle
                Ray mousecoordinate = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                dir = mousecoordinate.origin - transform.position;
                dir.z = 0;

                float shootForce = 1000; // needs to be tweaked
                GameObject projectile = Instantiate(oilDrop, transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(dir.normalized * shootForce);
                amountOfOil++;
                Debug.Log(amountOfOil);
            }
            else if (currentPowerUp == "Vine")
            {
                //shoot vines in calculated angle
            }
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
        Debug.DrawLine(transform.position, mousecoordinate.origin, Color.red);

        dir = mousecoordinate.origin - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            angle += 360;
        }
    }

    void Fire()
    {
        
        Ray mousecoordinate = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        dir = mousecoordinate.origin - transform.position;
        dir.z = 0;

        Debug.DrawRay(transform.position, -dir.normalized * jumpVelocity, Color.red, 3);
        velocity -= dir.normalized * jumpVelocity;

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
                slippery = 3f;

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
