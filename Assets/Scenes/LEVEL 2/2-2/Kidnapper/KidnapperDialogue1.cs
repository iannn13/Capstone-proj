using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KidnapperDialogue1 : MonoBehaviour
{
    public delegate void DialogueCompleteHandler();
    public event DialogueCompleteHandler OnDialogueComplete;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject title1;
    [SerializeField] private GameObject title2;
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject message1;
    [SerializeField] private GameObject message2;


    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Picture")]
    [SerializeField] private GameObject kidnapper;
    [SerializeField] private GameObject kid;

    [Header("Name")]
    [SerializeField] private GameObject kidnapper1;
    [SerializeField] private GameObject you;


    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;


    [Header("Points")]
    [SerializeField] private PointsManager PointsManager;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static KidnapperDialogue1 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static KidnapperDialogue1 GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        kidnapper.gameObject.SetActive(false);
        kid.gameObject.SetActive(false);
        kidnapper1.gameObject.SetActive(false);
        you.gameObject.SetActive(false);

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

        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
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
            if (storyText.Contains("Hey kid! Have you seen a small fluffy dog around?") || storyText.Contains("Can you help me look for it?") || 
            storyText.Contains("But why? I thought you saw it. You will be a big help for me to find my pet.") ||
            storyText.Contains ("If you don't help me, something bad might happen to my dog.") ||
            storyText.Contains ("Ok, at least help me with other kids. Come, let's ask them together")
            )
            {
                kidnapper.gameObject.SetActive(true);
                kidnapper1.gameObject.SetActive(true);
                you.gameObject.SetActive(false);
                kid.gameObject.SetActive(false);
            }
            else if (storyText.Contains("No, I haven't seen that kind of dog.") || storyText.Contains("Sorry. I can't.") || storyText.Contains("Sorry. I can't.") || storyText.Contains("I'm going to school sir.")
                || storyText.Contains("I'm going to school sir.") || storyText.Contains("Sorry, I'm gonna be late.") || storyText.Contains("I can't help you sir. I told you so."))
            {
                kidnapper.gameObject.SetActive(false);
                kidnapper1.gameObject.SetActive(false);
                you.gameObject.SetActive(true);
                kid.gameObject.SetActive(true);
            }

            if (storyText.Contains("The shady man sighed and gave up on you.")||storyText.Contains("I can't help you sir. I told you so.")) 
            {
                Debug.Log("Adding Points");
                if (PointsManager.Instance != null)
                {
                    PointsManager.Instance.AddPoints(10);
                    Debug.Log("Added");
                }
                else
                {
                    Debug.LogError("PointsManager not assigned!");
                }
            }
            else if (storyText.Contains("...."))
            {
                Debug.Log("Game over detected (4 dots)");
                ShowGameOver2();
            }
            else if (storyText.Contains("..."))
            {
                Debug.Log("Game over detected (3 dots)");
                ShowGameOver1();
            }
            else if (storyText.Contains(".."))
            {
                Debug.Log("Game over detected (2 dots)");
                ShowGameOver();
            }
            else if (storyText.Contains("The shady man sighed and gave up on you."))
            {
                kidnapper.gameObject.SetActive(false);
                kidnapper1.gameObject.SetActive(false);
                you.gameObject.SetActive(false);
                kid.gameObject.SetActive(false);
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

    private void ShowGameOver()
    {
        Debug.Log("Restarting game");
        dialoguePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        title.gameObject.SetActive(true);
        message.gameObject.SetActive(true);
        title1.gameObject.SetActive(false);
        message1.gameObject.SetActive(false);
        title2.gameObject.SetActive(false);
        message2.gameObject.SetActive(false);

    }

    private void ShowGameOver1()
    {
        Debug.Log("Restarting game");
        dialoguePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        title1.gameObject.SetActive(true);
        message1.gameObject.SetActive(true);
        title.gameObject.SetActive(false);
        message.gameObject.SetActive(false);
        title2.gameObject.SetActive(false);
        message2.gameObject.SetActive(false);
    }

    private void ShowGameOver2()
    {
        Debug.Log("Restarting game");
        dialoguePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        title2.gameObject.SetActive(true);
        message2.gameObject.SetActive(true);
        title.gameObject.SetActive(false);
        message.gameObject.SetActive(false);
        title1.gameObject.SetActive(false);
        message1.gameObject.SetActive(false);  
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
