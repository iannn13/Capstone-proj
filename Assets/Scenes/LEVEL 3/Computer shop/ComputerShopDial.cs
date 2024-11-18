using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComputerShopDial : MonoBehaviour
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

     [Header("PLAY PANEL")]
    [SerializeField] private GameObject PlayPanel;
    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static ComputerShopDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static ComputerShopDial GetInstance()
    {
        return instance;
    }

   private void Start()
{
    Debug.Log("Start ");
    dialogueIsPlaying = false;
    dialoguePanel.SetActive(false);
    continueButton.gameObject.SetActive(false);
    PlayPanel.gameObject.SetActive(false);

    continueButton.onClick.AddListener(ContinueStory);

    // Initialize choices text
   choicesText = new TextMeshProUGUI[choices.Length];
for (int index = 0; index < choices.Length; index++)
{
    choicesText[index] = choices[index].GetComponentInChildren<TextMeshProUGUI>();

    // Clear existing listeners and add new ones
    choices[index].GetComponent<Button>().onClick.RemoveAllListeners();

    int choiceIndex = index; // Cache index to avoid closure issues
    choices[index].GetComponent<Button>().onClick.AddListener(() =>
    {
        Debug.Log($"Choice button {choiceIndex} clicked");
        MakeChoice(choiceIndex);
    });
}
    // Start dialogue after scene loads
    if (inkJSON != null)
    {
        Debug.Log("Starting dialogue after scene load");
        EnterDialogueMode(inkJSON);
    }
    else
    {
        Debug.LogError("Ink JSON is not assigned in the Inspector!");
    }
}

    public void EnterDialogueMode(TextAsset inkJSON)
{
    Debug.Log("Entering Dialogue Mode");
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


           if (storyText.Contains("..."))
        {
            PlayPanel.gameObject.SetActive(true);
            StartCoroutine(LoadSceneAfterDelay("YourNextSceneName", 1f)); // 1-second delay
        }
    }
    else
    {
        ExitDialogueMode();
    }
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
{
    yield return new WaitForSeconds(delay);
    SceneManager.LoadScene(27);
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
//private void LoadNextScene()
//{
  //  // Use SceneManager to load the next scene by name or index
    //string nextSceneName = "NextScene"; // Replace with your next scene's name
   // SceneManager.LoadScene(nextSceneName);

    // Alternatively, load the next scene by build index
    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//}
}
