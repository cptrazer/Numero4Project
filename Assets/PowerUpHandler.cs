using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour {


    [SerializeField]
    GameObject waterDrop;
    [SerializeField]
    GameObject oilDrop;

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


        if (Input.GetButtonDown("Fire1"))
        {
            if (currentPowerUp == "Fire")
            {
                //thrust using fire into calculated angle
                float force = 75;
                if (angle>0)
                {
                    angle -= 180;
                }
                else
                {
                    angle += 180;
                }
                Player.GetComponent<PlayerMovement>().velocity.x += Mathf.Sin(angle) * force;
                Player.GetComponent<PlayerMovement>().velocity.y += Mathf.Cos(angle) * force;
                Debug.Log(Player.GetComponent<PlayerMovement>().velocity);
            }
            else if (currentPowerUp == "Water")
            {
                //shoot Water in calculated angle
                float shootForce = 0; // needs to be tweaked
                GameObject projectile = Instantiate(waterDrop);
                projectile.GetComponent<Rigidbody2D>().AddForce(dir * shootForce);
                amountOfWater++;
                Debug.Log(amountOfWater);
            }
            else if (currentPowerUp == "Oil")
            {
                //shoot Oil in calculated angle
                float shootForce = 0; // needs to be tweaked
                GameObject projectile = Instantiate(oilDrop);
                projectile.GetComponent<Rigidbody2D>().AddForce(dir * shootForce);
                amountOfOil++;
                Debug.Log(amountOfOil);
            }
            else if (currentPowerUp == "Vine")
            {
                //shoot vines in calculated angle
            }
        }
    }



    private void OnMouseDown()
    {
       
    }
}
