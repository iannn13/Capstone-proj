using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MyGameNamespace
{
    public class PlayerDataManager : MonoBehaviour
    {
        public Transform playerTransform;

        private void Awake()
        {
            // Automatically assign playerTransform if not assigned in the Inspector
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
                else
                {
                    Debug.LogError("Player GameObject with tag 'Player' not found.");
                }
            }
        }

        public void SaveGame()
        {
            if (playerTransform == null)
            {
                Debug.LogError("Player Transform is not assigned.");
                return;
            }

            PlayerData playerData = new PlayerData();
            playerData.position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z };

            string json = JsonUtility.ToJson(playerData);
            string path = Path.Combine(Application.persistentDataPath, "playerData.json");
            File.WriteAllText(path, json);

            Debug.Log("Game Saved to " + path);
        }

        public void LoadGame()
        {
            if (playerTransform == null)
            {
                Debug.LogError("Player Transform is not assigned.");
                return;
            }

            string path = Path.Combine(Application.persistentDataPath, "playerData.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

                Vector3 loadedPosition = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
                playerTransform.position = loadedPosition;

                Debug.Log("Game Loaded from " + path);
            }
            else
            {
                Debug.LogWarning("Save file not found at " + path);
            }
        }
    }
}
