using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour {

    [System.NonSerialized]
    public Vec3 position;

    public float radius;
    public float height;

    private void Start()
    {
        position = (Vec3)transform.position;
    }

    private void Update()
    {
        position = (Vec3)transform.position;
    }

    /// <summary>
    /// Comprova la colissió amb un altre objecte
    /// </summary>
    /// <param name="collider">collisder de l'altre objecte</param>
    /// <returns></returns>
    public bool Collides(Collider collider)
    {
        float inHeight = Mathf.Abs(collider.position.y - position.y);
        float inArea = Mathf.Sqrt((collider.position.x - position.x) * (collider.position.x - position.x) + (collider.position.z - position.z) * (collider.position.z - position.z));
        if (inHeight <= (height / 2 + collider.height / 2))
        {
            if (inArea <= (radius + collider.radius))
                return true;
        }
        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(position.x - radius, position.y + height / 2, position.z),
            new Vector3(position.x + radius, position.y + height / 2, position.z));
        Gizmos.DrawLine(new Vector3(position.x, position.y + height / 2, position.z - radius),
            new Vector3(position.x, position.y + height / 2, position.z + radius));
    }
}
