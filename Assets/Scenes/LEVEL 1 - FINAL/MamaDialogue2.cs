using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class MamaDialogue2 : MonoBehaviour
{
    public delegate void DialogueCompleteHandler();
    public event DialogueCompleteHandler OnDialogueComplete;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Name")]
    [SerializeField] private GameObject mama;
    [SerializeField] private GameObject you;

    [Header("Picture")]
    [SerializeField] private GameObject mamapic;
    [SerializeField] private GameObject youpic;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static MamaDialogue2 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Mama Dialogue in the scene");
        }
        instance = this;
    }

    public static MamaDialogue2 GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueStory);

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

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        continueButton.gameObject.SetActive(true);

        if (movebutton != null)
        {
            movebutton.SetActive(false); // Hide the move button
        }

        // Simulate button click if needed (e.g., reset its state)
        Button moveButtonComponent = movebutton.GetComponent<Button>();
        if (moveButtonComponent != null)
        {
            // Reset button state if needed (not usually necessary)
            // moveButtonComponent.onClick.Invoke(); // Optional: Simulate click
        }

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);

        // Re-enable the move button after dialogue
        if (movebutton != null)
        {
            movebutton.SetActive(true); // Show the move button
        }


        OnDialogueComplete?.Invoke();
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
            if (storyText.Contains("It's okay, mama. I'm hungry.") || 
                storyText.Contains("I don't feel safe going home.") ||
                storyText.Contains("It was great, mama! I'm excited for tomorrow!") ||
                storyText.Contains("Me and my bestfriend are going to play after school!") ||
                storyText.Contains("Thank you, mama. I'm hungry now."))
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
