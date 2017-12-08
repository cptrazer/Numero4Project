using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSys : MonoBehaviour {

    private GameObject Player;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    

	
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	void LateUpdate () {

        float x = Mathf.Clamp(Player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(Player.transform.position.y, yMin, xMax);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
	}
}
