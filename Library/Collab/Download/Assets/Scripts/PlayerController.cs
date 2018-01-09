using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(RigidBody))]
public class PlayerController : MonoBehaviour {

    public enum State { DIRECTION, PROPULSION, STRINGS, JUMP, LANDING };

    public float elasticConstant;
    //public bool isMoving;
    //Animator animation;
    //private RigidBody rb;
    public GameObject resetButton;
    
    private Vec3 inicialPosition;
    private Quat inicialRotation;

    private Vec3 auxVel;
    private Vec3 direction;
    public bool grounded = true;

    public Text text;
    //private int time_speed;

    private static State state;

    private float ang;
    private bool increaseAngle;
    [Range(0, 70)]
    public int maxAngle;

    private float velocity;
    private bool increase;
    [Range (15, 20)]
    public int maxForce;
    [Range(5, 10)]
    public int minForce;

    public Slider powerBar;
    public Image redArrow;
    //public Image forceBar;

    private float powerSpeed;
    private int turnSpeed;

    public Vec3 endPos;
    private Vec3 previousPos;

    private float drag = 0.9f;

    void Start()
    {

        powerSpeed = (maxForce - minForce) / (Time.deltaTime * 180);
        turnSpeed = 50;

        //powerBar = GameObject.Find("Power Bar").GetComponent<Slider>();
        powerBar.minValue = minForce;
        powerBar.maxValue = maxForce;
        powerBar.value = velocity;


        velocity = minForce;
        increase = true;
        state = State.DIRECTION;

        ang = 0;
        increaseAngle = true;
        maxAngle = 70;

        inicialPosition = (Vec3)transform.position;
        inicialRotation = (Quat)transform.rotation;

        //animation = GetComponent<Animator>();
        //rb = GetComponent<RigidBody>();
        
        auxVel = new Vec3(0.0f, 1.5f, 0.0f);
        direction = auxVel;
        //time_speed = 1;
        text.text = "Force: " + velocity.ToString();
    }


    private void CheckPoint()
    {
        transform.position = (Vector3)previousPos;
        transform.rotation = (Quaternion)inicialRotation;
        powerSpeed = (maxForce - minForce) / (Time.deltaTime * 180);
        turnSpeed = 50;

        //powerBar = GameObject.Find("Power Bar").GetComponent<Slider>();
        powerBar.minValue = minForce;
        powerBar.maxValue = maxForce;
        powerBar.value = velocity;


        velocity = minForce;
        increase = true;
        state = State.DIRECTION;

        ang = 0;
        increaseAngle = true;

        direction = auxVel;
        //time_speed = 1;
        text.text = "Force: " + velocity.ToString();
    }


    //Before rendering a frame
    void Update()
    {
        if(this.transform.position.y < -100){
            CheckPoint();
        }
        switch (state)
        {
            case State.DIRECTION:
                VisualDebug.DisableRenderer(VisualDebug.Vectors.GREEN, transform);

                redArrow.gameObject.SetActive(true);
                text.text = "Angle: " + Mathf.Round(ang).ToString();

                // TIMER - ang
                if (increaseAngle)
                {
                    ang += Time.deltaTime * turnSpeed;
                    if (ang >= maxAngle)
                        increaseAngle = false;
                }
                else if (!increaseAngle)
                {
                    ang -= Time.deltaTime * turnSpeed;
                    if (ang <= -maxAngle)
                        increaseAngle = true;
                }
                if (InputManager.Space())
                {
                    Quat rotate = Quat.AngleAxis(ang, (Vec3)this.transform.up);
                    this.transform.rotation = (Quaternion)(rotate * (Quat)this.transform.rotation);
                    redArrow.gameObject.SetActive(false);
                    velocity = minForce;
                    state = State.PROPULSION;
                    previousPos = (Vec3)transform.position;
                }

                float angArrow = ang;
                Quat rotateArrow = Quat.AngleAxis(ang-90, (Vec3)this.transform.forward * -1);
                redArrow.transform.rotation = (Quaternion)(rotateArrow * (Quat)this.transform.rotation);

                break;

            case State.PROPULSION:
                powerBar.gameObject.SetActive(true);
                text.text = "Force: " + Mathf.Round(velocity).ToString();

                // TIMER - FORCE
                if (increase)
                {
                    velocity += Time.deltaTime * powerSpeed;
                    if (velocity >= maxForce)
                        increase = false;
                }
                else if(!increase)
                {
                    velocity -= Time.deltaTime * powerSpeed;
                    if (velocity <= minForce)
                        increase = true;
                }
                powerBar.value = velocity;
             
                // INPUT
                if (InputManager.Space())
                {

                    state = State.STRINGS;
                    grounded = false;
                    powerBar.gameObject.SetActive(false);
                    direction += (Vec3)transform.forward;
                    //Vec3 endPos = Physics_functions.TrobarPosicioFinalTir((direction + (Vec3)this.transform.forward) * velocity, this.transform);
                    //GameObject.Find("base").GetComponent<ENTICourse.IK.InverseKinematics>().target = endPos;
                }
               
                break;

            case State.STRINGS:
                direction = Physics_functions.Molla(direction, transform);
                state = State.JUMP;
                break;
            case State.JUMP:
                if (!grounded)
                {
                    transform.position = (Vector3)Physics_functions.TirParabolic(direction, this.transform);
                    direction.y -= 9.81f * Time.deltaTime;
                    //endPos = Physics_functions.TrobarPosicioFinalTir(direction, (Vec3)this.transform.position);

                }
                VisualDebug.DrawVector(direction, VisualDebug.Vectors.GREEN, transform);
                break;
            case State.LANDING:
                Vec3 friction = Physics_functions.FrictionJump(direction, drag);
                VisualDebug.DrawVector(friction, VisualDebug.Vectors.RED, transform);
                direction += friction * Time.deltaTime;
                if (direction.x > 0.1f && direction.z > 0.1f)
                    transform.position += (Vector3)direction;
                else
                    state = State.DIRECTION;
                break;
            default:
                break;
        }
    }

    IEnumerator TimerSpeed(float s)
    {
        yield return new WaitForSeconds(s);
        grounded = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Ground")
        {
            direction.y = 0;
            grounded = true;
            transform.rotation = (Quaternion)inicialRotation;
            if(state == State.JUMP)
                state = State.LANDING;
            //direction = auxVel;
            powerSpeed = (maxForce - minForce) / (Time.deltaTime * 180);
            turnSpeed = 50;

            //powerBar = GameObject.Find("Power Bar").GetComponent<Slider>();
            powerBar.minValue = minForce;
            powerBar.maxValue = maxForce;
            powerBar.value = velocity;


            velocity = minForce;
            increase = true;
            //state = State.DIRECTION;

            ang = 0;
            increaseAngle = true;

            //drag = other.gameObject.GetComponent<>

            //animation = GetComponent<Animator>();
            //rb = GetComponent<RigidBody>();

            //auxVel = new Vec3(0.0f, 1.5f, 0.0f);
            //direction = auxVel;
            //time_speed = 1;
            text.text = "Force: " + velocity.ToString();

        }
    }

    /// <summary>
    /// returns the actual state of the state machine
    /// </summary>
    /// <returns></returns>
    public static State GetState()
    {
        return state;
    }

    public void Reset()
    {
        transform.position = (Vector3)inicialPosition;
        transform.rotation = (Quaternion)inicialRotation;
        powerBar.gameObject.SetActive(false);
        Start();
    }
}
