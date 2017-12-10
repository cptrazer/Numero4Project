﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    
    [SerializeField]
    GameObject slipperySurface;
    public int amountOfOil = 0;
    public int amountOfWater = 0;
    float TimePassed = 0;

    PlayerMovement movementRef;
    
    [SerializeField]
    GameObject Player;

   

    // Use this for initialization
    void Start () {
	}

    private void Update()
    {
        TimePassed += Time.deltaTime;
        if(TimePassed >= 10 && this.gameObject.name == "Oil Track(Clone)")
        {
            Destroy(this.gameObject);
            Player.GetComponent<PlayerMovement>().amountOfOil--;
        }
        else if(TimePassed >=10 && this.gameObject.name == "Frozen Ground(Clone)")
        {
            Destroy(this.gameObject);
            Player.GetComponent<PlayerMovement>().amountOfWater--;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "floor" && this.tag == "Oil")
        {
            Instantiate(slipperySurface, new Vector3(this.transform.position.x, this.transform.position.y - 1f, -1), Quaternion.identity);
            
            Destroy(this.gameObject);
        }
       else if(collision.gameObject.tag == "WaterFall" && this.tag == "Water")
        {
            Instantiate(slipperySurface, new Vector3(this.transform.position.x, this.transform.position.y - 0.8f, -1), Quaternion.identity);

            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
       
        
        if(collision.gameObject.tag == "Player")
        {

            //Change friction
            //Can change the acceleration speed or the playerSpeed

        }


    }


}
