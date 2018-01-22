using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    public Animator PlayerAnimator;
    [SerializeField]
    public Animator ArmAnimator;
    public GameObject Player;

    string currentAnimation = "Idle";

    // Use this for initialization
    void Start () {
        PlayerAnimator = this.GetComponent<Animator>();
        Player = GameObject.Find("PlayerController");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && currentAnimation != "Jumping")
        {
            //jump animation plays once, then return to idle
            PlayerAnimator.Play("Start_Jump");
            currentAnimation = "Jumping";
        }
        else if(Player.GetComponent<PlayerMovement>().velocity.y < 0 && !Player.GetComponent<PlayerMovement>().controller.collisionsBools.below && Player.GetComponent<PlayerMovement>().currentPowerUp != "Glue")
        {
            PlayerAnimator.Play("Jump_Falling");
        }
        else if(Input.GetKeyDown(KeyCode.S) && currentAnimation != "Ducking" && currentAnimation != "Running" || ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))&& currentAnimation == "Ducked-Walk"))
        {
            //duck
            if(currentAnimation != "Ducked-Walk")
            {
                PlayerAnimator.Play("Start_Duck");
            }
            else
            {
                PlayerAnimator.Play("Ducking");
            }
            currentAnimation = "Ducking";
        }
        else if (Input.GetKey(KeyCode.S) && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && currentAnimation == "Ducking")
        {
            //duck walk
            PlayerAnimator.Play("Ducked-Walk");
            currentAnimation = "Ducked-Walk";
        }
        else if (Input.GetKeyUp(KeyCode.S) || (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && currentAnimation != "Idle" && currentAnimation != "Ducked-Walk")
        {
            //reverse duck?
            if(currentAnimation == "Ducking")
            {
                PlayerAnimator.Play("Stop_Duck");
            }
            else
            {
                PlayerAnimator.Play("Stop_Running");
            }
            
            currentAnimation = "Idle";
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && Input.GetKey(KeyCode.S) == false && currentAnimation != "Running")
        {
            //run
            PlayerAnimator.Play("Running");
            currentAnimation = "Running";
        }
        else if (Player.GetComponent<PlayerMovement>().velocity.y <= 0 && currentAnimation == "Jumping" && Player.GetComponent<PlayerMovement>().controller.collisionsBools.below)
        {
            //should become falling animation
            PlayerAnimator.Play("Jump_Getting_Up");
            currentAnimation = "Idle";
        }
        else if(!Player.GetComponent<PlayerMovement>().controller.collisionsBools.below && Player.GetComponent<PlayerMovement>().velocity.x == 0 && Player.GetComponent<PlayerMovement>().currentPowerUp == "Glue")
        {
            PlayerAnimator.Play("Hanging");
            currentAnimation = "Hanging";
        }
        else if(Input.GetKey(KeyCode.Mouse0))
        {
            ArmAnimator.Play("Throwing");
        }
    }
}
