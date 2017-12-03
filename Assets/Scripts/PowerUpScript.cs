using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    
    [SerializeField]
    GameObject slipperyOil;

	// Use this for initialization
	void Start () {
		
	}

    private void Update()
    {
        /*
        if(this.name == "Oil (Clone)")
        {
            this.transform.localScale = new Vector3(10 + Mathf.Sin(Time.deltaTime),10,10);
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "floor" && this.tag == "Oil")
        {
            Instantiate(slipperyOil, new Vector3(this.transform.position.x, this.transform.position.y - 1f, -1), Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Slippery floor" && this.tag == "Slippery floor")
        {
            Debug.Log("Fuse");
            Fuse(collision);
        }
    }

    void Fuse(Collision2D collision)
    {
        this.transform.Translate(Vector3.right * ((collision.transform.position.x - this.transform.position.x)/2));
        this.gameObject.transform.localScale += collision.transform.localScale;
        Destroy(collision.gameObject);
    }
}
