using UnityEngine;

[System.Serializable]
public struct FloatRange {

    public float min, max;
    
    public float RandomInRange {
        get {
            return Random.Range(min, max);
        }
    }
}