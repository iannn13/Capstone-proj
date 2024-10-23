using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteManager : MonoBehaviour
{
    private bool pointsAwarded = false;

    private void Start()
    {
        // Award the achievement points when the scene is loaded (if not already awarded)
        AwardAchievementPoints();
    }

    private void AwardAchievementPoints()
    {
        if (!pointsAwarded)
        {
            Debug.Log("Awarding points.");
            PointsManager.Instance.AddPoints(50);
            pointsAwarded = true;
        }
        else
        {
            Debug.Log("Points already awarded, skipping.");
        }
    }


    // If you have a button that proceeds to the next level:
    public void OnNextLevelButton()
    {
        // Award achievement points just in case (safety check) and load the next level
        AwardAchievementPoints();
        // Load next scene logic here, e.g., SceneManager.LoadScene("NextLevel");
    }
}
