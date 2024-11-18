using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class MamaLevel4 : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;  // Array for the 2 choice buttons
    private TextMeshProUGUI[] choicesText;

    [Header("Ink File")]
    [SerializeField] private TextAsset inkFile;

    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youpic;
    [SerializeField] private GameObject mama;
    [SerializeField] private GameObject mamapic;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueStory);
        movebutton.gameObject.SetActive(false);
        

        // Initialize the choices array

        // Automatically start dialogue after 1-second delay
        StartCoroutine(StartDialogueAfterDelay(1f));
    }

    private IEnumerator StartDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnterDialogueMode();
    }

    private void EnterDialogueMode()
    {
        if (inkFile == null)
        {
            Debug.LogError("Ink file not found!");
            return;
        }

        currentStory = new Story(inkFile.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        continueButton.gameObject.SetActive(true);
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string storyText = currentStory.Continue();
            StopAllCoroutines();
            StartCoroutine(TypeText(storyText));
            Debug.Log("Current Story Text: " + storyText); // Log the current story text
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";  // Clear the text
        continueButton.gameObject.SetActive(false);
        movebutton.gameObject.SetActive(true);

        // Hide the choice buttons after the dialogue is over
        foreach (GameObject choice in choices)
        {
            choice.gameObject.SetActive(false);
        }
    }
}
