using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SchoolTime : MonoBehaviour
{
    public TextAsset inkJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f; 
    [Header("Points")]
    [SerializeField] private PointsManager PointsManager;
    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static SchoolTime instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static SchoolTime GetInstance()
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
            StopAllCoroutines();
            StartCoroutine(TypeText(storyText));
            Debug.Log("Current Story Text: " + storyText); // Log the current story text
            DisplayChoices();


            if (storyText.Contains("Correct"))
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
            // Check for specific text to toggle images
            if (storyText.Contains("He will come back soon. It won't be that long.") ||
                storyText.Contains("Canada"))
            {
            }
            else
            {
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
                transitionManager.FadeToScene(29);
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
