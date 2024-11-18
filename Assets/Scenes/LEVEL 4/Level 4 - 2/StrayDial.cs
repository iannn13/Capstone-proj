using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class StrayDial : MonoBehaviour
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
    [SerializeField] private GameObject kid;
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject stray;
    [SerializeField] private GameObject straypic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject strayname;


    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static StrayDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static StrayDial GetInstance()
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

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);
        movebutton.gameObject.SetActive(true);

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
            if (storyText.Contains("What are you looking at?") || storyText.Contains("What are you talking about, I'm not Teddy.")
            || storyText.Contains("My name is Remy, people call me Smelly Remy because my stink won't come off.")
            || storyText.Contains("I get that a lot, but I'm not even from here.") || storyText.Contains("Some group of guys pick me up but I escaped from them.")
             || storyText.Contains("I haven't seen my Mama for weeks because I don't know where I live.")
             || storyText.Contains("It's fine, why don't you just do me a favor?") || storyText.Contains("Haysss.") || storyText.Contains("Thanks, bro.")
             
            )
            {
                kid.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(false);
                stray.gameObject.SetActive(true);
                straypic.gameObject.SetActive(true);
                strayname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
            }
            else if (storyText.Contains("Why are you talking to me?") || storyText.Contains("Sorry, I didn't mean-") || storyText.Contains("The smelly child looked very familar.") 
            || storyText.Contains("Teddy, you got to go home!") || storyText.Contains("Teddy, Is that you? You gotta go home!") || storyText.Contains("Sorry, I thought you are my friend.") 
            || storyText.Contains("Sorry, I thought you are my friend.")
            )            
            {
                kid.gameObject.SetActive(true);
                kidpic.gameObject.SetActive(true);
                stray.gameObject.SetActive(false);
                straypic.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
                strayname.gameObject.SetActive(false);
            }
        }   
        else
        {
            ExitDialogueMode();
            kid.gameObject.SetActive(true);
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
