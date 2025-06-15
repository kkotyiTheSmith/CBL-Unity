using UnityEngine;

public class LEDController : MonoBehaviour
{
    private const float ON_DURATION = 5f;
    // Adjust this threshold based on microphone sensitivity
    private const float THRESHOLD = -30f;

    [SerializeField] private Light led;

    public MicInput micInput;
    private float timer = 0f;
    private bool isOn = false;
    
    void Start()
    {
        Debug.Log("Start()");
        led.enabled = false;
    }
    
    void Update()
    {
        Debug.Log("Update()");
        if (isOn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                TurnOff();
                return;
            }
        }

        float dB = micInput.GetDecibels();
        if (dB > THRESHOLD)
        {
            Debug.Log(dB + " > " + THRESHOLD);
            TurnOn();
        } else {
            Debug.Log(dB + " <= " + THRESHOLD);
        }
    }

    private void TurnOn()
    {
        Debug.Log("Turning light on...");
        isOn = true;
        timer = ON_DURATION;
        if (led != null)
        {
            Debug.Log("Turned light on.");
            led.enabled = true;
        }
    }

    private void TurnOff()
    {
        Debug.Log("Turning light off...");
        isOn = false;
        timer = 0f;
        if (led != null)
        {
            Debug.Log("Turned light off.");
            led.enabled = false;
        }
    }
}
