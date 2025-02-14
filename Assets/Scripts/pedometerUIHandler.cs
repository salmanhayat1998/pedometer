using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pedometerUIHandler : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private Text stepCountText;
    [SerializeField] private Text distanceText;
    [SerializeField] private Text goalText;
    [SerializeField] private Text dayText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text caloriesText;
    [SerializeField] private Text BMIText;
    [SerializeField] private Text BMICategoryText;
    [SerializeField] private List<Text> suggestedGoalTexts;
    [Header("Image Components")]
    [SerializeField] private Image stepsProgressBar;
    [SerializeField] private Image bmiProgressBar;

    [Header("GameObjects")]

    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject getStartedPanel;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject bmiPanel,detailsPanel;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject bottomPanel;
    [SerializeField] private InputField weight, height;
    [SerializeField] private Dropdown targetGoal;
    [SerializeField] private StepGoalData suggestedGoals;
    private stepData stepData;
    private void Start()
    {
        if (StepDataHandler.Instance.isConnected())
        {
            stepData = StepDataHandler.Instance.CurrentStepData;
            if (StepDataHandler.Instance.isFirstTime())
            {
                loading.SetActive(true);
                menu.SetActive(false);
            }
            else
            {
                loading.SetActive(false);
                menu.SetActive(true);
                bottomPanel.SetActive(true);
                GetStepGoal();

            }
         
        }

        //Connect();
    }


    public void Connect()
    {
        StartCoroutine(connect());
    }
    private IEnumerator connect()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("connection", 1);
        loading.SetActive(false);

        if (StepDataHandler.Instance.isConnected())
        {
            //Debug.Log("connected");
            if (StepDataHandler.Instance.isFirstTime())
            {
                Debug.Log("first time");
                stepData = StepDataHandler.Instance.CurrentStepData;

                getStartedPanel.SetActive(true);
                PlayerPrefs.SetInt("isfirstTime", 1);

            }
            else
            {
                Debug.Log("second time");

                menu.SetActive(true);
                bottomPanel.SetActive(true);
                stepData = StepDataHandler.Instance.CurrentStepData;
                GetStepGoal();
            }

        }
    


    }
    void Update()
    {
        if (StepDataHandler.Instance.CurrentStepData != null)
        {
            stepCountText.text = StepDataHandler.Instance.CurrentStepData.stepCounts.ToString();
            goalText.text = "GOAL " + StepCounter.Instance.GetGoal();
            string distancePostfix = StepDataHandler.Instance.CurrentStepData.distanceWalked >= 1609 ? " mi" : "m";
            distanceText.text = StepDataHandler.Instance.CurrentStepData.distanceWalked.ToString("F2") + distancePostfix;
            int minutes = Mathf.FloorToInt(StepDataHandler.Instance.CurrentStepData.time / 60);
            int seconds = Mathf.FloorToInt(StepDataHandler.Instance.CurrentStepData.time % 60);

            string timeString = minutes > 0 ? minutes + " min : " + seconds + " s " : seconds + " s";
            //timeText.text = stepData.time.ToString("F1");
            timeText.text = timeString;
            dayText.text = StepDataHandler.Instance.CurrentStepData.Day;
            caloriesText.text = StepDataHandler.Instance.CurrentStepData.calories.ToString("0");

            stepsProgressBar.fillAmount = (float)StepDataHandler.Instance.CurrentStepData.stepCounts / StepCounter.Instance.GetGoal();
          //  Debug.Log((float)stepData.stepCounts / StepCounter.Instance.GetGoal());

        }
    }

    public void onSubmitDetails()
    {
        //try
        {
            DataHandler.Instance.userData.weight = int.Parse(weight.text);
            DataHandler.Instance.userData.height = int.Parse(height.text);
            bmiPanel.SetActive(true);
            showBMI();
            detailsPanel.SetActive(false);


        }
        //catch
        {

            
        }
    }
    private void showBMI()
    {
        BMIText.text = StepCounter.Instance.calculateBMI().ToString("F1");
        BMICategoryText.text = StepCounter.Instance.GetBMICategory(StepCounter.Instance.calculateBMI());
        bmiProgressBar.fillAmount = StepCounter.Instance.calculateBMI() / 40; // 40 max BMI
        bmiProgressBar.GetComponentInParent<CircleSlider>().fillamount = bmiProgressBar.fillAmount;
        bmiProgressBar.GetComponentInParent<CircleSlider>().progressAmount = StepCounter.Instance.calculateBMI();
        // suggestedGoalText.text = GetStepGoal().ToString();
        GetStepGoal();
    }

    public void GetStepGoal()
    {
        float bmi = StepCounter.Instance.calculateBMI();

        if (bmi < 18.5f)
        {
            for (int i = 0; i < suggestedGoalTexts.Count; i++)
            {
                suggestedGoalTexts[i].text = suggestedGoals.goalList[0].goals[i];
            }

            //return 8000; // Underweight

        }
        else if (bmi >= 18.5f && bmi < 24.9f)
        {
            for (int i = 0; i < suggestedGoalTexts.Count; i++)
            {
                suggestedGoalTexts[i].text = suggestedGoals.goalList[1].goals[i];
            }
            //return 10000; // Normal weight
        }
        else if (bmi >= 25f && bmi < 29.9f)
        {
            for (int i = 0; i < suggestedGoalTexts.Count; i++)
            {
                suggestedGoalTexts[i].text = suggestedGoals.goalList[2].goals[i];
            }
           // return 12000; // Overweight
        }
        else
        {
            for (int i = 0; i < suggestedGoalTexts.Count; i++)
            {
                suggestedGoalTexts[i].text = suggestedGoals.goalList[2].goals[i];
            }
            //return 8000; // Obese
        }
    }
    public void showHistoryPanel()
    {
        foreach (var item in DataHandler.Instance.inGameData.data)
        {
            var temp = Instantiate(itemPrefab, content);
            var tempComp = temp.GetComponent<item>();
            tempComp.stepCount.text = item.stepCounts + " / " + DataHandler.Instance.userData.goal;
            tempComp.dayText.text = item.Day;
            tempComp.calorieText.text = item.calories.ToString();
            tempComp.distanceTxt.text = item.distanceWalked.ToString();
            tempComp.progress.fillAmount = (float)item.stepCounts / (float)DataHandler.Instance.userData.goal;


        }
    }
    public void onSetGoal()
    {
        int value = int.Parse(targetGoal.options[targetGoal.value].text);
        //Debug.Log(value);
        StepCounter.Instance.setGoal(value);
    }
}
