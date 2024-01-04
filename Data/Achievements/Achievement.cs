using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAchievement", menuName = "Data/AchievementsData")]
public class Achievement : ScriptableObject
{
    public string achievementName;
    public string description;
    public int requiredProgress;
    public int currentProgress;
    public bool IsCompleted { get; private set; }
    /*
    public bool CheckProgress(AchievementEvent achievementEvent)
    {
        // Implement logic to check if the event fulfills the achievement's criteria.
        // Update currentProgress as needed.
        // Return true if the achievement is completed, false otherwise.
    }*/

    public void Unlock()
    {
        IsCompleted = true;
    }
}
