using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour {

    public LayerMask playLayerMask;
    GameObject checkPointBlock;
    PlayerMovement playerMovementRef;
    
	void Start () {
        playerMovementRef = FindObjectOfType<PlayerMovement>();
	}
	
	void Update () {

        RaycastHit2D raycast = Physics2D.Raycast(this.transform.position, Vector2.up, 20f, playLayerMask);
        if (raycast)
        {
            playerMovementRef.StartPos = new Vector2(this.transform.position.x, this.transform.position.y + 5f);
        }
	}
}
