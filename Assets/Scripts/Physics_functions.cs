using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Physics_functions {
    /// <summary>
    /// Funció per al tir parabòlic
    /// </summary>
    /// <param name="velocity">velocitat inicial</param>
    /// <param name="target">transform del objecte al que se li aplica</param>
    public static void TirParabolic(Vec3 velocity, Transform target)
    {
        target.GetComponent<Rigidbody>().position = new Vector3(target.position.x + velocity.x * Time.deltaTime,
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
}
