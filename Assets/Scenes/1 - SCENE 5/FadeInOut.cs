using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup textCanvasGroup;
    [SerializeField] private CanvasGroup backgroundCanvasGroup;
    [SerializeField] private Image additionalImage;
    [SerializeField] private Image teacher;
    [SerializeField] private Image greatjobImage; 

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 2f;

    [Header("Dialogue Trigger")]
    [SerializeField] private DialogueManager5 dialogueManager;
    [SerializeField] private TextAsset inkJSON;

    private void Start()
    {
        additionalImage.canvasRenderer.SetAlpha(0f);
        greatjobImage.canvasRenderer.SetAlpha(0f); // Set the image to be initially invisible

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

        // Start dialogue
        dialogueManager.EnterDialogueMode(inkJSON);

        // Wait for the dialogue to finish
        while (dialogueManager.dialogueIsPlaying)
        {
            yield return null;
        }

        // Fade in the greatjob image after the dialogue ends
        greatjobImage.CrossFadeAlpha(1f, fadeDuration, false);

        // Wait for the image to be fully visible, then display it for a few seconds
        yield return new WaitForSeconds(fadeDuration + displayDuration);

        // Fade out the greatjob image
        greatjobImage.CrossFadeAlpha(0f, fadeDuration, false);

        // Wait for the image to fully fade out
        yield return new WaitForSeconds(fadeDuration);

       
        SceneManager.LoadScene(7); 
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
