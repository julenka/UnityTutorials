using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstancingTest : MonoBehaviour
{

    public Transform prefab;
    public int numSpheres = 1000;
    public int sphereRadius = 10;

    public float pulseDuration = 5.0f;
    public float pulseMagnitude = 10.0f;

    struct ObjectBehavior
    {
        public float scale;
        public float duration;
        public float initialValue;
    };
    private ObjectBehavior[] objectBehaviors;
	// Use this for initialization
	void Start () {
        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        objectBehaviors = new ObjectBehavior[numSpheres];
        for(int i = 0; i < numSpheres; i++)
        {
            Transform t = Instantiate(prefab);
            t.localPosition = Random.insideUnitSphere * sphereRadius;
            t.SetParent(transform);
            properties.SetColor("_Tint", new Color(Random.value * 0.01f, Random.value, Random.value * 0.01f));
            t.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
            ObjectBehavior b;
            b.scale = Random.RandomRange(0.1f, pulseMagnitude);
            b.duration = Random.RandomRange(0.01f, pulseDuration);
            b.initialValue = Random.value;
            objectBehaviors[i] = b;
        }
    }

    private void Update()
    {
        for(int i = 0; i < numSpheres; i++)
        {
            Transform t = transform.GetChild(i);
            ObjectBehavior b = objectBehaviors[i];
            t.localScale = (Mathf.PingPong(Time.time + b.duration * b.initialValue, b.duration)) * b.scale * Vector3.one;
        }
    }
}
