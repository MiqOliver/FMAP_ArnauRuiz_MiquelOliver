using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float cameraSmooth = .1f;

    private Vec3 offset;

    // Use this for initialization
    void Start () {
        offset = (Vec3)transform.position - (Vec3)player.transform.position;
    }
	
	// Runs every frame, after all items have been processed
	void LateUpdate ()
    {
        Vec3 desiredPosition = (Vec3)player.transform.position + offset;
        Vec3 smoothedPosition = (Vec3)Vector3.Lerp(transform.position, (Vector3)desiredPosition, cameraSmooth);
        
        transform.position = (Vector3)smoothedPosition;
        transform.LookAt(player.transform.position);

    }
}
