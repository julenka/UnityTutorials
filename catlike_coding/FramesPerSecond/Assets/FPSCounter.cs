using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    public int FPS { get; private set; }

    private void Update()
    {
        FPS = (int)(1f / Time.unscaledDeltaTime);
    }
}
