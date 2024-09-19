using UnityEngine;
using System.Collections;

public class Mamatrigger2 : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Delay")]
    [SerializeField] private float dialogueStartDelay = 1f;

    private bool dialogueStarted;
    private bool playerInRange;

    private void Start()
    {
        dialogueStarted = false;
        playerInRange = false;

        // Subscribe to TeddyDialogue's OnDialogueComplete event
        MamaDialogue2.GetInstance().OnDialogueComplete += OnMamaDialogue2Complete;
    }

    private void Update()
    {
        // Start the dialogue if the player is in range and the dialogue hasn't started
        if (playerInRange && !dialogueStarted)
        {
            StartCoroutine(StartDialogueAfterDelay());
        }
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        if (!dialogueStarted)
        {
            dialogueStarted = true; // Prevents starting the dialogue multiple times
            yield return new WaitForSeconds(dialogueStartDelay);
            MamaDialogue2.GetInstance().EnterDialogueMode(inkJSON);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !dialogueStarted)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OnMamaDialogue2Complete()
    {
        // Do not reset dialogueStarted, so the dialogue cannot be retriggered
        Debug.Log("Dialogue finished and will not be repeated.");
    }
}
