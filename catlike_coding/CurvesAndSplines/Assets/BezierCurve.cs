using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;

    /// <summary>
    /// This is called whenever the component is created, or reset
    /// </summary>
    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
            transform.position;
    }


    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
