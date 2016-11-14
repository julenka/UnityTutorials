using UnityEngine;
using System.Collections;

public class LightAnimator : MonoBehaviour {
    public float scaleFrequency = 1;
    public float scaleOffset = 1;
    public float rotateSpeed = 1;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }
	// Update is called once per frame
	void Update () {
        float scaleT = Time.timeSinceLevelLoad * scaleFrequency;
        transform.localScale = startScale * Mathf.Sin(scaleT) + Vector3.one *scaleOffset;

        Quaternion rotationDelta = Quaternion.EulerAngles(0, 0, rotateSpeed);
        transform.localRotation = rotationDelta * transform.localRotation;

	}
}
