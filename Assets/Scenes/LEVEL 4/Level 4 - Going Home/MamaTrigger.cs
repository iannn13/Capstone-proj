using UnityEngine;
using System.Collections;

public class MamaTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Delay")]
    [SerializeField] private float dialogueStartDelay = 0.05f;


    private bool dialogueStarted;
    private bool playerInRange;

    private void Start()
    {
        dialogueStarted = false;
        playerInRange = false;

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
            MamaDial.GetInstance().EnterDialogueMode(inkJSON);
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

}
