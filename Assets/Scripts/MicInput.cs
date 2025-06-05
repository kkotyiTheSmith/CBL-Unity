using UnityEngine;

public class MicInput : MonoBehaviour
{
    private AudioClip micClip;
    private const int sampleWindow = 128;

    void Start()
    {
        micClip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
    }

    public float GetDecibels()
    {
        float[] samples = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(null) - sampleWindow + 1;
        if (micPosition < 0) return -1;

        micClip.GetData(samples, micPosition);

        float sum = 0f;
        foreach (var sample in samples)
        {
            sum += sample * sample;
        }

        float rms = Mathf.Sqrt(sum / sampleWindow);
        float dB = 20 * Mathf.Log10(rms / 0.1f); // 0.1f is a reference level, tune as needed

        return Mathf.Clamp(dB, -80f, 0f); // Limit range to avoid extreme values
    }
}
