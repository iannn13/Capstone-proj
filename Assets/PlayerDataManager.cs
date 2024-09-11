using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

namespace MyGameNamespace
{
    public class PlayerDataManager : MonoBehaviour
    {
        public Transform playerTransform;

        private void Start()
        {
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

            PlayerData playerData = new PlayerData
            {
                position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z },
                sceneName = SceneManager.GetActiveScene().name
            };

            string json = JsonUtility.ToJson(playerData);
            string path = Path.Combine(Application.persistentDataPath, "playerData.json");
            File.WriteAllText(path, json);

            Debug.Log("Game Saved to " + path);
        }

        public void LoadGame()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerData.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

                if (SceneManager.GetActiveScene().name != loadedData.sceneName)
                {
                    StartCoroutine(LoadSceneAndSetPosition(loadedData));
                }
                else
                {
                    playerTransform.position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
                    Debug.Log("Game Loaded from " + path);
                }
            }
            else
            {
                Debug.LogWarning("Save file not found at " + path);
            }
        }

        private IEnumerator LoadSceneAndSetPosition(PlayerData loadedData)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadedData.sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerTransform.position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
                Debug.Log("Game Loaded in scene " + loadedData.sceneName);
            }
            else
            {
                Debug.LogError("Player GameObject with tag 'Player' not found in the loaded scene.");
            }
        }
    }
}
