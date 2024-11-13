using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1 : MonoBehaviour
{
   public void level1()
    {
        DataHandler.isNewGame = true;
        SceneManager.LoadSceneAsync(2);
    }
}
