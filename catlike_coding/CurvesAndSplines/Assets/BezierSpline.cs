using UnityEngine;
using System.Collections;
using System;

public class BezierSpline : MonoBehaviour
{

    public Vector3[] points;

    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        float new_t;
        ComputeIandT(t, out new_t, out i);
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], new_t));
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        float new_t = t;
        ComputeIandT(t, out new_t, out i);
        return transform.TransformPoint(
            Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], new_t)) - transform.position;
    }

    private void ComputeIandT(float t_in, out float t, out int i)
    {
        t = t_in;
        if (t >= 1f)
        {
            t = 1;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
    }



    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }

    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;
    }


}
