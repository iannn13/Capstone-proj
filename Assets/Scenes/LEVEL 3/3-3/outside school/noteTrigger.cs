using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Delay")]
    [SerializeField] private float dialogueStartDelay = 1f;

    [Header("movebutton and inventory")]
    [SerializeField] private GameObject move;
    [SerializeField] private GameObject inventory;


    private bool dialogueStarted;
    private bool playerInRange;

    private void Start()
    {
        dialogueStarted = false;
        playerInRange = false;

        noteDial.GetInstance().OnDialogueComplete += OnCatDialogueComplete;

    }

    private void Update()
    {
        if (playerInRange && !dialogueStarted)
        {
            StartCoroutine(StartDialogueAfterDelay());
            move.SetActive(false);
            inventory.SetActive(false);
        }
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        if (!dialogueStarted)
        {
            dialogueStarted = true;
            yield return new WaitForSeconds(dialogueStartDelay);
            noteDial.GetInstance().EnterDialogueMode(inkJSON);
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

    private void OnCatDialogueComplete()
    {
        Debug.Log("Dialogue finished and will not be repeated.");
    }
}
