using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note1 : MonoBehaviour
{
    [SerializeField] private GameObject notepanel;
    [SerializeField] private GameObject movebutton;

    void Start()
    {
        movebutton.SetActive(false);
        notepanel.SetActive(true);
    }


    public void Continue()
        {
        movebutton.SetActive(true);
        notepanel.SetActive(false);
        }
}
