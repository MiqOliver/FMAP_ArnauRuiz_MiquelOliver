using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    Collider c;
    PlayerController player;
    public float drag;

	// Use this for initialization
	void Start () {
        c = GetComponent<Collider>();
        player = GameObject.Find("Frog").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (c.Collides(player.GetComponent<Collider>()))
        {
            transform.GetComponentInParent<ENTICourse.IK.InverseKinematics>().enabled = false;

            player.direction.y = 0;
            transform.GetComponentInParent<ENTICourse.IK.InverseKinematics>().enabled = false;
            player.grounded = true;

            player.powerSpeed = (player.maxForce - player.minForce) / (Time.deltaTime * 180);
            player.turnSpeed = 50;

            player.powerBar.minValue = player.minForce;
            player.powerBar.maxValue = player.maxForce;
            player.powerBar.value = player.velocity;

            player.velocity = player.minForce;

            player.ang = 0;
            player.drag = drag;

            if (player.state == PlayerController.State.JUMP)
            {
                player.state = PlayerController.State.LANDING;
            }

            this.enabled = false;
        }
	}
}
