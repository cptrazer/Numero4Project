using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour {


    GameObject Player;


    string currentPowerUp;
    float amountOfOil;
    float amountOfWater;
    float angle;
    Vector3 dir;

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        currentPowerUp = Player.GetComponent<PlayerMovement>().currentPowerUp;
        amountOfOil = Player.GetComponent<PlayerMovement>().amountOfOil;
        amountOfWater = Player.GetComponent<PlayerMovement>().amountOfWater;
        angle = GameObject.Find("Player").GetComponent<PlayerMovement>().angle;
        dir = GameObject.Find("Player").GetComponent<PlayerMovement>().dir;


        
    }
}
