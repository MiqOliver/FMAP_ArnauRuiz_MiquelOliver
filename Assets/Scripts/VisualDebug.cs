using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualDebug {

    public enum Vectors { RED, GREEN, BLUE, GREY };

	public static void DrawVector(Vec3 v, Vectors type, Transform target)
    {
        LineRenderer lineRenderer = null;
        Color color = Color.black;
        switch (type)
        {
            case Vectors.RED:
                lineRenderer = target.GetChild(1).GetComponent<LineRenderer>();
                color = Color.red;
                break;
            case Vectors.GREEN:
                lineRenderer = target.GetChild(2).GetComponent<LineRenderer>();
                color = Color.green;
                break;
            case Vectors.BLUE:
                lineRenderer = target.GetChild(3).GetComponent<LineRenderer>();
                color = Color.blue;
                break;
            case Vectors.GREY:
                lineRenderer = target.GetChild(4).GetComponent<LineRenderer>();
                color = Color.grey;
                break;
            default:
                break;
        }
        lineRenderer.enabled = true;
        lineRenderer.material.color = color;
        lineRenderer.widthMultiplier = 0.005f;
        Vector3[] pos = { target.position, target.position + (Vector3)v };
        lineRenderer.SetPositions(pos);
    }
    
    public static void DisableRenderer(Vectors r, Transform target)
    {
        switch (r)
        {
            case Vectors.RED:
                target.GetChild(1).GetComponent<LineRenderer>().enabled = false;
                break;
            case Vectors.GREEN:
                target.GetChild(2).GetComponent<LineRenderer>().enabled = false;
                break;
            case Vectors.BLUE:
                target.GetChild(3).GetComponent<LineRenderer>().enabled = false;
                break;
            case Vectors.GREY:
                target.GetChild(4).GetComponent<LineRenderer>().enabled = false;
                break;
            default:
                break;
        }
    }
}

