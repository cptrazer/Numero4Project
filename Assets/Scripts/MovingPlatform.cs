using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float outerEdge = 10;
    public float innerEdge = 0;
    public float speed = 10;
    public Vector3 dir;
    string currentEdge = "inner";

	// Use this for initialization
	void Start () {
        if(this.tag == "Horizontal")
        {
            innerEdge += transform.position.x;
            outerEdge += transform.position.x;
        }
        else if(this.tag == "Vertical")
        {
            innerEdge += transform.position.y;
            outerEdge += transform.position.y;
        }
	}
	
	// Update is called once per frame
	void Update () {


		if(currentEdge == "inner")
        {
            transform.Translate(dir * Time.deltaTime * speed);
            if((transform.position.x >= outerEdge && this.tag == "Horizontal") || (transform.position.y >= outerEdge && this.tag == "Vertical"))
            {
                currentEdge = "outer";
            }

            if (currentEdge == "inner" && this.tag == "Vertical")
            {

                dir = Vector3.up;
            }

            if (currentEdge == "inner" && this.tag == "Horizontal")
            {

                dir = Vector3.right;
            }
        }
        else
        {
            transform.Translate(dir * Time.deltaTime * speed);
            if ((transform.position.x <= innerEdge && this.tag == "Horizontal") || (transform.position.y <= innerEdge && this.tag == "Vertical"))
            {
                currentEdge = "inner";
            }


            if (currentEdge == "outer" && this.tag == "Vertical")
            {

                dir = Vector3.down;
            }

            if (currentEdge == "outer" && this.tag == "Horizontal")
            {

                dir = Vector3.left;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.parent = transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.parent = null;
    }
}
