using UnityEngine;
using UnityEngine.UI; // If you want to display the step count in a UI Text

public class Steps : MonoBehaviour
{
    public Text stepCountText; // Assign your UI Text object in the Inspector
    private int currentStepCount = 0;

    void Start()
    {
        // Check if the device supports the step counter sensor.  Important for compatibility.
        if (SystemInfo.supportsAccelerometer && SystemInfo.supportsGyroscope) // Check for both to be sure
        {
            Input.gyro.enabled = true; // Enable gyroscope for more accurate step detection.
        }
        else
        {
            Debug.LogError("Step counter sensor not supported on this device.");
            if (stepCountText != null)
            {
                stepCountText.text = "Not Supported";
            }
        }

        // Initialize the step counter
        currentStepCount = GetCurrentStepCount(); // Get any previous step count
    }

    void Update()
    {
        // Get the current step count.  This is the key part.
        int steps = GetCurrentStepCount();

        if (steps > currentStepCount)
        {
            currentStepCount = steps;
            if (stepCountText != null)
            {
                stepCountText.text = "Steps: " + currentStepCount;
            }
            Debug.Log("Steps: " + currentStepCount);
        }
    }


    private int GetCurrentStepCount()
    {
#if UNITY_ANDROID && !UNITY_EDITOR // Only compile this for Android builds, not the editor
        // Use the AndroidJavaClass to access the StepCounter sensor.
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaClass sensorManager = new AndroidJavaClass("android.hardware.SensorManager"))
        using (AndroidJavaObject sensor = sensorManager.CallStatic<AndroidJavaObject>("getDefaultSensor", sensorManager.GetStatic<int>("TYPE_STEP_COUNTER")))
        {
            if (sensor != null)
            {
                // You might need to implement a SensorEventListener in Java and call it from here.
                // This is more complex and usually not necessary. The TYPE_STEP_COUNTER is usually handled by the system.
                // The following line is usually enough:
                return GetStepCountFromSensor(); // You would get the step count from the sensor here.
            }
            else
            {
                Debug.LogError("Step counter sensor not found.");
                return 0;
            }
        }
#else
        return 0; // Return 0 in the editor or on other platforms
#endif
    }


    // Placeholder for the actual sensor reading.  This is device-specific.
    private int GetStepCountFromSensor()
    {
        // This is where you would interact with the Android SensorEventListener.
        // It's often handled implicitly by Android.  The TYPE_STEP_COUNTER usually provides an accumulated step count.
        // You generally don't need to implement a listener yourself.
        // This example just returns a dummy value.
        // In a real app, you would get this value from the Android API.
        // You might need to use a plugin or write a small Java/Kotlin plugin to do this.

        // IMPORTANT:  This is often handled by the OS.  You may NOT need a listener.
        // The TYPE_STEP_COUNTER usually gives you an accumulated count.
        // For testing, you might want to simulate steps.

        // For testing in the editor, simulate steps:
        // return (int)(Time.time * 2); // Example: 2 steps per second

        // For a real build, you might not even need this function, just rely on GetCurrentStepCount()
        // If you need the listener, you'll need to implement the Java/Kotlin part.

        return 0; // Or return a value you get from the Android API.
    }
}