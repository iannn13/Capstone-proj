using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
  

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Save()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 2)
        {
            if (DataHandler.Instance != null)
            {
                DataHandler.Instance.ResetCash();
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.ClearInventory();
            }
        }
        SceneManager.LoadScene(currentSceneIndex);
    }

}

