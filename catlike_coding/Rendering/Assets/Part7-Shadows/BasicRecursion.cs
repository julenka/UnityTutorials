using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BasicRecursion : MonoBehaviour
{
    public GameObject childNodePrefab;
    public Vector3 recursiveTranslate;
    public Vector3 recursiveRotate;
    public Vector3 startRotate;
    public Vector3 startScale = Vector3.one;
    public Vector3 startTranslate = Vector3.one;
    public Vector3 recursiveScale = Vector3.one;
    public int levels = 4;
    // Update is called once per frame
    void Update()
    {
        // Destroy all children
        for(int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        if(childNodePrefab != null)
        {
            GameObject root = Helper(levels);
            root.transform.parent = transform;
            root.transform.localPosition = startTranslate;
            root.transform.localRotation = Quaternion.Euler(startRotate);
            root.transform.localScale = startScale;
        }

    }

    private GameObject Helper(int level)
    {
        GameObject result = Instantiate(childNodePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        if(level == 0)
        {
            return result;
        }
        GameObject child = Helper(level - 1);
        child.transform.parent = result.transform;
        child.transform.localPosition = recursiveTranslate;
        child.transform.localRotation = Quaternion.Euler(recursiveRotate);
        child.transform.localScale = recursiveScale;
        return result;
    }
}
