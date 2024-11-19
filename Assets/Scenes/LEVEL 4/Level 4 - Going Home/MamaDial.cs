using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class MamaDial : MonoBehaviour
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

    [Header("Dialogue UI")]
    [SerializeField] private GameObject kid;
    [SerializeField] private GameObject kidpic;
    [SerializeField] private GameObject mama;
    [SerializeField] private GameObject mamapic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject mamaname;


    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static MamaDial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static MamaDial GetInstance()
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
            if (storyText.Contains("Hello, son. How are you today?") || storyText.Contains("Ohh, you mean Pako? I talked to him too and he looks like a nice funny kid.") ||        
            storyText.Contains("Why, sweetie?") || storyText.Contains("Haha, I don't see it that way.") || storyText.Contains("Ooo, my son's a bit grumpy.") ||
            storyText.Contains("Alright, hahaha.") || storyText.Contains("Well, son. You can ignore him when he tries to talk to you next time.")
            || storyText.Contains("Oh, what happened next?") || storyText.Contains("Ohh... Poor little boy.")
                        )
            {
                kidpic.gameObject.SetActive(false);
               
                mamapic.gameObject.SetActive(true);
                mamaname.gameObject.SetActive(true);
                youname.gameObject.SetActive(false);
            }
           
            else if (storyText.Contains("Okay... I still don't like him.") || storyText.Contains("The way he talks is weird.") || storyText.Contains("Our new neighbor talked to me, he is being weird.")
            ||  storyText.Contains("He asks a lot of things about you, I'm annoyed.") || storyText.Contains("I'm serious, mama.")
            || storyText.Contains("I don't know, he is just strange.") || storyText.Contains("Okay, mama. That's a good idea.")
            || storyText.Contains("I met a kid that look like Teddy, but his name is actually Remy.") || storyText.Contains("He's a street kid and He says he haven't seen his mom for a long time.") 
            )            
            {
                kidpic.gameObject.SetActive(true);
                mamapic.gameObject.SetActive(false);
                youname.gameObject.SetActive(true);
                mamaname.gameObject.SetActive(false);
            }
            else if (storyText.Contains("..."))
            
            {
                SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
                if (transitionManager != null)
                {
                    transitionManager.FadeToScene(34);
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
