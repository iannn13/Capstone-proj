using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class AsniDialogue : MonoBehaviour
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

    [Header("Dialogue UI")]
    [SerializeField] private GameObject hiro;
    [SerializeField] private GameObject hiropic;
    [SerializeField] private GameObject asni;
    [SerializeField] private GameObject asnipic;

    [Header("Poster")]
    [SerializeField] private GameObject poster;

    [Header("Poster Button")]
    [SerializeField] private Button posterButton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static AsniDialogue instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static AsniDialogue GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        poster.gameObject.SetActive(false) ;
        posterButton.gameObject.SetActive(false);
        posterButton.onClick.AddListener(HidePoster);
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

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);
        poster.gameObject.SetActive(true);
        poster.gameObject.SetActive(true); // Show poster at end of dialogue
        posterButton.gameObject.SetActive(true);

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
            if (storyText.Contains("Meow!"))
            {
                hiro.gameObject.SetActive(true);
                hiropic.gameObject.SetActive(true);
                asni.gameObject.SetActive(false);
                asnipic.gameObject.SetActive(false);
            }
            else
            {
                hiro.gameObject.SetActive(false);
                hiropic.gameObject.SetActive(false);
                asni.gameObject.SetActive(true);
                asnipic.gameObject.SetActive(true);
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

    private void HidePoster()
    {
        poster.gameObject.SetActive(false);
        posterButton.gameObject.SetActive(false);
    }
}
