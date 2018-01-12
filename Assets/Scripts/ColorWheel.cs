using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour {


    GameObject Player;
    Vector3 offset;

    void Start()
    {
        Player = GameObject.Find("PlayerController");

        offset = transform.position - Player.transform.position;
    }
    
    void LateUpdate()
    {
        transform.position = Player.transform.position + offset;
    }
}
