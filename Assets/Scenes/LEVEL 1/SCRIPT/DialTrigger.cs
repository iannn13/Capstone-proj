using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Bag")]
    [SerializeField] public GameObject cue;
    [SerializeField] public GameObject bagTrigger;

    [Header("Mama GameObject")]
    [SerializeField] private GameObject mamaGameObject;

    [Header("Note1")]
    [SerializeField] private GameObject uiCanvas; // The Canvas GameObject
    [SerializeField] private Image messageImage; // The Image component for displaying the message
    [SerializeField] private Button continueButton; // The Button component for continuing

    [Header("Note2")]
    [SerializeField] private GameObject uiCanvas2;
    [SerializeField] private Image messageImage2;
    [SerializeField] private Button continueButton2;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Note3")]
    [SerializeField] private GameObject uiCanvas3;
    [SerializeField] private Image bagNote;

    [Header("Movement Buttons")]
    [SerializeField] private GameObject MoveButton; // Reference to the left movement button

    [Header("Delay Settings")]
    [SerializeField] private float continueButtonDelay = 2f; // Delay in seconds before continue button appears

    private bool playerInRange;
    private bool dialogueStarted;
    private mamaFadeout mamaFade;

    private void Start()
    {
        playerInRange = false;
        dialogueStarted = false;
        cue.SetActive(false);
        bagTrigger.SetActive(false);

        MamaDialogue.GetInstance().OnDialogueComplete += OnMamaDialogueComplete; // Subscribe to event

        // Ensure mamaGameObject is assigned
        if (mamaGameObject != null)
        {
            mamaFade = mamaGameObject.GetComponent<mamaFadeout>();
            if (mamaFade == null)
            {
                Debug.LogError("mamaFadeout component missing from Mama GameObject");
            }
        }
        else
        {
            Debug.LogError("Mama GameObject not assigned in the Inspector");
        }

        // Ensure UI elements are assigned
        if (uiCanvas != null && messageImage != null && continueButton != null)
        {
            uiCanvas.SetActive(false); // Hide the Canvas initially
            continueButton.interactable = false; // Disable continue button initially
        }
        else
        {
            Debug.LogError("UI elements not assigned in the Inspector");
        }

        // Ensure UI elements for note 2 are assigned
        if (uiCanvas2 != null && messageImage2 != null && continueButton2 != null)
        {
            uiCanvas2.SetActive(false); // Hide the Canvas initially
            continueButton2.interactable = false; // Disable continue button 2 initially
        }
    }

    private void Update()
    {
        if (playerInRange && !dialogueStarted)
        {
            Debug.Log("Player in range and dialogue not started. Starting dialogue...");
            MamaDialogue.GetInstance().EnterDialogueMode(inkJSON);
            dialogueStarted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !dialogueStarted)
        {
            Debug.Log("Player entered trigger area.");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger area.");
            playerInRange = false;
        }
    }

    public void OnMamaDialogueComplete()
    {
        Debug.Log("Dialogue complete. Starting fade out for Mama.");
        StartCoroutine(FinishDialogueSequence());
    }

    private IEnumerator FinishDialogueSequence()
    {
        if (mamaFade != null)
        {
            mamaFade.startFading(); // Start Mama fade out
            yield return new WaitForSeconds(0f); // Wait for fade out to complete (adjust as needed)
        }
        else
        {
            Debug.LogError("mamaFadeout component missing from Mama GameObject");
        }
        StartCoroutine(FadeOut());
        Destroy(mamaGameObject); // Destroy Mama game object
        StartCoroutine(FadeIn());

        // Show the image on the UI Canvas
        uiCanvas.SetActive(true);

        // Disable movement buttons when image is active
        MoveButton.gameObject.SetActive(false);

        // Delay before enabling the continue button
        yield return new WaitForSeconds(continueButtonDelay);

        // Enable continue button and make it interactable
        continueButton.interactable = true;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f; // Start fully transparent
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true); // Ensure the fadeImage is active

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // Start fully opaque
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // Hide the fadeImage after fading in
    }

    public void OnContinueButtonClicked()
    {
        Debug.Log("Continue button clicked.");
        uiCanvas.SetActive(false);
        uiCanvas2.SetActive(true);

        // Disable continue button 1 after it's clicked
        continueButton.interactable = false;

        // Start delay for continue button 2
        StartCoroutine(DelayContinueButton2());
    }

    private IEnumerator DelayContinueButton2()
    {
        // Delay before enabling the continue button 2
        yield return new WaitForSeconds(continueButtonDelay);

        // Enable continue button 2
        continueButton2.interactable = true;
    }

    public void OnContinueButton2Clicked()
    {
        Debug.Log("Continue button 2 clicked.");
        uiCanvas2.SetActive(false);
        uiCanvas3.SetActive(true);
        cue.SetActive(true);
        bagTrigger.SetActive(true);
        MoveButton.gameObject.SetActive(true);

        // Disable continue button 2 after it's clicked
        continueButton2.interactable = false;

        dialogueStarted = false;
        DisableTrigger();
    }

    private void DisableTrigger()
    {
        Debug.Log("Disabling trigger.");
        gameObject.SetActive(false);
    }
}
