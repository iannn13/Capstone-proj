using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tedsmomPoliceTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Delay")]
    [SerializeField] private float dialogueStartDelay = 1f;

    [Header("panel")]
    [SerializeField] private GameObject panel;


    private bool dialogueStarted;
    private bool playerInRange;

    private void Start()
    {
        dialogueStarted = false;
        playerInRange = false;

        tedsmomPoliceDial.GetInstance().OnDialogueComplete += OntedsmomPoliceDialComplete;

    }

    private void Update()
    {
        if (playerInRange && !dialogueStarted)
        {
            StartCoroutine(StartDialogueAfterDelay());
            panel.SetActive(true);
        }
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        if (!dialogueStarted)
        {
            dialogueStarted = true;
            yield return new WaitForSeconds(dialogueStartDelay);
            tedsmomPoliceDial.GetInstance().EnterDialogueMode(inkJSON);
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

    private void OntedsmomPoliceDialComplete()
    {
        Debug.Log("Dialogue finished and will not be repeated.");
    }
}
