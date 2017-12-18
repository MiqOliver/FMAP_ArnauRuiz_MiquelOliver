using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quat {
    public float w, x, y, z;

    //CONSTRUCTOR
    public Quat(float a, float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = a;
    }
    public Quat(float a, Vec3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        this.w = a;
    }
    public Quat(Quaternion q)
    {
        this.x = q.x;
        this.y = q.y;
        this.z = q.z;
        this.w = q.w;
    }
    public Quat()
    {
        x = 0;
        y = 0;
        z = 0;
        w = 0;
    }



    //METHODS
    public static Quat AngleAxis(float a, Vec3 vec)
    {
        Quat q = new Quat();
        Vec3 v = vec.Normalize();
        float angle = a * Mathf.Deg2Rad;
        q.w = Mathf.Cos((angle) / 2);
        q.x = v.x * Mathf.Sin(angle / 2);
        q.y = v.y * Mathf.Sin(angle / 2);
        q.z = v.z * Mathf.Sin(angle / 2);
        q.Normalize();
        return q;
    }

    public void Normalize()
    {
        this.w /= this.Module();
        this.x /= this.Module();
        this.y /= this.Module();
        this.z /= this.Module();
    }
    public float Module()
    {
        return Mathf.Sqrt(w * w + x * x + y * y + z * z);
    }
    public static Quat Multiply(Quat lhs, Quat rhs)
    {
        Quat q = new Quat();
        q.w = rhs.w * lhs.w - rhs.x * lhs.x - rhs.y * lhs.y - rhs.z * lhs.z;
        q.x = rhs.w * lhs.x + rhs.x * lhs.w - rhs.y * lhs.z + rhs.z * lhs.y;
        q.y = rhs.w * lhs.y + rhs.x * lhs.z + rhs.y * lhs.w - rhs.z * lhs.x;
        q.z = rhs.w * lhs.z - rhs.x * lhs.y + rhs.y * lhs.x + rhs.z * lhs.w;
        return q;
    }
    public static float Angle(Quat lhs, Quat rhs)
    {
        lhs.Normalize();
        rhs.Normalize();

        rhs.Inverse();
        Quat q = Multiply(lhs, rhs);

        return 2 * Mathf.Acos(q.w) * Mathf.Rad2Deg;
    }
    public void Inverse()
    {
        this.x = -this.x;
        this.y = -this.y;
        this.z = -this.z;
    }
    public static Quat FromAxis(float angle, Vec3 v)
    {
        Quat axis_q = new Quat();

        // RADIANES

        v.Normalize();

        float radianAngle = angle * Mathf.Deg2Rad;

        axis_q.x = v.x * Mathf.Sin(radianAngle / 2);
        axis_q.y = v.y * Mathf.Sin(radianAngle / 2);
        axis_q.z = v.z * Mathf.Sin(radianAngle / 2);
        axis_q.w = Mathf.Cos(radianAngle / 2);

        axis_q.Normalize();

        return axis_q;
    }

    public Quat ToAxis(Quat q)
    {
        Quat new_q = new Quat();

        new_q.x = q.x / Mathf.Sqrt(1 - q.w * q.w);
        new_q.y = q.y / Mathf.Sqrt(1 - q.w * q.w);
        new_q.z = q.z / Mathf.Sqrt(1 - q.w * q.w);
        new_q.w = 2 * Mathf.Acos(q.w) * Mathf.Rad2Deg;

        return new_q;
    }

    //OPERADORS
    public static Quat operator *(Quat lhs, Quat rhs)
    {
        Quat q = new Quat();
        q.w = (rhs.w * lhs.w - rhs.x * lhs.x - rhs.y * lhs.y - rhs.z * lhs.z);
        q.x = (rhs.w * lhs.x + rhs.x * lhs.w - rhs.y * lhs.z + rhs.z * lhs.y);
        q.y = (rhs.w * lhs.y + rhs.x * lhs.z + rhs.y * lhs.w - rhs.z * lhs.x);
        q.z = (rhs.w * lhs.z - rhs.x * lhs.y + rhs.y * lhs.x + rhs.z * lhs.w);

        return q;
    }

    //CAST DE quat a Quaternion
    public static explicit operator Quat(Quaternion v)
    {
        return new Quat(v.w, v.x, v.y, v.z);
    }
    public static explicit operator Quaternion(Quat v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }
}