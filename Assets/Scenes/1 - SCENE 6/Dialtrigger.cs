using UnityEngine;
using System.Collections;

public class Dialtrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Delay")]
    [SerializeField] private float dialogueStartDelay = 1f; 

    [Header("Image Fade")]
    [SerializeField] private CanvasGroup imageCanvasGroup; 
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float displayDuration = 2.0f;

    private bool playerInRange;
    private bool dialogueStarted;

    private void Start()
    {
        playerInRange = true;
        dialogueStarted = false;

        ChismisDialogue.GetInstance().OnDialogueComplete += OnChismisDialogueComplete;

        StartCoroutine(StartDialogueAfterDelay());

        // Initialize the image as hidden
        imageCanvasGroup.alpha = 0;
    }

    private void Update()
    {
    
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        if (playerInRange && !dialogueStarted)
        {
            yield return new WaitForSeconds(dialogueStartDelay);
            ChismisDialogue.GetInstance().EnterDialogueMode(inkJSON);
            dialogueStarted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !dialogueStarted)
        {
            playerInRange = true;
            StartCoroutine(StartDialogueAfterDelay());
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void OnChismisDialogueComplete()
    {
        dialogueStarted = false;

        StartCoroutine(FadeInAndOutImage());
    }

    private IEnumerator FadeInAndOutImage()
    {
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            imageCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageCanvasGroup.alpha = 1;

        // Wait for a few seconds with the image fully visible
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            imageCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageCanvasGroup.alpha = 0;
    }
}
