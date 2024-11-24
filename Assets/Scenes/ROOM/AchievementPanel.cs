using UnityEngine;
using UnityEngine.EventSystems;

public class AchievementPanel : MonoBehaviour
{
    public GameObject basketball;       

    void Start()
    {
        basketball.SetActive(false);
    }

    public void bballpanel ()
    {
        basketball.SetActive(true);
    }

    public void bballexit ()
    {
        basketball.SetActive(false);
    }
}
