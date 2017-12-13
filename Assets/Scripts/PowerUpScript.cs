using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    
    [SerializeField]
    GameObject slipperySurface;
    float TimePassed = 0;

    PlayerMovement movementRef;
    
    GameObject Player;

   

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player");
        TimePassed = 0;
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

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "floor" && this.tag == "Oil")
        {
            Instantiate(slipperySurface, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, -1), collision.gameObject.transform.rotation);

            Destroy(this.gameObject);
        }


        if (collision.gameObject.tag == "Waterfall" && this.tag == "Water")
        {
            Instantiate(slipperySurface, new Vector3(this.transform.position.x, collision.transform.position.y + 2.6f, -1), collision.gameObject.transform.rotation);
            print("hello iceman");
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag != "WaterFall" && this.tag == "Water" && collision.gameObject.tag != "Player")
        {
            Player.GetComponent<PlayerMovement>().amountOfWater--;

            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "WaterFall" && this.tag == "Oil" && collision.gameObject.tag != "Player")
        {
            Player.GetComponent<PlayerMovement>().amountOfOil--;

            Destroy(this.gameObject);
        }
    }


}
