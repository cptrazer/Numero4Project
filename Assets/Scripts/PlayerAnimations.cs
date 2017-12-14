using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    Animator PlayerAnimator;

	// Use this for initialization
	void Start () {
        PlayerAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            //jump animation plays once, then return to idle
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            //duck
        }
        else if (Input.GetKeyDown(KeyCode.S) && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)))
        {
            //duck walk
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            //reverse duck?
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            //run
        }
    }
}
