using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class MamaLevel5 : MonoBehaviour
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


    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject mamaname;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueStory);
        

        // Initialize the choices array

        // Automatically start dialogue after 1-second delay
        StartCoroutine(StartDialogueAfterDelay(1f));

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            int choiceIndex = index;
            choice.GetComponent<Button>().onClick.AddListener(() => MakeChoice(choiceIndex));
            index++;
        }
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
            if (storyText.Contains("Good morning, son.")
            || storyText.Contains("Please go straight home as soon as possible.") || 
            storyText.Contains("Because someone needs to look after junior.") ||
            storyText.Contains("I'll be getting the new car that Papa left for us.") ||
            storyText.Contains("Thank you, dear. Remember, I'll be waiting for you before I go.") ||
            storyText.Contains("I need to get the new car that Papa gave to us.") ||
            storyText.Contains("You will no longer need to walk to school everyday.") ||
            storyText.Contains("Thank you, dear. Remember, I'll be waiting for you before I go.")
            )
            {
                youpic.gameObject.SetActive(false);
                mamapic.gameObject.SetActive(true);
                mamaname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
            }
            else if (storyText.Contains("Yay! Okay, mama. I'll go straight home!"))
            {
                youpic.gameObject.SetActive(true);
                mamapic.gameObject.SetActive(false);
                mamaname.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
            }
            else if (storyText.Contains("It's Friday, your bestfriend is still missing."))
            {
                youpic.gameObject.SetActive(false);
                mamapic.gameObject.SetActive(false);
                mamaname.gameObject.SetActive(false);
                youname.gameObject.SetActive(false);
            }
           
        }   
        else
        {
            ExitDialogueMode();
            you.gameObject.SetActive(true);

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

        // Hide the choice buttons after the dialogue is over
        foreach (GameObject choice in choices)
        {
            choice.gameObject.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        continueButton.gameObject.SetActive(currentChoices.Count == 0);
    }

    private void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
