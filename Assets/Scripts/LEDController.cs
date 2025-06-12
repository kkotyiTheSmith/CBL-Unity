using UnityEngine;

public class LEDController : MonoBehaviour {
    private const float ON_DURATION = 5f;
    // Adjust this threshold based on microphone sensitivity
    private const float THRESHOLD = 0f;

    [SerializeField] private Light led;

    // I don't know where this needs to be initialized
    public MicInput micInput;
    private float timer = 0f;
    private bool isOn = false;
    
    void Start() {
        led.enabled = false;
    }
    
    void Update() {
        if (isOn) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                TurnOff();
                return;
            }
        }

        float dB = micInput.GetDecibels();
        Debug.Log(dB + " " + THRESHOLD);
        if (dB > THRESHOLD) {
            TurnOn();
        }
    }

    private void TurnOn() {
        Debug.Log("Turned on");
        isOn = true;
        timer = ON_DURATION;
        if (led != null) {
            Debug.Log("Setting active");
            led.enabled = true;
        }
    }

    private void TurnOff() {
        isOn = false;
        timer = 0f;
        if (led != null) {
            led.enabled = false;
        }
    }
}
