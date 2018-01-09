using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

    public float amplitud;
    public float velocitat;
    public float initPosition;

    private Vec3 initPos;
    [SerializeField]
    public Vec3 axis;

	// Use this for initialization
	void Start () {
        initPos = (Vec3)this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //float posVarZ = -Mathf.Sqrt(amplitud * amplitud - posVar * posVar);

        //this.transform.position = new Vector3(initPos.x + posVarX, this.transform.position.y, initPos.z + posVarZ);
        if (axis.x == 1)
        {
            float posVar = amplitud * Mathf.Cos(/*velocitat * */ Time.time + initPosition);
            this.transform.position = new Vector3(initPos.x - posVar, this.transform.position.y, this.transform.position.z);
        }
        else if (axis.y == 1)
        {
            float posVar = amplitud * Mathf.Cos(/*velocitat * */Time.time + initPosition);
            this.transform.position = new Vector3(this.transform.position.x, initPos.y - posVar, this.transform.position.z);
        }
        else if (axis.z == 1)
        {
            float posVar = amplitud * Mathf.Cos(/*velocitat * */Time.time + initPosition);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, initPos.z - posVar);
        }
    }
}
