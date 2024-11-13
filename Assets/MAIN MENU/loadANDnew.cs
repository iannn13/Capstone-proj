using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadANDnew : MonoBehaviour
{

    [Header("LOAD")]
    [SerializeField] private GameObject loadtxt;
    [SerializeField] private GameObject loadbutton;

    [Header("NEW")]
    [SerializeField] private GameObject newtxt;
    [SerializeField] private GameObject newbutton;

    void Awake() {
        loadtxt.SetActive(false);
        newtxt.SetActive(false);
        newbutton.SetActive(false);
        loadbutton.SetActive(false);
    }
    
    public void Load()
    {
        loadtxt.SetActive(true);
        loadbutton.SetActive(true);
        newtxt.SetActive(false);
        newbutton.SetActive(false);
    }

    public void New()
    {
        loadtxt.SetActive(false);
        loadbutton.SetActive(false);
        newtxt.SetActive(true);
        newbutton.SetActive(true);
    }
}
