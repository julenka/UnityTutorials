using UnityEngine;

public class Node
{
    // Option 1: use static line renderer and add points to it
    // Option 2: pass in a line renderer
    // Option 3: pass in a gameObject with the context so you can find a line renderer (or create it)

    Vector3 startPosition;
    Vector3 endPosition;
    bool isLeaf;

    public Node(Vector3 startPos, Vector3 endPos, bool isLeaf)
    {
        this.startPosition = startPos;
        this.endPosition = endPos;
        this.isLeaf = isLeaf;
    }

    public void Draw(GameObject context)
    {
        Color c = isLeaf ? Color.cyan : Color.green;
        Debug.DrawLine(this.startPosition, this.endPosition, c);
    }
}
