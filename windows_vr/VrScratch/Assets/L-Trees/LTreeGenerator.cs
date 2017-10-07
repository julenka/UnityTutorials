using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LTreeGenerator : MonoBehaviour
{

    public Vector3 startPosition;
    public Vector3 startDirection;
    [Range(1, 10)]
    public int numLevels;
    public string axiom = "0";
    public int branchAngle = 45;
    [Range(0.001f, 1.0f)]
    public float branchLength = 1.0f;

    private Vector3 currentPosition;
    private Vector3 currentDirection;

    private Stack<Vector3> savedPositions;
    private Stack<Vector3> savedDirections;

    private string lString;
    private List<Node> lTreeNodes;

    private Node root;

    private LineRenderer lineRenderer;

    private void CreateLTree()
    {
        savedPositions = new Stack<Vector3>();
        savedDirections = new Stack<Vector3>();
        lTreeNodes = new List<Node>();

        currentDirection = startDirection;
        currentPosition = startPosition;

        // 1 - add non-leaf node
        //   - update current position
        // 0 - add leaf Node to nodes
        //   - update current position
        // [ - Push current position onto positions
        //   - Push current directin onto directions
        //   - update direction by rotating 45 to the left
        // ] - currentPosition = savedPositions.pop()
        //   - currentDirection = savedDirections.pop()
        //   - update direction by rotating 45 to the right
        foreach (char c in lString)
        {
            if (c == '1')
            {
                Vector3 startPos = currentPosition;
                currentPosition += currentDirection * branchLength;
                lTreeNodes.Add(new Node(startPos, currentPosition, false));
            }
            else if (c == '0')
            {
                Vector3 startPos = currentPosition;
                currentPosition += currentDirection * branchLength;
                lTreeNodes.Add(new Node(startPos, currentPosition, true));
            }
            else if (c == '[')
            {
                savedPositions.Push(currentPosition);
                savedDirections.Push(currentDirection);
                Quaternion q = Quaternion.Euler(0, 0, branchAngle);
                currentDirection = q * currentDirection;
            }
            else if (c == ']')
            {
                currentPosition = savedPositions.Pop();
                currentDirection = savedDirections.Pop();
                Quaternion q = Quaternion.Euler(0, 0, -branchAngle);
                currentDirection = q * currentDirection;
            }
        }
    }

    private void DrawLTree()
    {
        foreach (var node in lTreeNodes)
        {
            node.Draw(gameObject);
        }
    }

    private void ExpandLString(int iterationsLeft)
    {
        // Generate L string using rules and axiom.
        // Rules:
        // 0 -> 1[0]0
        // 1 -> 11
        // const: [,]
        if (iterationsLeft <= 0)
        {
            return;
        }
        string nextString = "";
        for (int i = 0; i < lString.Length; i++)
        {
            char c = lString[i];
            if (c == '0')
            {
                nextString += "1[0]0";
            }
            else if (c == '1')
            {
                nextString += "11";
            }
            else
            {
                nextString += c;
            }
        }
        lString = nextString;
        ExpandLString(iterationsLeft - 1);
    }

    private void GenerateLString(string axiom, int iterations)
    {
        lString = axiom;
        ExpandLString(iterations);
    }

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // setPositions takes an array.
        // We will want to add a position for every node.
        // We can get the positions, add to the array, and then set the position this way.
        // Pretty inefficient...
        //lineRenderer
        GenerateLString(axiom, numLevels);
        CreateLTree();
        DrawLTree();
    }
}
