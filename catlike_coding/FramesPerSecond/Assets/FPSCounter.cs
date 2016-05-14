using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public int frameRange = 60;
    public int AverageFPS { get; private set; }
    public int MinFPS { get; private set; }
    public int MaxFPS { get; private set; }

    private float[] frameRateBuffer;
    private int frameRateIndex;

    private void Start()
    {
        if (frameRange <= 0)
        {
            frameRange = 1;
        }

        frameRateBuffer = new float[frameRange];
        frameRateIndex = 0;
    }

    private void Update()
    {
        UpdateBuffer();
        UpdateComputedValues();
    }

    private void UpdateComputedValues()
    {
        float sum = 0;
        float min = float.MaxValue;
        float max = 0;
       
        for (int i = 0; i < frameRateBuffer.Length; i++)
        {
            var val = frameRateBuffer[i];
            
            sum += val;
            if (val < min )
            {
                min = (int)val;
            }
            if (val > max) {
                max = (int)val;
            }
        }
        AverageFPS = (int) (sum / frameRange);
        MinFPS = (int)min;
        MaxFPS = (int)max;
    }

    private void UpdateBuffer()
    {
        frameRateBuffer[frameRateIndex] = 1f / Time.unscaledDeltaTime;
        frameRateIndex++;
        frameRateIndex %= frameRateBuffer.Length;
    }




}
