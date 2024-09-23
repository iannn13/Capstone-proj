using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class MamaLevel3 : MonoBehaviour
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

        // Initialize the choices array
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            int choiceIndex = index;
            choice.GetComponent<Button>().onClick.AddListener(() => MakeChoice(choiceIndex));
            index++;
        }

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
            DisplayChoices();

            // Check for specific text to toggle images
            if (storyText.Contains("Good morning, Mama.") || storyText.Contains("Yes but he went to te town's game event after.") || storyText.Contains("Not so long, cause he went to a game event."))
            {
                you.gameObject.SetActive(true);
                youpic.gameObject.SetActive(true);
                mama.gameObject.SetActive(false);
                mamapic.gameObject.SetActive(false);
            }
            else
            {
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                mama.gameObject.SetActive(true);
                mamapic.gameObject.SetActive(true);
            }
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


    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices than UI can support! Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // Display each choice text
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // Hide any unused choice buttons
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        // Show the continue button only if there are no choices
        continueButton.gameObject.SetActive(currentChoices.Count == 0);
    }

    private void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";  // Clear the text
        continueButton.gameObject.SetActive(false);

        // Hide the choice buttons after the dialogue is over
        foreach (GameObject choice in choices)
        {
            choice.gameObject.SetActive(false);
        }
    }
}
