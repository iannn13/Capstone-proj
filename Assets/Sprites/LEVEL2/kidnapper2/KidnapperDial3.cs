using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KidnapperDial3 : MonoBehaviour
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

    [Header("Picture and name")]
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youpic;
    [SerializeField] private GameObject teddy;
    [SerializeField] private GameObject teddypic;
    [SerializeField] private GameObject kidnapper;
    [SerializeField] private GameObject kidnapperpic;

    [Header("teddy and kidnapper")]
    [SerializeField] private GameObject teddy1;
    [SerializeField] private GameObject kidnapper1;

    [Header("Points")]
    [SerializeField] private PointsManager PointsManager;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static KidnapperDial3 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static KidnapperDial3 GetInstance()
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

        OnDialogueComplete?.Invoke();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string storyText = currentStory.Continue();
            StopAllCoroutines();
            StartCoroutine(TypeText(storyText));
            Debug.Log("Current Story Text: " + storyText);
            DisplayChoices();

            if (storyText.Contains("Hey kids! Do you play computer games?") || storyText.Contains("I'm giving away free games passes worth 100 peesos!") || storyText.Contains("Yes, but you have to come with me to the game event near the woods!") || storyText.Contains("Don�t worry, kids! Almost everyone in this town are there!")
                || storyText.Contains("Yes, this game pass works in every gaming platform!") || storyText.Contains("You�ll miss out with the free rewards that you can get, if you don�t attend to the game event.") || storyText.Contains("What do you say?"))
            {
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                teddy.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
                kidnapper.gameObject.SetActive(true);
                kidnapperpic.gameObject.SetActive(true);
            }

           
            else if (storyText.Contains("...."))
            {
                ShowGameOver2();
            }
            else if (storyText.Contains("..."))
            {
                ShowGameOver1();
            }
            else if (storyText.Contains(".."))
            {
                ShowGameOver();
            }
            else if (storyText.Contains("I can't go. My mom will be worried.") || storyText.Contains("Because we don't know if this is real.") || storyText.Contains("You can go ahead... ")
                || storyText.Contains("I don't know about this, Teddy.") || storyText.Contains("I have to go, I feel sick."))
            {
                you.gameObject.SetActive(true);
                youpic.gameObject.SetActive(true);
                teddy.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
                kidnapper.gameObject.SetActive(false);
                kidnapperpic.gameObject.SetActive(false);
            }
            else if (storyText.Contains("The man and Teddy left you alone and they are on their way to the event.") || storyText.Contains("You should go home now."))
            {
                teddy1.SetActive(false);
                kidnapper1.SetActive(false);
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                teddy.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
                kidnapper.gameObject.SetActive(false);
                kidnapperpic.gameObject.SetActive(false);
            }

            else
            {
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                teddy.gameObject.SetActive(true);
                teddypic.gameObject.SetActive(true);
                kidnapper.gameObject.SetActive(false);
                kidnapperpic.gameObject.SetActive(false);
            }

             if (storyText.Contains("You should go home now.")) 
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
        }
        else
        {
            ExitDialogueMode();
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(16);
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
