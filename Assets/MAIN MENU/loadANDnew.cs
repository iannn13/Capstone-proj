using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import for Button interaction

public class LoadAndNew : MonoBehaviour
{
    [Header("LOAD")]
    [SerializeField] private GameObject loadtxt;
    [SerializeField] private GameObject loadbutton;

    [Header("NEW")]
    [SerializeField] private GameObject newtxt;
    [SerializeField] private GameObject newbutton;

    private void Awake()
    {
        // Initially hide text elements
        loadtxt.SetActive(false);
        newtxt.SetActive(false);
        newbutton.SetActive(false);
        loadbutton.SetActive(false);

        // Check if a save file exists
        string path = System.IO.Path.Combine(Application.persistentDataPath, "playerData.json");

        if (System.IO.File.Exists(path))
        {
            // Enable the load button if save file exists
            loadbutton.SetActive(true);
        }
        else
        {
            // Disable the load button if no save file is found
            loadbutton.GetComponent<Button>().interactable = false;
        }
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
