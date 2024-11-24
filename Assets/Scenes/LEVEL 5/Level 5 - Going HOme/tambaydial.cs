using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Burst.Intrinsics;
using UnityEngine.SceneManagement;

public class tambaydial : MonoBehaviour
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


    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage; 
    [SerializeField] private float fadeDuration = 1f;

    [Header("panel")]
    [SerializeField] private GameObject panel;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    public FadeManager fadeManager;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject kid;
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject tambay;
    [SerializeField] private GameObject tambaypic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject tambayname;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static tambaydial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static tambaydial GetInstance()
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
        private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string storyText = currentStory.Continue();
            StopAllCoroutines();
            StartCoroutine(TypeText(storyText));
            Debug.Log("Current Story Text: " + storyText); // Log the current story text
            DisplayChoices();

            // tambay
            if (storyText.Contains("Hey, Aris! I saw your mom left earlier.")
            || storyText.Contains("She went on a birthday party with your baby sibling, too bad she didn't wait for you.")
            || storyText.Contains("She went on a birthday party, she asked me to pick you up.")
            || storyText.Contains("I'm not kidding she left 30 minutes ago.")
            || storyText.Contains("She asked me to pick you up.")
            )   
            {
                tambaypic.gameObject.SetActive(true);
                tambayname.gameObject.SetActive(true);
                kidpic.gameObject.SetActive(false);
                youname.gameObject.SetActive(false);
            }
            // you
            else if (storyText.Contains("Okay, I'm going home now.")
            || storyText.Contains("It's alright, you are just messing with me.")
            || storyText.Contains("I don't believe you, my Mama says she's waiting for me at home.")
            || storyText.Contains("You are lying! She's home and I know it!")
            )
            {
                tambaypic.gameObject.SetActive(false);
                tambayname.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(true);
                youname.gameObject.SetActive(true);
            }
            else if (storyText.Contains("...."))
            {
            Debug.Log("Game over detected (4 dots)");
                ShowGameOver();
            }
            else if (storyText.Contains("..."))
            {
            Debug.Log("Game over detected (3 dots)");
            ShowGameOver1();
            }
            else if (storyText.Contains(".."))
            {
            Debug.Log("Game over detected (2 dots)");
            ShowGameOver2();
            }

        }   
        else
        {
            ExitDialogueMode();
            kid.gameObject.SetActive(true);
            panel.gameObject.SetActive(false);
            
        }
    }
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        OnDialogueComplete?.Invoke();
    }

    
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f; // Start fully transparent
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true); // Ensure the fadeImage is active

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // Start fully opaque
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // Hide the fadeImage after fading in
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (FadeManager.Instance != null)
        {
            yield return FadeManager.Instance.FadeOut(() => SceneManager.LoadSceneAsync(20));
        }
        else
        {
            Debug.LogError("FadeManager instance is null!");
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
