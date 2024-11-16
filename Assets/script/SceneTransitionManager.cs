using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MyGameNamespace;
public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // Reference to the Image component used for fading
    public float fadeDuration = 1f; // Duration of the fade-out effect

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneIndex));
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned!");
            yield break;
        }

        string nextSceneName = SceneManager.GetSceneByBuildIndex(sceneIndex).name;



     PlayerDataManager playerDataManager = FindObjectOfType<PlayerDataManager>();
    if (playerDataManager != null)
    {
        playerDataManager.SaveGame();
        Debug.Log("Game autosaved.");
    }
    else
    {
        Debug.LogError("PlayerDataManager not found!");
    }
        fadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f; // Start fully transparent
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
