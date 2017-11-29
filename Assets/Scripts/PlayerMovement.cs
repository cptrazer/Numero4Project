using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float playerSpeed = 10;
    private bool facing = false;
    public float playerJumpPower = 1250;
    private float moveX;

    private bool IsDucking = false;

    private Rigidbody2D PlayerBody;
    

    [SerializeField]
    private GameObject Player;

    private void Awake()
    {
        PlayerBody = Player.GetComponent<Rigidbody2D>();
    }


    void Update () {
        PlayerMoves();
	}

    void PlayerMoves()
    {
        //Controls
        moveX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
            Jump();
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
   
        //Flipping charater to face correct way
        if (moveX < 0.0f && facing == false)
            FlipPlayer();
        if (moveX > 0.0f && facing == true)
            FlipPlayer();

        //Phyiscs
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, PlayerBody.velocity.y);
    }

    void Jump()
    {
        PlayerBody.AddForce(Vector2.up * playerJumpPower);
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
