using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    Collider c;
    Transform player;

	// Use this for initialization
	void Start () {
        c = GetComponent<Collider>();
        player = GameObject.Find("Frog").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (c.Collides(player.GetComponent<Collider>()))
        {
            Debug.Log("banana");
        }
	}
}
