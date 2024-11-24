using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class ClassrDial : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;  // Array for the 2 choice buttons
    private TextMeshProUGUI[] choicesText;

    [Header("Ink File")]
    [SerializeField] private TextAsset inkFile;

    [Header("Dialogue UI")]
    
    [SerializeField] private GameObject boyuna;
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youpic;
    [SerializeField] private GameObject youname;
    [SerializeField] private GameObject teacherpic;
    [SerializeField] private GameObject teachername;
    [SerializeField] private GameObject kidnakaupo;
    

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueStory);
        kidnakaupo.gameObject.SetActive(false);

        // Initialize the choices array
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            int choiceIndex = index;
            choice.GetComponent<Button>().onClick.AddListener(() => MakeChoice(choiceIndex));
            index++;
        }

        // Automatically start dialogue after 1-second delay
        StartCoroutine(StartDialogueAfterDelay(1f));
    }

    private IEnumerator StartDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnterDialogueMode();
    }

    private void EnterDialogueMode()
    {
        if (inkFile == null)
        {
            Debug.LogError("Ink file not found!");
            return;
        }

        currentStory = new Story(inkFile.text);
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
            if (storyText.Contains("Why are you late for class, Aris?")
            || storyText.Contains("Quiet, children. That's a wonderful news then.")
            || storyText.Contains("It's alright")
            )
            {
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(true);
                teachername.gameObject.SetActive(true);
            }

            else if(storyText.Contains("Today, we will talk about making the right choices.")
             || storyText.Contains("I'll be telling scenarios and you will tell me what you would do.")
             || storyText.Contains("Let's begin.")
             || storyText.Contains("A man in a van stop by and asked you.")
             || storyText.Contains(" 'Come in to my van, I have a lot of ice cream and candy at the back.' What would you do?")
             || storyText.Contains("No, you don't know him and that's not safe.") 
             || storyText.Contains("That's right!")
             || storyText.Contains("There's a woman who said that she is your mom's co-worker.")
             || storyText.Contains("'I'm your mom's friend and she ask me to pick you up.' What would you do?")
             || storyText.Contains("Are you sure? She can be lying.")
             || storyText.Contains("Yes, you can't be sure if she's telling the truth.")
             || storyText.Contains("If you neighbor said, come with me. There's an emergency!")
             || storyText.Contains("Your mom is at the hospital! She had an accident!")
             || storyText.Contains("Right, don't believe until it's proven.")
             || storyText.Contains("It's worrying, but we can't be sure.")
             || storyText.Contains("Alright, that's it for now.")
            )
                {
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(true);
                teachername.gameObject.SetActive(true);
                kidnakaupo.gameObject.SetActive(true);
                boyuna.gameObject.SetActive(false);
                }


            else if(storyText.Contains("Teddy had returned, he came back. He escaped from the bad guys.")
            )
                {
                you.gameObject.SetActive(true);
                youpic.gameObject.SetActive(true);
                teacherpic.gameObject.SetActive(false);
                teachername.gameObject.SetActive(false);
            }

            else if(storyText.Contains("The students began whispers and chatting.")
            )
                {
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(false);
                teachername.gameObject.SetActive(false);
                }

            if (storyText.Contains("That's right!")
            ||storyText.Contains("Yes, you can't be sure if she's telling the truth.")
            || storyText.Contains("Right, don't believe until it's proven.")
            ) 
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
            Debug.LogError("More choices than UI can support! Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // Display each choice text
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // Hide any unused choice buttons
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        // Show the continue button only if there are no choices
        continueButton.gameObject.SetActive(currentChoices.Count == 0);
    }

    private void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";  // Clear the text
        continueButton.gameObject.SetActive(false);

        // Hide the choice buttons after the dialogue is over
        foreach (GameObject choice in choices)
        {
            choice.gameObject.SetActive(false);
        }
    }
}
