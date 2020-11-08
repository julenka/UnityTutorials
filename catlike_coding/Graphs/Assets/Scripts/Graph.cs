using UnityEngine;

public class Graph : MonoBehaviour {

	[SerializeField]
	Transform pointPrefab = default;
    [SerializeField, Range(10,100)]
    int resolution = 10;
    Transform[] points;
    void Awake () {
        points = new Transform[resolution];
        float step = 2f / resolution;
        Vector3 position = default, scale = Vector3.one * step;
        for(int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
	}

    void Update() {
        for(int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            point.position = position;
        }
    }
}