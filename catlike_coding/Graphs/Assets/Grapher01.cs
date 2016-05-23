using UnityEngine;

public class Grapher01 : MonoBehaviour
{
    private int m_resolution = 10;
    [Range(10, 100)]
    public int Resolution = 10;

    private ParticleSystem.Particle[] points;

    public enum FunctionOption
    {
        Linear,
        Exponential,
        Parabola,
        Sine
    }

    public FunctionOption function;

    private delegate float FunctionDelegate(float x);
    private static FunctionDelegate[] functionDelegates = {
        Linear,
        Exponential,
        Parabola,
        Sine
    };

    void Start()
    {
        CreatePoints();
    }

    private void CreatePoints()
    {
        points = new ParticleSystem.Particle[m_resolution];

        float increment = 1.0f / m_resolution;
        for (int i = 0; i < points.Length; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].color = new Color(x, 0f, 0f);
            points[i].size = 0.1f;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    void Update()
    {
        if (Resolution != m_resolution || points == null)
        {
            m_resolution = Resolution;
            CreatePoints();
        }

        for (int i = 0; i < m_resolution; i++)
        {
            Vector3 p = points[i].position;
            p.y = functionDelegates[(int)function](p.x);
            points[i].position = p;

            Color c = points[i].color;
            c.g = p.y;
            points[i].color = c;
        }

        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }

    private static float Linear(float x)
    {
        return x;
    }

    private static float Exponential(float x)
    {
        return x * x;
    }

    private static float Parabola(float x)
    {
        x = 2f * x - 1f;
        return x * x;
    }

    private static float Sine(float x)
    {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }
}