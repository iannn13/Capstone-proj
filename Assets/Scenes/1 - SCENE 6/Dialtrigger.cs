using UnityEngine;
using System.Collections;

public class Dialtrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Delay")]
    [SerializeField] private float dialogueStartDelay = 1f; // Delay before starting dialogue

    private bool playerInRange;
    private bool dialogueStarted;

    private void Start()
    {
        playerInRange = true;
        dialogueStarted = false;

        ChismisDialogue.GetInstance().OnDialogueComplete += OnChismisDialogueComplete;

        StartCoroutine(StartDialogueAfterDelay());
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
        // Once the dialogue is complete, reset the dialogueStarted flag to allow triggering again if needed
        dialogueStarted = false;

        // If you want the dialogue not to repeat, you can keep this flag true
        // dialogueStarted = true; // Uncomment this to prevent the dialogue from repeating
    }
}
