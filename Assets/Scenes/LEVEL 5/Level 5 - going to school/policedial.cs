using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Burst.Intrinsics;
using UnityEngine.SceneManagement;

public class policedial : MonoBehaviour
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
    [SerializeField] private GameObject policepic;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject policename;
    


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static policedial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static policedial GetInstance()
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

            // Check for specific text to toggle images
            if (storyText.Contains("Thank you, Officer!")
            )          
            {
                kidpic.gameObject.SetActive(true);
                youname.gameObject.SetActive(true);
                policepic.gameObject.SetActive(false);
                policename.gameObject.SetActive(false);


            }

            else if (storyText.Contains("No problem young man.") || storyText.Contains("Have fun at school")
            )
            {
                kidpic.gameObject.SetActive(false);
                youname.gameObject.SetActive(false);
                policepic.gameObject.SetActive(true);
                policename.gameObject.SetActive(true);

            }
        }   
        else
        {
            ExitDialogueMode();
            panel.gameObject.SetActive(false);
            police.gameObject.SetActive(false);
            
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


    private void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}