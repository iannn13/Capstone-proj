using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup textCanvasGroup; 
    [SerializeField] private CanvasGroup backgroundCanvasGroup; 
    [SerializeField] private Image additionalImage;
    [SerializeField] private Image teacher;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f; 
    [SerializeField] private float displayDuration = 2f; 

    [Header("Dialogue Trigger")]
    [SerializeField] private DialogueManager5 dialogueManager; 

    private void Start()
    {
        additionalImage.canvasRenderer.SetAlpha(0f);

        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Initial state: Fully transparent
        textCanvasGroup.alpha = 0;
        backgroundCanvasGroup.alpha = 0;

        // Fade in text and background
        yield return StartCoroutine(FadeCanvasGroup(textCanvasGroup, 0, 1, fadeDuration));
        yield return StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0, 1, fadeDuration));

        // Wait for a few seconds
        yield return new WaitForSeconds(displayDuration);

        // Fade out text and background
        yield return StartCoroutine(FadeCanvasGroup(textCanvasGroup, 1, 0, fadeDuration));
        yield return StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1, 0, fadeDuration));

        // Fade in the additional image
        additionalImage.CrossFadeAlpha(1f, fadeDuration, false);

        // Wait for the image to be fully visible, then display it for a short time
        yield return new WaitForSeconds(fadeDuration + displayDuration);

        // Fade out the additional image
        additionalImage.CrossFadeAlpha(0f, fadeDuration, false);

        // Wait for the image to fully fade out
        yield return new WaitForSeconds(fadeDuration);

        teacher.gameObject.SetActive(false);

        // Start dialogue after the image fades out
        dialogueManager.StartDialogue();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
