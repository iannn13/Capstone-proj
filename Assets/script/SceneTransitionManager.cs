using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
