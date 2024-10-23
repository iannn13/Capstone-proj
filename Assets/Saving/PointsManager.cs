using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;
    public int achievementPoints = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure PointsManager is not destroyed between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        achievementPoints += points;
        Debug.Log("Points Added: " + points + " | Total Points: " + achievementPoints);

        AchievementDisplay display = FindObjectOfType<AchievementDisplay>();
        if (display != null)
        {
            display.UpdateAchievementDisplay();
        }
        else
        {
            Debug.LogError("AchievementDisplay not found");
        }
    }


    public void SaveAchievementPoints()
    {
        PlayerPrefs.SetInt("AchievementPoints", achievementPoints);
        Debug.Log("Achievement Points Saved: " + achievementPoints);
    }

    public void LoadAchievementPoints()
    {
        achievementPoints = PlayerPrefs.GetInt("AchievementPoints", 0);
        Debug.Log("Achievement Points Loaded: " + achievementPoints);
    }
}
