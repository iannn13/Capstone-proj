using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class LolaDial : MonoBehaviour
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

    [Header("panel")]
    [SerializeField] private GameObject panel;

    [SerializeField] private GameObject lolaID;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject kid;
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject lola;
    [SerializeField] private GameObject lolapic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject lolaname;


    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static LolaDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static LolaDial GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueStory);
        panel.gameObject.SetActive(false);

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
            if (storyText.Contains("Iho, it's you. Come here.")
            )
            {
                kid.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(false);
                lola.gameObject.SetActive(true);
                lolapic.gameObject.SetActive(true);
                lolaname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
            }
            else if (storyText.Contains("Please Help me read the ID number here?")|| storyText.Contains("Thank you, Iho. Here's 25 peesos for you.") ||
            storyText.Contains("Haya, this kid."))         
            {
                kid.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(false);
                lola.gameObject.SetActive(true);
                lolapic.gameObject.SetActive(true);
                lolaname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
                lolaID.gameObject.SetActive(true);
            }
            else if (storyText.Contains("Thank you Lola."))           
            {


                kid.gameObject.SetActive(true);
                kidpic.gameObject.SetActive(true);
                lola.gameObject.SetActive(false);
                lolapic.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
                lolaname.gameObject.SetActive(false);
            }
        }   
        else
        {
            ExitDialogueMode();
            kid.gameObject.SetActive(true);
            panel.gameObject.SetActive(false);
            lolaID.gameObject.SetActive(false);
            lola.gameObject.SetActive(true);
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
