using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class BystanderDial : MonoBehaviour
{
    public delegate void DialogueCompleteHandler();
    public event DialogueCompleteHandler OnDialogueComplete;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("panel")]
    [SerializeField] private GameObject panel;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;


    [Header("Dialogue UI")]
    [SerializeField] private GameObject kid;
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject npcpic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject bystandername;


    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static BystanderDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static BystanderDial GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
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
        panel.gameObject.SetActive(false);
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
            if (storyText.Contains("Hey, kid. How are you today?") || storyText.Contains("I'm just your neighbor, Pako. Just being friendly.") ||
            storyText.Contains("Nothing, I just talk to everyone passing by. How is your mom?") || 
            storyText.Contains("Alright, bye!")|| storyText.Contains("I see, good to know.") || storyText.Contains("That's great, how about your mom? Is she busy?")
            || storyText.Contains("Okay, nice to meet you. Bye.")   || storyText.Contains(" Good to know, bye!") || storyText.Contains("Good to know, bye!") ||
            storyText.Contains("Good to know, bye!") ||  storyText.Contains("That's great. I should talk to her too sometimes.") ||
            storyText.Contains("Oh okay, haha."))
            {
                kid.gameObject.SetActive(true);
                kidpic.gameObject.SetActive(false);
                npc.gameObject.SetActive(true);
                npcpic.gameObject.SetActive(true);
                bystandername.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
            }
            else if (storyText.Contains("Why are you talking to me?") || storyText.Contains("Bye, I gotta go to school.")
            || storyText.Contains("She's busy, doing a lot of stuff.") || storyText.Contains("I'm okay.") || storyText.Contains("Bye!")
            || storyText.Contains("She is okay. I gotta go now.") || storyText.Contains("No, I guess.") || storyText.Contains("I don't know.") )            
            {
                kid.gameObject.SetActive(true);
                kidpic.gameObject.SetActive(true);
                npc.gameObject.SetActive(false);
                npcpic.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
                bystandername.gameObject.SetActive(false);
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
