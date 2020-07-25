using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

    private Transform player;
    private Vector3 offset;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset  = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
            

    }
}
