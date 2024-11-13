using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI; // Include the UI namespace for Image handling

namespace MyGameNamespace
{
    public class PlayerDataManager : MonoBehaviour
    {
        public Transform playerTransform;
        public Image saveAlertImage; // Reference to the Image element
        public float alertDuration = 2f; // Duration the image will be visible
        public GameObject pauseMenu; // Reference to the Pause Menu GameObject
        private bool isSaveAlertActive = false; // Track if the save alert is active
        public List<string> collectedItems = new List<string>(); // Track collected items

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

            // Hide the save alert image at the start
            if (saveAlertImage != null)
            {
                saveAlertImage.enabled = false;
            }

            // Make sure the pause menu is inactive at start
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
        }

        public void SaveGame()
        {
            if (playerTransform == null)
            {
                Debug.LogError("Player Transform is not assigned.");
                return;
            }

            // Close the pause menu if it's open
            if (pauseMenu != null && pauseMenu.activeSelf)
            {
                ClosePauseMenu();
            }

            // Save player data
            PlayerData playerData = new PlayerData
            {
                position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z },
                sceneName = SceneManager.GetActiveScene().name,
                collectedItems = collectedItems
            };

            string json = JsonUtility.ToJson(playerData);
            string path = Path.Combine(Application.persistentDataPath, "playerData.json");
            File.WriteAllText(path, json);

            Debug.Log("Game Saved to " + path);

            // Show save alert image and start the hide countdown
            if (saveAlertImage != null)
            {
                StartCoroutine(ShowSaveAlert());
            }
            PointsManager.Instance.SaveAchievementPoints();
        }

        private IEnumerator ShowSaveAlert()
        {
            // Show the save alert image
            saveAlertImage.enabled = true;
            isSaveAlertActive = true;

            // Wait for the specified duration
            yield return new WaitForSeconds(alertDuration);

            // Hide the save alert image after the duration
            saveAlertImage.enabled = false;
            isSaveAlertActive = false;
        }

        public void OpenPauseMenu()
        {
            if (pauseMenu != null)
            {
                // Ensure the pause menu is active
                pauseMenu.SetActive(true);
            }

            // If the save alert is still active (within the 2 seconds), leave it visible
            if (isSaveAlertActive)
            {
                Debug.Log("Save alert is still active, keeping it visible.");
            }
        }

        private void ClosePauseMenu()
        {
            // Deactivate the pause menu
            pauseMenu.SetActive(false);
        }

        public void LoadGame()
        {
            DataHandler.isNewGame = false;
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
                    collectedItems = loadedData.collectedItems;
                    Debug.Log("Game Loaded from " + path);
                }
            }
            else
            {
                Debug.LogWarning("Save file not found at " + path);
            }

            PointsManager.Instance.LoadAchievementPoints();
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
