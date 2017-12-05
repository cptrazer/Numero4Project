using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    
    [SerializeField]
    GameObject slipperyOil;
    public int amountOfOil = 0;
    float TimePassed = 0;

    // Use this for initialization
    void Start () {
		if(this.name == "Oil(Clone)")
        {
            amountOfOil++;
        }
	}

    private void Update()
    {
        TimePassed += Time.deltaTime;
        if(TimePassed >= 10 && this.gameObject.name == "Oil Track(Clone)")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "floor" && this.tag == "Oil")
        {
            Instantiate(slipperyOil, new Vector3(this.transform.position.x, this.transform.position.y - 1f, -1), Quaternion.identity);
            amountOfOil--;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Change friction
    }
}
