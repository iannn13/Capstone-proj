using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Burst.Intrinsics;
using UnityEngine.SceneManagement;

public class C2DanDial : MonoBehaviour
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

    [Header("Audio")]
    [SerializeField] private AudioSource snowBarkAudioSource;


    [Header("Player Movement")]
    public RightMove rightMove;
    public RightMovePlayer2 player2move;
    public GameObject Player;
    public GameObject Player2;

    public FadeManager fadeManager;

      [Header("Dialogue UI")]
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject danpic;
    [SerializeField] private GameObject teddypic;
    [SerializeField] private GameObject teddyname;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject danname;
    [SerializeField] private GameObject SnowPic;
    [SerializeField] private GameObject SnowName
    ;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static C2DanDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static C2DanDial GetInstance()
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
            if (storyText.Contains("Miss, please help us.")
            || storyText.Contains("I know her! She's always outside guarding.")|| storyText.Contains("Hello, Snow. I think we are neighbors!")
            )          
            {
                kidpic.gameObject.SetActive(true);
                danpic.gameObject.SetActive(false);
                danname.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
                teddypic.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(false);
                SnowPic.gameObject.SetActive(false);
                SnowName.gameObject.SetActive(false);
                
            }

            else if (storyText.Contains("What can I help?")||storyText.Contains("Wait, you're the missing kid! Are you okay?")
            || storyText.Contains("Okay, I know I have your flyer here,")
            || storyText.Contains("Please stay here with me while we wait for them.")
            || storyText.Contains("The lady took her phone out and dial Teddy's mom's phone number.")
            || storyText.Contains("Allow me to introduce myself, I'm Dan and this is my dog name Snow.")
            || storyText.Contains("We just live in the suburbs here, I'm taking her to a walk right now.")
            || storyText.Contains("Yes, because she always waits for me outside when I'm away.")
            )
            {
                kidpic.gameObject.SetActive(false);
                danpic.gameObject.SetActive(true);
                danname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(false);
                SnowPic.gameObject.SetActive(false);
                SnowName.gameObject.SetActive(false);
            }

            else if (storyText.Contains("I'm okay! I want to see my Mama, can you please call her?")
            )
            {
                kidpic.gameObject.SetActive(false);
                danpic.gameObject.SetActive(false);
                danname.gameObject.SetActive(false);
                youname.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(true);
                teddyname.gameObject.SetActive(true);
                SnowPic.gameObject.SetActive(false);
                SnowName.gameObject.SetActive(false);

            }

            else if (storyText.Contains("Woof!")
            )
            {
                kidpic.gameObject.SetActive(false);
                danpic.gameObject.SetActive(false);
                danname.gameObject.SetActive(false);
                youname.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(false);
                SnowPic.gameObject.SetActive(true);
                SnowName.gameObject.SetActive(true);

            if (snowBarkAudioSource != null)
            {
                snowBarkAudioSource.Play();
            }
            else
            {
                Debug.LogError("No audio source assigned for Snow's bark sound!");
            }

            }
            else if (storyText.Contains("...")){
                gameOverPanel.gameObject.SetActive(true);
            }

            else if(storyText.Contains("..")
            ){
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(39);
            }
            else
            {
                Debug.LogError("SceneTransitionManager not found in the scene!");
            }
            }
        }   
        else
        {
            ExitDialogueMode();
            panel.gameObject.SetActive(false);
            
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
