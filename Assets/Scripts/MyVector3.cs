using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vec3
{
    public float x, y, z;

    public Vec3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vec3(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vec3 Normalize()
    {
        return new Vec3(x / Module(), y / Module(), z / Module());
    }
    public float Module()
    {
        return Mathf.Sqrt(Mathf.Pow(this.x, 2) + Mathf.Pow(this.y, 2) + Mathf.Pow(this.z, 2));
    }
    public static float Dot(Vec3 lhs, Vec3 rhs)
    {
        return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z);
    }
    public static Vec3 Cross(Vec3 lhs, Vec3 rhs)
    {
        return new Vec3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.x * rhs.z - lhs.z * rhs.x, lhs.x * rhs.y - lhs.y * rhs.x);
    }
    public static float Angle(Vec3 lhs, Vec3 rhs)
    {
        return Mathf.Acos(Dot(lhs, rhs) / (lhs.Module() * rhs.Module()));
    }

    //NO FUNCIONA ENCARA
    public static Vec3 Lerp(Vec3 origin, Vec3 target, float time)
    {
        return (target - origin) * (Time.deltaTime / time);
    }

    //OPERADORS
    public static Vec3 operator +(Vec3 lhs, Vec3 rhs)
    {
        return new Vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.x + rhs.z);
    }
    public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
    {
        return new Vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.x - rhs.z);
    }
    public static Vec3 operator *(Vec3 v, float f)
    {
        return new Vec3(v.x * f, v.y * f, v.z * f);
    }
    public static Vec3 operator /(Vec3 v, float f)
    {
        return new Vec3(v.x / f, v.y / f, v.z / f);
    }
    public static bool operator == (Vec3 lhs, Vec3 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }
    public static bool operator != (Vec3 lhs, Vec3 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
    }

    //CAST DE Vector3 a vec3
    public static explicit operator Vec3(Vector3 v)
    {
        return new Vec3(v);
    }
    public static explicit operator Vector3(Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}

