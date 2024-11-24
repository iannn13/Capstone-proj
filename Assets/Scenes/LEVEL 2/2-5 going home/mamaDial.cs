using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class mamaDial : MonoBehaviour
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

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Name and Pic")]
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject mama;
    [SerializeField] private GameObject mamaname;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static mamaDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static mamaDial GetInstance()
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
                if (storyText.Contains("Aris!") || storyText.Contains("You had me worried all day why you did'nt wake me up?") || storyText.Contains("You have to wake me up, Aris. I could’ve cook you breakfast.")
                    || storyText.Contains("It’s okay, son. Let’s go inside and eat.") || storyText.Contains("You have to wake me up, Aris. No matter what. I need to know when do you left, okay?"))
                {
                    you.gameObject.SetActive(false);
                    youname.gameObject.SetActive(false);
                    mama.gameObject.SetActive(true);
                    mamaname.gameObject.SetActive(true);
                }
                else if (storyText.Contains("I don’t want to wake you up mama, because you are tired taking care of baby all night long.")
                || storyText.Contains("I’m sorry, Mama. I won’t do it again.")
                || storyText.Contains("I was hungry, mama. I don’t want to disturb you and I went to go get something to eat.")
                || storyText.Contains("I’m sorry, Mama. I won’t do it again.") 
                || storyText.Contains("I’m sorry, Mama."))
                {
                    you.gameObject.SetActive(true);
                    youname.gameObject.SetActive(true);
                    mama.gameObject.SetActive(false);
                    mamaname.gameObject.SetActive(false);
                }         
        }
        else
        {
            ExitDialogueMode();
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(18);
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
