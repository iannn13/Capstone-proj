using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Burst.Intrinsics;
using UnityEngine.SceneManagement;

public class PoliceDial : MonoBehaviour
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

    [SerializeField] private GameObject kid;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject teddy;
    [SerializeField] private GameObject policewoman;

      [Header("Dialogue UI")]
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject policepic;
    [SerializeField] private GameObject teddypic;
    [SerializeField] private GameObject teddyname;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject policename;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static PoliceDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static PoliceDial GetInstance()
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

            if (storyText.Contains("You and Teddy went to the police station...") 
            )
            {
                youname.gameObject.SetActive(false);
                policename.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(false);
                policepic.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
            }
            //police
            else if (storyText.Contains("Other kids?") || storyText.Contains("What are you two doing here?")
            || storyText.Contains("Alright but we have to call your mother first.")
            || storyText.Contains("Let's wait inside.")
            || storyText.Contains("You, young man. You have to go home.")
            || storyText.Contains("Oh yeah, you do want me to take you there?")
            || storyText.Contains("Alright, It was just near here anyway.")
            )
            {
                youname.gameObject.SetActive(false);
                policename.gameObject.SetActive(true);
                teddyname.gameObject.SetActive(false);
                policepic.gameObject.SetActive(true);
                kidpic.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(false);
            }
            //teddy
            else if (storyText.Contains("Please, Help! There were other kids in the woods!") 
            || storyText.Contains("I'll see you around, Aris.")
            )
            {
                youname.gameObject.SetActive(false);
                policename.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(true);
                policepic.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(false);
                teddypic.gameObject.SetActive(true);
            }
            //you
            else if (storyText.Contains("Officer, that's Teddy Junzales!") 
            || storyText.Contains("I can't go home, I have school.") 
            )
            {
                youname.gameObject.SetActive(true);
                policename.gameObject.SetActive(false);
                teddyname.gameObject.SetActive(false);
                policepic.gameObject.SetActive(false);
                kidpic.gameObject.SetActive(true);
                teddypic.gameObject.SetActive(false);
            }


            else if (storyText.Contains("...")){
                {
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(42);
            }
            else
            {
                Debug.LogError("SceneTransitionManager not found in the scene!");
            }
            }
            }
             else if(storyText.Contains("..")
            ){
            ExitDialogueMode();
            kid.gameObject.SetActive(true);
            panel.gameObject.SetActive(false);
            police.gameObject.SetActive(false);
            teddy.gameObject.SetActive(false);
            policewoman .gameObject.SetActive(false);

            }
            

        }   
        else
        {
            ExitDialogueMode();
            kid.gameObject.SetActive(true);
            panel.gameObject.SetActive(false);
            police.gameObject.SetActive(false);
            teddy.gameObject.SetActive(false);
            policewoman.gameObject.SetActive(false);
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
}
