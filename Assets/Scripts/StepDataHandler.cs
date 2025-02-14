using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StepDataHandler : SingeltonBase<StepDataHandler>
{

    private const string lastRecordedDateKey = "LastRecordedDate";

    public stepData CurrentStepData;
    public bool isConnected() => PlayerPrefs.GetInt("connection") == 1 ? true : false;
    public bool isFirstTime() => PlayerPrefs.GetInt("isfirstTime") == 0 ? true : false;
    public void SaveDailySteps(stepData data)
    {
     //   PlayerPrefs.SetInt(dailyStepsKey, data.stepCounts);
        int index = DataHandler.Instance.inGameData.data.IndexOf(data);
        if (index != -1)
        {
            // Update the existing entry
            DataHandler.Instance.inGameData.data[index] = data;
        }
        else
        {
            // Add new entry
            DataHandler.Instance.inGameData.data.Add(data);
        }

    }
    public void CheckForNewDay()
    {
        string currentDateString = System.DateTime.Now.ToString("yyyyMMdd");
        string lastRecordedDate = PlayerPrefs.GetString(lastRecordedDateKey, currentDateString);
        if (currentDateString != lastRecordedDate)
        {
            ResetDailySteps();
            PlayerPrefs.SetString(lastRecordedDateKey, currentDateString);
        }
        else
        {
            LoadDailySteps();
        }
    }
    private void ResetDailySteps()
    {
        //PlayerPrefs.SetInt(dailyStepsKey, 0);
        StepCounter.Instance.ResetStepData();
        DataHandler.Instance.inGameData.data.Add(new stepData());
        Debug.Log("New day, new steps! Counter reset.");
    }
    private void LoadDailySteps()
    {
        //int stepCount = PlayerPrefs.GetInt(dailyStepsKey, 0);
        //  StepCounter.Instance.LoadStepData(stepCount);
        string today = System.DateTime.Now.DayOfWeek.ToString();
        foreach (var item in DataHandler.Instance.inGameData.data)
        {
            if (item.Day == today)
            {
                CurrentStepData = item;
                StepCounter.Instance.LoadStepData(item);
            }

        }

        if(CurrentStepData == null)
        {
            CurrentStepData = new stepData
            {
                Day = today,

            };
        }


    }
}
