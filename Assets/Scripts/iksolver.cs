using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iksolver : MonoBehaviour {

	// Array to hold all the joints
	// index 0 - root
	// index END - End Effector
	public GameObject[] joints;
	// The target for the IK system
	public GameObject targ;
	// Array of angles to rotate by (for each joint)
	public float[] theta;

	// The sine component for each joint
	[SerializeField]
	private float[] sin;
	// The cosine component for each joint
	[SerializeField]
	private float[] cos; 

	// To check if the target is reached at any point
	public bool done = false;
	// To store the position of the target
	private Vector3 tpos;

	// Max number of tries before the system gives up (Maybe 10 is too high?)
	[SerializeField]
	private int Mtries = 10;
	// The number of tries the system is at now
	[SerializeField]
	private int tries = 0;
	
	// the range within which the target will be assumed to be reached
	private float epsilon = 0.1f;


	// Initializing the variables
	void Start () {
		theta = new float[joints.Length];
		sin = new float[joints.Length];
		cos = new float[joints.Length];
		tpos = targ.transform.position;
	}
	
	// Running the solver - all the joints are iterated through once every frame
	void Update () {
		// if the target hasn't been reached
		if (!done)
		{	
			// if the Max number of tries hasn't been reached
			if (tries <= Mtries)
			{
				// starting from the second last joint (the last being the end effector)
				// going back up to the root
				for (int i = joints.Length - 2; i >= 0; i--)
				{
                    // The vector from the ith joint to the end effector
                    //TODO1
                    Vec3 r1 = (Vec3)(joints[joints.Length-1].transform.position - joints[i].transform.position);
                    // The vector from the ith joint to the target
                    //TODO2
                    Vec3 r2 = (Vec3)(targ.transform.position - joints[i].transform.position);
				   
					// to avoid dividing by tiny numbers
					if (r1.Module() * r2.Module() <= 0.001f)
					{
                        // cos ? sin? 
                        //TODO3
                        sin[i] = 0.0f;
                        cos[i] = 1.0f;

                    }
					else
					{
                        // find the components using dot and cross product
                        //TODO4
                        cos[i] = Vec3.Dot(r1.Normalize(), r2.Normalize());
                        sin[i] = Vec3.Cross(r1, r2).Module() / (r1.Module() * r2.Module());
					}

                    // The axis of rotation
                    //TODO5
                    Vec3 axis = Vec3.Cross(r1.Normalize(), r2.Normalize());

                    // find the angle between r1 and r2 (and clamp values if needed avoid errors)
                    //theta[i] = TODO6 
                    theta[i] = Mathf.Acos(Vec3.Dot(r1 / r1.Module(), r2 / r2.Module()));
                    Mathf.Clamp(theta[i], -Mathf.PI, Mathf.PI);
                    //Optional. correct angles if needed, depending on angles invert angle if sin component is negative
                    //TODO7
                    if (sin[i] < 0)
                        theta[i] *= -1;



                    // obtain an angle value between -pi and pi, and then convert to degrees
                    //TODO8
                    theta[i] = (float)SimpleAngle(theta[i]) * Mathf.Rad2Deg;
                    // rotate the ith joint along the axis by theta degrees in the world space.
                    //TODO9
                    joints[i].transform.rotation = Quaternion.AngleAxis(theta[i], new Vector3(axis.x, axis.y, axis.z)) * joints[i].transform.rotation;

                }
				
				// increment tries
				tries++;
			}
		}

        // find the difference in the positions of the end effector and the target
        // TODO10
        float dist = new Vec3(joints[joints.Length - 1].transform.position - targ.transform.position).Module();
		
		// if target is within reach (within epsilon) then the process is done
        //TODO11
		if (dist <= epsilon)
		{
			done = true;
		}
		// if it isn't, then the process should be repeated
		else
		{
			done = false;
		}
		
		// the target has moved, reset tries to 0 and change tpos
		if(targ.transform.position!=tpos)
		{
			tries = 0;
			tpos = targ.transform.position;
		}

        

	}

    
	// function to convert an angle to its simplest form (between -pi to pi radians)
	double SimpleAngle(double theta)
	{   //TODO
		//theta = 
		return theta;
	}
}
