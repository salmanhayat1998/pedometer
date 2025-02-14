using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StepGoalData", menuName = "Pedometer/StepGoalData")]
public class StepGoalData : ScriptableObject
{
    public List<suggestedGoal> goalList = new List<suggestedGoal>();
   
}
[System.Serializable]
public class suggestedGoal
{
    public string name;
    public string[] goals;
}