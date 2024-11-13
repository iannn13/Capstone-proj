using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameNamespace // Use the same namespace or omit both
{
    public class MainMenu : MonoBehaviour
    {
        public PlayerDataManager playerDataManager;

        public void OnLoadGameButtonPressed()
        {
            if (playerDataManager != null)
            {
                playerDataManager.LoadGame();
            }
            else
            {
                Debug.LogError("PlayerDataManager not assigned in MainMenu.");
            }
        }
    }
}


