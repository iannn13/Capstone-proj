using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level1complete : MonoBehaviour
{
    void Update()
    {

    }

    public void GoToMainMenu()
    {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        if (transitionManager != null)
        {
            transitionManager.FadeToScene(0);
        }
        else
        {
            Debug.LogError("SceneTransitionManager not found in the scene!");
        }
    }

    public void Level2()
    {
        SceneManager.LoadScene(13);
    }

}

