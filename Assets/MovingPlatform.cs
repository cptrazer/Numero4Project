using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float outerEdge = 10;
    public float innerEdge = 0;
    public float speed = 10;
    Vector3 dir;
    string currentEdge = "inner";

	// Use this for initialization
	void Start () {
        if(this.tag == "Horizontal")
        {
            innerEdge += transform.position.x;
            outerEdge += transform.position.x;
            dir = Vector3.right;
        }
        else if(this.tag == "Vertical")
        {
            innerEdge += transform.position.y;
            outerEdge += transform.position.y;
            dir = Vector3.up;
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
        }
        else
        {
            transform.Translate(dir * Time.deltaTime * -speed);
            if ((transform.position.x <= innerEdge && this.tag == "Horizontal") || (transform.position.y <= innerEdge && this.tag == "Vertical"))
            {
                currentEdge = "inner";
            }
        }
	}
}
