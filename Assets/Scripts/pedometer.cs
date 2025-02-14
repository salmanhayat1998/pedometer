using UnityEngine;
using UnityEngine.UI;

public class pedometer : MonoBehaviour
{
    public float stepThreshold = 0.5f; // Adjust this value
    private Vector3 previousPosition;
    private int stepCount = 0;
    public Text stepCountText;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, previousPosition);

        if (distance > stepThreshold)
        {
            stepCount++;
            previousPosition = currentPosition;
        }

        //Debug.Log("Steps: " + stepCount); // Display step count in the console
        stepCountText.text = stepCount.ToString();  
    }
}