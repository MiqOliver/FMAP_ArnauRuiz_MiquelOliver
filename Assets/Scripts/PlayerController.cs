using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public enum State { DIRECTION, PROPULSION, STRINGS, JUMP, LANDING };

    public float elasticConstant;

    public GameObject resetButton;

    [System.NonSerialized]
    public Vec3 inicialPosition;
    [System.NonSerialized]
    public Quat inicialRotation;

    [System.NonSerialized]
    public Vec3 auxVel;
    [System.NonSerialized]
    public Vec3 direction;
    public bool grounded = true;

    public Text text;

    [System.NonSerialized]
    public State state;

    [System.NonSerialized]
    public float ang;
    private bool increaseAngle;
    [Range(0, 70)]
    public int maxAngle;

    [System.NonSerialized]
    public float velocity;
    private bool increase;
    [System.NonSerialized]
    public float maxForce;
    [System.NonSerialized]
    public float minForce;

    public Slider powerBar;
    public Image redArrow;

    [System.NonSerialized]
    public float powerSpeed;
    [System.NonSerialized]
    public int turnSpeed;

    public Vec3 endPos;
    private Vec3 previousPos;

    [ReadOnly]
    public float drag = 0.25f;

    void Start()
    {
        minForce = 0.1f;
        maxForce = Mathf.PI / 2;

        powerSpeed = (maxForce - minForce) / (Time.deltaTime * 180);
        turnSpeed = 50;
        
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
        
        auxVel = new Vec3(0.0f, 1.5f, 0.0f);
        direction = auxVel;
        text.text = "Force: " + velocity.ToString();
    }


    private void CheckPoint()
    {
        transform.position = (Vector3)previousPos;
        transform.rotation = (Quaternion)inicialRotation;
        powerSpeed = (maxForce - minForce) / (Time.deltaTime * 180);
        turnSpeed = 50;
        
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
        if(this.transform.position.y < -50){
            CheckPoint();
        }
        switch (state)
        {
            case State.DIRECTION:
                VisualDebug.DisableRenderer(VisualDebug.Vectors.GREEN, transform);
                VisualDebug.DisableRenderer(VisualDebug.Vectors.RED, transform);

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

                }
                VisualDebug.DrawVector(direction, VisualDebug.Vectors.GREEN, transform);
                break;

            case State.LANDING:

                Vec3 friction = Physics_functions.FrictionJump(direction, drag);
                VisualDebug.DrawVector(friction, VisualDebug.Vectors.RED, transform);
                //x = x0 + (v0 + Ft) * t
                direction += Physics_functions.TirParabolic(friction, transform) * Time.deltaTime;
                float aux = (direction * Time.deltaTime).Module();
                if (aux >= 0.0001f && (direction.z * Time.deltaTime) >= 0)
                    transform.position += transform.forward * aux;
                else
                {
                    state = State.DIRECTION;
                    transform.rotation = (Quaternion)inicialRotation;
                    direction = auxVel;
                }
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

    public void Reset()
    {
        transform.position = (Vector3)inicialPosition;
        transform.rotation = (Quaternion)inicialRotation;
        powerBar.gameObject.SetActive(false);
        Start();
    }
}
