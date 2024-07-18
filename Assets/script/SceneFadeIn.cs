using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    public Image fadeImage; // Reference to the Image component used for fading
    public float fadeDuration = 1f; // Duration of the fade-in effect

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned!");
            yield break;
        }

        fadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // Start fully opaque
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
