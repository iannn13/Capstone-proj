using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger3 : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("UI Button")]
    [SerializeField] private Button dialogueButton;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        dialogueButton.gameObject.SetActive(false);
        dialogueButton.onClick.AddListener(OndialogueButtonClicked);
    }


    private void Update()
    {
        if (playerInRange && !DialogueManager3.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            dialogueButton.gameObject.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
            dialogueButton.gameObject.SetActive(false);
        }
    }

    public void OndialogueButtonClicked()
    {
        DialogueManager3.GetInstance().EnterDialogueMode(inkJSON);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }

    }
}
