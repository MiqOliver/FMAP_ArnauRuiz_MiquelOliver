using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RigidBody : MonoBehaviour {

    //constant
    private const float gravity = -9.81f;

    public float mass;

    [System.NonSerialized]
    public Vec3 position;
    [System.NonSerialized]
    public Vec3 velocity;
    [System.NonSerialized]
    public Quat rotation;

	// Use this for initialization
	void Start () {
        position = (Vec3)transform.position;
        velocity = new Vec3(0, 0, 0);
        rotation = new Quat();
    }
	
	// Update is called once per frame
	void Update () {
        //transform.position = position;
    }

    public float GetGravity() { return gravity; }
}
