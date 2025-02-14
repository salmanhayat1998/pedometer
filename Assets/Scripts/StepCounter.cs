using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepCounter : SingeltonBase<StepCounter>
{
    

    [SerializeField] private StepCounterConfig config;
    [Header("Runtime Variables")]
    private float distanceWalked;
    [SerializeField] private int stepCount = 0;
    [SerializeField] private int goal = 50;
    [SerializeField] private float timeWalked;

    private float calerios;
    public Transform player;
    private Vector3 acceleration;
    private Vector3 prevAcceleration;

    private stepData stepData = new stepData();
    private float lastStepTime = 0f;

    public float moveSpeed = 2f;
    public float sprintSpeed = 5.3f;

    private ThirdPersonController controller;
    private void Start()
    {
        StepDataHandler.Instance.CheckForNewDay();
        controller = FindObjectOfType<ThirdPersonController>();
        prevAcceleration = player.position;
        StepDataHandler.Instance.CurrentStepData.Day = System.DateTime.Now.DayOfWeek.ToString();

    }
    private void FixedUpdate()
    {
        if (!StepDataHandler.Instance.isConnected()) return;
        Debug.Log("connected");
        DetectSteps(); 
        StepDataHandler.Instance.SaveDailySteps(StepDataHandler.Instance.CurrentStepData);

    }
    private void DetectSteps()
    {
        acceleration = player.position;
        float delta = (acceleration - prevAcceleration).magnitude;
        if (delta > config.threshold)
        {
            stepCount++;
            distanceWalked += delta*config.stepLength;
            calerios = (int) (stepCount * 0.04f);
            // calculate time travel //
            if (lastStepTime > 0)
            {
                timeWalked += Time.time - lastStepTime;
            }
            lastStepTime = Time.time; // Store last step time
            // ... (Calculate speed) ...
 
            updateStepsData();

            prevAcceleration = acceleration;
        }

    }
    void CalculateCalories( )
    {
        // Simplified calculation (without speed)


        // More accurate calculation with speed (if you have the MET(speed) function)
       //  calerios += (MET(speedKmh) * weightKg * 3.5f / 200f) * Time.deltaTime / 60f; // Calories per minute
    }
    float MET(float speedKmh)
    {
        // Example (replace with your actual data/formula)
        if (speedKmh < 5) return 3.0f;
        else if (speedKmh < 7) return 4.5f;
        else return 7.0f;
    }
    public float CalculateCaloriesBurned(float weightKg, float timeSeconds)
    {
        float met = controller.isRunning ? 7.0f : 3.5f; // MET values for running and walking
        float timeMinutes = timeSeconds / 3600;
        Debug.Log((met * weightKg * 3.5f / 200f) * timeMinutes);
        return (met * weightKg * 3.5f / 200f) * timeMinutes;
    }
    public float CalculateCaloriesBurned()
    {
        float baseCalories = stepCount * 0.04f;
        float speedFactor = controller.isRunning ? 2f : 1f;

        // Ensure calories accumulate over time, not revert back
        return baseCalories * speedFactor * Time.deltaTime;
    }
    public float calculateBMI()
    {
        float heightMeters = DataHandler.Instance.userData.height / 100f; // Convert cm to meters
        return DataHandler.Instance.userData.weight / (heightMeters * heightMeters);
    }
    public string GetBMICategory(float bmi)
    {
        if (bmi < 18.5f) return "Underweight";
        else if (bmi >= 18.5f && bmi < 24.9f) return "Normal Weight";
        else if (bmi >= 25f && bmi < 29.9f) return "Overweight";
        return "Obese";
    }
    public stepData GetStepsData() => StepDataHandler.Instance.CurrentStepData;
    public int GetGoal() => DataHandler.Instance.userData.goal;

    private void updateStepsData()
    {
        StepDataHandler.Instance.CurrentStepData.stepCounts = stepCount;
        StepDataHandler.Instance.CurrentStepData.distanceWalked = distanceWalked;
        StepDataHandler.Instance.CurrentStepData.time = timeWalked;
        StepDataHandler.Instance.CurrentStepData.calories = calerios;
    }
    public void ResetStepData()
    {
        stepCount = 0;
        distanceWalked = 0f;
        calerios = 0;
        timeWalked = 0f;
        StepDataHandler.Instance.CurrentStepData = new stepData();
    }
    public void LoadStepData(stepData data)
    {
        StepDataHandler.Instance.CurrentStepData = data;
        stepCount =     StepDataHandler.Instance.CurrentStepData.stepCounts;
        distanceWalked = StepDataHandler.Instance.CurrentStepData.distanceWalked;
        timeWalked =        StepDataHandler.Instance.CurrentStepData.time;
        calerios = StepDataHandler.Instance.CurrentStepData.calories;
    }
    public void setGoal(int value)
    {
        DataHandler.Instance.userData.goal = value;
    }
}

