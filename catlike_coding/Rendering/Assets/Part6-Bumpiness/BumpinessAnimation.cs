using UnityEngine;
using System.Collections;

public class BumpinessAnimation : MonoBehaviour
{
    public Vector3 rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        var dr = Quaternion.EulerAngles(rotationSpeed * Time.deltaTime);
        transform.localRotation = dr * transform.localRotation;
    }
}
