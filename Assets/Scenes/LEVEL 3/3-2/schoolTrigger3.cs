using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class schoolTrigger3 : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject cue;


    private bool isFading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            cue.gameObject.SetActive(true);
            StartCoroutine(FadeToNextScene());
        }
    }

    private IEnumerator FadeToNextScene()
    {
        isFading = true;
        yield return StartCoroutine(Fade(1f));
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Color fadeColor = fadeImage.color;
        float startAlpha = fadeImage.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeColor.a = newAlpha;
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = targetAlpha;
        fadeImage.color = fadeColor;
    }
}
