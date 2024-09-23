using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.XR;

public class IanDialogue : MonoBehaviour
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

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("name and pic")]
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject teddy;
    [SerializeField] private GameObject teddyname;
    [SerializeField] private GameObject ian;
    [SerializeField] private GameObject ianname;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static IanDialogue instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static IanDialogue GetInstance()
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
                if (storyText.Contains("I think he has a point."))
                {
                    you.gameObject.SetActive(true);
                    youname.gameObject.SetActive(true);
                    teddy.gameObject.SetActive(false);
                    teddyname.gameObject.SetActive(false);
                    ian.gameObject.SetActive(false);
                    ianname.gameObject.SetActive(false);
                }
                else if (storyText.Contains("I can beat those guys up!") || storyText.Contains("I agree, but i can beat those guys up!")
                    || storyText.Contains("You're just scaring us big bro.") || storyText.Contains("No Problem, Aris!") 
                    || storyText.Contains("Big bro Ian, Can we get two sticks of egg waffles?"))
                {
                    teddy.gameObject.SetActive(true);
                    teddyname.gameObject.SetActive(true);
                    you.gameObject.SetActive(false);
                    youname.gameObject.SetActive(false); 
                    ian.gameObject.SetActive(false);
                    ianname.gameObject.SetActive(false);
                }
                else if (storyText.Contains("Your friend paid for your snack. The vendor hand out the egg waffles."))
                {
                    teddy.gameObject.SetActive(false);
                    teddyname.gameObject.SetActive(false);
                    you.gameObject.SetActive(false);
                    youname.gameObject.SetActive(false);
                    ian.gameObject.SetActive(false);
                    ianname.gameObject.SetActive(false);
                }
                else
                {
                    ian.gameObject.SetActive(true);
                    ianname.gameObject.SetActive(true);
                    you.gameObject.SetActive(false);
                    youname.gameObject.SetActive(false);
                    teddy.gameObject.SetActive(false);
                    teddyname.gameObject.SetActive(false);
                }
            }
        
        else
        {
            ExitDialogueMode();
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(15);
            }
            else
            {
                Debug.LogError("SceneTransitionManager not found in the scene!");
            }
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
