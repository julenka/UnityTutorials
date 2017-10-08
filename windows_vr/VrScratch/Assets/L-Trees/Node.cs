using UnityEngine;

public class Node
{
    // Option 1: use static line renderer and add points to it
    // Option 2: pass in a line renderer
    // Option 3: pass in a gameObject with the context so you can find a line renderer (or create it)

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isLeaf;
    private LineRenderer lineRenderer;

    public Node(Vector3 startPos, Vector3 endPos, bool isLeaf)
    {
        this.startPosition = startPos;
        this.endPosition = endPos;
        this.isLeaf = isLeaf;
    }

    public void Draw(GameObject context)
    {
        if (lineRenderer != null)
        {
            // Lerp 0 to 50, purple to orange
            float startY = 40.0f;
            lineRenderer.startColor = Color.Lerp(Color.green, Color.magenta, Mathf.Clamp01((startPosition.y - startY) / 50.0f));
            lineRenderer.endColor = Color.Lerp(Color.green, Color.magenta, Mathf.Clamp01((endPosition.y - startY) / 50.0f));
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }

    public void Initialize(LTreeGenerator context)
    {
        lineRenderer = MonoBehaviour.Instantiate<LineRenderer>(context.lineRendererPrefab);
        lineRenderer.gameObject.transform.SetParent(context.transform);
        lineRenderer.gameObject.name = string.Format("{0} -> {1}", startPosition.ToString(), endPosition.ToString());
    }

    public void Destroy()
    {
        if (lineRenderer.gameObject != null)
        {
            GameObject.Destroy(lineRenderer.gameObject);
        }
        lineRenderer = null;
    }
}
