using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public GameObject confirmationPanel; // Reference to the confirmation panel

    private void Start()
    {
        // Ensure the confirmation panel is inactive at start
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
    }

    public void level1()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        if (File.Exists(path))
        {
            // If the save file exists, show the confirmation panel
            if (confirmationPanel != null)
            {
                confirmationPanel.SetActive(true);
            }
        }
        else
        {
            // No save file, proceed directly to starting a new game
            StartNewGame();
        }
    }

    // Called when the "Confirm" button on the panel is clicked
    public void ConfirmOverwrite()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        // Delete the existing save file
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }

        // Proceed with starting a new game
        StartNewGame();
    }

    // Called when the "Cancel" button on the panel is clicked
    public void CancelOverwrite()
    {
        // Hide the confirmation panel without doing anything
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
    }

    private void StartNewGame()
    {
        DataHandler.isNewGame = true;
        SceneManager.LoadSceneAsync(2);
    }
}
