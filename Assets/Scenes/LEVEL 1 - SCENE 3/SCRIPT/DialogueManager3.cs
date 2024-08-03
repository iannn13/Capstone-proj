using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager3 : MonoBehaviour
{
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

    [Header("Image")]
    [SerializeField] private Image kid;
    [SerializeField] private Image npc;
    [SerializeField] private Image pesos;
    [SerializeField] private Image pesobg;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f; // Speed of the typing effect

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager3 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager3 GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        kid.gameObject.SetActive(false);
        npc.gameObject.SetActive(true);  // Assuming tambay is the default image

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

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
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
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string storyText = currentStory.Continue();
            Debug.Log("Current Story Text: " + storyText); // Log the current story text
            DisplayChoices();

            // Check for specific text to toggle images and apply highlight
            bool highlightFivePeso = false;
            bool highlightWrong = false;
            if (storyText.Contains("5 Peso"))
            {
                highlightFivePeso = true;
                storyText = storyText.Replace("5 Peso", "<color=#FFFF00>5 Peso</color>");
                pesos.gameObject.SetActive(true);
                pesobg.gameObject.SetActive(true);
            }
            else
            {
                pesos.gameObject.SetActive(false);
                pesobg.gameObject.SetActive(false);
            }
            if (storyText.Contains("wrong"))
            {
                highlightWrong = true;
                storyText = storyText.Replace("wrong", "<color=#FFFF00>wrong</color>");
            }

            StopAllCoroutines(); // Stop any ongoing typing coroutine
            StartCoroutine(TypeText(storyText, highlightFivePeso, highlightWrong));
            Debug.Log("Current Story Text: " + storyText);
            // Check for specific conditions to toggle images
            if (storyText.Contains("Sorry, Lola. I'm going to school.") ||
                storyText.Contains("Sure, Lola. What is it?") ||
                storyText.Contains("I think it's this one?") ||
                storyText.Contains("Here it is, Lola."))
            {
                kid.gameObject.SetActive(true);
                npc.gameObject.SetActive(false);
            }
            else if (storyText.Contains("You might pickup the wrong peso but she didn't notice it."))
            {
                kid.gameObject.SetActive(false);
                npc.gameObject.SetActive(false);
            }
            else
            {
                kid.gameObject.SetActive(false);
                npc.gameObject.SetActive(true);
            }

            // Check the current story text for the game-over condition
            if (storyText.Contains("..."))
            {
                Debug.Log("Game over detected");
                ShowGameOver();
            }
        }
        else
        {
            ExitDialogueMode();

            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(5);
            }
            else
            {
                Debug.LogError("SceneTransitionManager not found in the scene!");
            }
        }
    }


    private IEnumerator TypeText(string text, bool highlightFivePeso, bool highlightWrong)
    {
        dialogueText.text = "";
        string originalText = text;

        if (highlightFivePeso)
        {
            // Remove color tags for typing animation
            originalText = originalText.Replace("<color=#FFFF00>", "").Replace("</color>", "");
        }

        if (highlightWrong)
        {
            // Remove color tags for typing animation
            originalText = originalText.Replace("<color=#FFFF00>", "").Replace("</color>", "");
        }

        foreach (char letter in originalText.ToCharArray())
        {
            dialogueText.text += letter;

            // Add delay for typing effect
            yield return new WaitForSeconds(typingSpeed);
        }

        // After typing, restore the original text with color tags if highlighting was applied
        if (highlightFivePeso)
        {
            dialogueText.text = text;
        }

        if (highlightWrong)
        {
            dialogueText.text = text;
        }
    }


    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More Choices were given than UI can support. Number of choices given: " + currentChoices.Count);
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
        Debug.Log("Choice made: " + choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void ShowGameOver()
    {
        Debug.Log("Restarting game");
        dialoguePanel.SetActive(false);
        gameOverPanel.SetActive(true);
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
