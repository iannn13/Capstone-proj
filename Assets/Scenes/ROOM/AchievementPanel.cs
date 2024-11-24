using UnityEngine;
using UnityEngine.EventSystems;

public class AchievementPanel : MonoBehaviour
{
    public GameObject basketball;
    public GameObject gameboy;
    public GameObject teddybear;
    public GameObject buzz;
    public GameObject ps5;
    public GameObject woody;
    public GameObject car;
    public GameObject robot;


    void Start()
    {
        basketball.SetActive(false);
        gameboy.SetActive(false);
        teddybear.SetActive(false);
        buzz.SetActive(false);
        ps5.SetActive(false);
        woody.SetActive(false);
        car.SetActive(false);
        robot.SetActive(false);
    }

    public void bballpanel ()
    {
        basketball.SetActive(true);
    }

    public void bballexit ()
    {
        basketball.SetActive(false);
    }

    public void gameboypanel()
    {
        gameboy.SetActive(true);
    }

    public void gameboyexit()
    {
        gameboy.SetActive(false);
    }

    public void teddypanel()
    {
        teddybear.SetActive(true);
    }

    public void teddyexit()
    {
        teddybear.SetActive(false);
    }
    public void buzzpanel()
    {
        buzz.SetActive(true);
    }

    public void buzzexit()
    {
        buzz.SetActive(false);
    }
    public void ps5panel()
    {
        ps5.SetActive(true);
    }

    public void ps5exit()
    {
        ps5.SetActive(false);
    }
    public void woodypanel()
    {
        woody.SetActive(true);
    }

    public void woodyexit()
    {
        woody.SetActive(false);
    }
    public void carpanel()
    {
        car.SetActive(true);
    }

    public void carexit()
    {
        car.SetActive(false);
    }
    public void robotpanel()
    {
        robot.SetActive(true);
    }

    public void robotexit()
    {
        robot.SetActive(false);
    }
}
