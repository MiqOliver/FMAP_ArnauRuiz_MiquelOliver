using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Physics_functions {

    /// <summary>
    /// Funció per al tir parabòlic
    /// </summary>
    /// <param name="velocity">velocitat inicial</param>
    /// <param name="target">transform del objecte al que se li aplica</param>
    public static Vec3 TirParabolic(Vec3 velocity, Transform target)
    {
        return new Vec3(target.position.x + velocity.x * Time.deltaTime,
                            target.position.y + velocity.y * Time.deltaTime - 0.5f * 9.81f * Time.deltaTime * Time.deltaTime,
                            target.position.z + velocity.z * Time.deltaTime);
    }

    /// <summary>
    /// Funció per a trobar la posició en que aterrarà un objecte
    /// </summary>
    /// <param name="velocity">velocitat inicial</param>
    /// <param name="target">transform de l'objecte al que se li aplica</param>
    /// <returns></returns>
    public static Vec3 TrobarPosicioFinalTir(Vec3 velocity, Transform target)
    {
        float time = velocity.y / (0.5f * 9.81f);
        Vec3 p = new Vec3(target.position.x + velocity.x * time,
            target.position.y + velocity.y * time + 0.5f * -9.81f * time * time,
            //target.transform.position.y,
            target.position.z + velocity.z * time);
        Debug.Log(p.x + " " + p.y + " " + p.z);
        return p;
    }

    /// <summary>
    /// Funció que retorna la velocitat inicial del target generada per una molla
    /// </summary>
    /// <param name="direction">vector director, serà normalitzada en la funció</param>
    /// <param name="target">el transform de l'objecte</param>
    /// <returns></returns>
    public static Vec3 Molla(Vec3 direction, Transform target)
    {
        Vec3 velocity = direction.Normalize();
        float vel;
        float angularVel;
        float mass = 1;
        float K = target.GetComponent<PlayerController>().elasticConstant;       //constant elàstica molla
        float A = 0.7f;        //amplitud
        float initialPhase;

        float init = target.GetComponent<PlayerController>().velocity;

        //Trobem velocitat angular
        angularVel = 1 / Mathf.Sqrt(mass / K);
        Debug.Log(angularVel);

        //Fase inicial
        initialPhase = 3 * Mathf.PI / 2;

        //Trobem la velocitat a la que sortirà la granota
        vel = angularVel * A * Mathf.Abs(Mathf.Cos(init + initialPhase));

        return velocity * vel;
    }

    /// <summary>
    /// Funció que a partir de la velocitat i coeficient de fricció entre dos cossos en calcula la força de fricció generada, això s'ha d'alplicar mentere colisiona
    /// </summary>
    /// <param name="vel">velocitat relativa entre els dos cossos</param>
    /// <param name="drag">coeficient de fricció entre els cossos</param>
    /// <returns></returns>
    public static Vec3 FrictionJump(Vec3 vel, float drag)
    {
        return vel * -drag;
        #region comment

        //Vec3 friction;
        //float cFriction = 0.1f;
        //Vec3 velocity;
        //float acceleration;
        //float timeElapsed;
        //float mass = target.GetComponent<RigidBody>().mass;
        //float K = 1;

        //velocity = Molla(target);
        //friction = new Vec3(-cFriction * velocity, -cFriction * velocity, -cFriction * velocity);

        //Vec3 totalForce = 

        //timeElapsed = 1 / 4 * 2 * Mathf.PI * Mathf.Sqrt(mass / K);
        //velocity = acceleration * timeElapsed;
#endregion
    }
}
