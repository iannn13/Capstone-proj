using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Burst.Intrinsics;
using UnityEngine.SceneManagement;

public class teddydial5 : MonoBehaviour
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
    [SerializeField] private GameObject teddy;
    [SerializeField] private GameObject teddypic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject teddyname;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static teddydial5 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static teddydial5 GetInstance()
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
            if (storyText.Contains("Aris! Psst! Over here!")||storyText.Contains("It's me, Teddy. You have to come with me!")
            || storyText.Contains("We have to go back to the woods.") || storyText.Contains("I can't! Not yet!")
            || storyText.Contains("Because... There were other kids in the woods too.")
            || storyText.Contains("We have to save them.") ||
            storyText.Contains("Okay!") 
            )

            {
                kidpic.gameObject.SetActive(false);
                teddy.gameObject.SetActive(true);
                teddypic.gameObject.SetActive(true);
                teddyname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
            }

            else if (storyText.Contains("Why? What are we gonna do?")||
            storyText.Contains("No! Why do you want to go back there?") ||
            storyText.Contains("Let's go and save them!") ||
            storyText.Contains("We're gonna be like superheroes!") || 
            storyText.Contains("Alright, but we have to ask help to an adult.")
            ||storyText.Contains("We have to ask help to the police.") 
            ||storyText.Contains("I'm taking you to the police.") 
            ||storyText.Contains("You pulled Teddy's hand but let go.") 
            ||storyText.Contains("What's wrong with you?") 
            ||storyText.Contains("Okay.") 
            )         
            {
                kidpic.gameObject.SetActive(true);
                teddy.gameObject.SetActive(true);
                teddypic.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
                
            }
            else if (storyText.Contains("...")){
                gameOverPanel.gameObject.SetActive(true);
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
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(36);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
