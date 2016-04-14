using UnityEngine;
using System.Collections;

public class CircleGizmo : MonoBehaviour
{
    public int resolution = 10;
    private void OnDrawGizmosSelected()
    {
        float step = 2f / resolution;
        for (int i = 0; i <= resolution; i++)
        {
            ShowPoint(i * step - 1f, 1f);
            ShowPoint(i * step - 1f, -1f);
            // Only draw the left and right edges if we are not at the edge.
            if (i < resolution)
            {
                ShowPoint(-1f, i * step - 1f);
                ShowPoint(1f, i * step - 1f);
            }
        }
    }

    /// <summary>
    /// Shows a mapping from a point on a square to a point on a circle
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ShowPoint(float x, float y)
    {
        // Draw the point on the edge of square, in black
        Vector2 square = new Vector2(x, y);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(square, 0.025f);

        // Draw the corresponding pointon the circle, in white
        Vector2 circle;
        circle.x = square.x * Mathf.Sqrt(1f - square.y * square.y * 0.5f);
        circle.y = square.y * Mathf.Sqrt(1f - square.x * square.x * 0.5f);

        circle.Normalize();
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(circle, 0.025f);

        // Draw a line from square to circle, in yellow
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(circle, square);

        // Draw a line from center to circle vertex, in gray
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(Vector2.zero, circle);
    }
}
