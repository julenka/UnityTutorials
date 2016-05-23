using UnityEngine;

public class Grapher01 : MonoBehaviour
{
    private int m_resolution = 10;
    [Range(10, 100)]
    public int Resolution = 10;

    private ParticleSystem.Particle[] points;

    void Start()
    {
        CreatePoints();
    }

    private void CreatePoints()
    {
        if (m_resolution < 10 || m_resolution > 100)
        {
            Debug.LogWarning("Grapher resolution out of bounds, resetting to minimum.", this);
            m_resolution = 10;
        }

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
            p.y = p.x;
            points[i].position = p;
        }

        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }
}