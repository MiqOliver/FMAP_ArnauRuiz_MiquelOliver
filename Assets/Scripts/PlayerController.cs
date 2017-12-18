using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RigidBody))]
public class PlayerController : MonoBehaviour {

    public enum State { DIRECTION, PROPULSION, STRINGS, JUMP, LANDING };

    public float speed;
    //public bool isMoving;
    //Animator animation;
    private RigidBody rb;
    public GameObject resetButton;
    
    private Vector3 inicialPosition;
    private Quaternion inicialRotation;

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
    private Vector3 previousPos;
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

        inicialPosition = transform.position;
        inicialRotation = transform.rotation;

        //animation = GetComponent<Animator>();
        rb = GetComponent<RigidBody>();
        
        auxVel = new Vec3(0.0f, 1.5f, 0.0f);
        direction = auxVel;
        //time_speed = 1;
        text.text = "Force: " + velocity.ToString();
    }
    private void CheckPoint()
    {
        transform.position = previousPos;
        transform.rotation = inicialRotation;
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
                    Quaternion rotate = Quaternion.AngleAxis(ang, this.transform.up);
                    this.transform.rotation = rotate * this.transform.rotation;
                    redArrow.gameObject.SetActive(false);
                    velocity = minForce;
                    state = State.PROPULSION;
                    previousPos = transform.position;
                }

                float angArrow = ang;
                Quaternion rotateArrow = Quaternion.AngleAxis(ang-90, -this.transform.forward);
                redArrow.transform.rotation = rotateArrow * this.transform.rotation;

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

                    state = State.JUMP;
                    grounded = false;
                    powerBar.gameObject.SetActive(false);
                    Vec3 endPos = Physics_functions.TrobarPosicioFinalTir((direction + (Vec3)this.transform.forward) * velocity, this.transform);
                    GameObject.Find("base").GetComponent<ENTICourse.IK.InverseKinematics>().target = (Vector3)endPos;
                }
               
                break;

            case State.STRINGS:
                break;
            case State.JUMP:
                if (!grounded)
                {
                    Physics_functions.TirParabolic((direction + (Vec3)this.transform.forward) * velocity, this.transform);
                    direction.y -= 9.81f * Time.deltaTime;
                    //endPos = Physics_functions.TrobarPosicioFinalTir(direction, (Vec3)this.transform.position);
            
                }
                break;
            case State.LANDING:
                break;
            default:
                break;
        }

        //GetComponent<LineRenderer>().SetPositions(new Vector3[]{ transform.position, transform.position + (new Vector3(velocity.x, velocity.y, velocity.z) + transform.forward) * force});
    }

    IEnumerator TimerSpeed(float s)
    {
        //text.text = "Force: " + time_speed.ToString();
        yield return new WaitForSeconds(s);
        //time_speed++;
        //time_speed = time_speed % 10;
        //text.text = "Force: " + time_speed.ToString();
        //if (grounded)
        //    TimerSpeed(s);

        grounded = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Ground")
        {
            grounded = true;
            transform.rotation = inicialRotation;
            state = State.DIRECTION;
            direction = auxVel;
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


            //animation = GetComponent<Animator>();
            //rb = GetComponent<RigidBody>();

            auxVel = new Vec3(0.0f, 1.5f, 0.0f);
            direction = auxVel;
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
        transform.position = inicialPosition;
        transform.rotation = inicialRotation;
        powerBar.gameObject.SetActive(false);
        Start();
    }
}
