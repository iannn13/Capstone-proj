using UnityEngine;
using UnityEngine.UI; // or TMPro if using TextMeshPro

public class AchievementDisplay : MonoBehaviour
{
    public Text AchievementText; // If using TextMeshPro, change this to TextMeshProUGUI

    private void Start()
    {
        UpdateAchievementDisplay();
    }

    public void UpdateAchievementDisplay()
    {
        AchievementText.text = "Score: " + PointsManager.Instance.achievementPoints;
    }
}
