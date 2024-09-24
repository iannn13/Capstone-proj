using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class clasrom : MonoBehaviour
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
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youpic;
    [SerializeField] private GameObject pinang;
    [SerializeField] private GameObject pinangpic;
    [SerializeField] private GameObject policeman;
    [SerializeField] private GameObject policemanpic;
    [SerializeField] private GameObject policewoman;
    [SerializeField] private GameObject policewomanpic;
    [SerializeField] private GameObject teacher;
    [SerializeField] private GameObject teacherpic;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueStory);

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
            if (storyText.Contains("Yes, he was! We saw them."))
            {
                pinang.gameObject.SetActive(true);
                pinangpic.gameObject.SetActive(true);
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                policeman.gameObject.SetActive(false);
                policemanpic.gameObject.SetActive(false);
                policewoman.gameObject.SetActive(false);
                policewomanpic.gameObject.SetActive(false);
                teacher.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(false);


            }
            else if (storyText.Contains("Hello, students. Based on our investigation, one of you has been with Teddy Junzales where he was last seen.") || storyText.Contains("Boy with thick hair, were you with him yesterday?")
                || storyText.Contains("Game event? There wasn’t a game event permitted to us yesterday."))
            {
                pinang.gameObject.SetActive(false);
                pinangpic.gameObject.SetActive(false);
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                policeman.gameObject.SetActive(true);
                policemanpic.gameObject.SetActive(true);
                policewoman.gameObject.SetActive(false);
                policewomanpic.gameObject.SetActive(false);
                teacher.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(false);
            }
            else if (storyText.Contains("Good morning students. The police officers here to help us find our missing classmate.") ||
               storyText.Contains("Now, kids. Here’s a lesson for all of you.") ||
               storyText.Contains("Never believe or talk to any strangers, okay.") ||
               storyText.Contains("If you ever feel unsafe, you can call your parents on the phone or approach to police officers and teachers.")
               || storyText.Contains("Let’s all hope that our dear Teddy will be found soon.") || storyText.Contains("Class dismissed. Good bye everyone. Be safe!"))
            {
                pinang.gameObject.SetActive(false);
                pinangpic.gameObject.SetActive(false);
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                policeman.gameObject.SetActive(false);
                policemanpic.gameObject.SetActive(false);
                policewoman.gameObject.SetActive(false);
                policewomanpic.gameObject.SetActive(false);
                teacher.gameObject.SetActive(true);
                teacherpic.gameObject.SetActive(true);
            }
            else if (storyText.Contains("I was with him yesterday after school!") || storyText.Contains("Yes, I was.")
                || storyText.Contains("He went to a town’s game event with a tall man because he said he is giving away game passes.")
                || storyText.Contains("Near Kompyuter Shop at noon.")
                || storyText.Contains("Near Tusok-tusok at noon.")
                || storyText.Contains("I don’t remember the time and I think it’s across the street."))
            {
                pinang.gameObject.SetActive(false);
                pinangpic.gameObject.SetActive(false);
                you.gameObject.SetActive(true);
                youpic.gameObject.SetActive(true);
                policeman.gameObject.SetActive(false);
                policemanpic.gameObject.SetActive(false);
                policewoman.gameObject.SetActive(false);
                policewomanpic.gameObject.SetActive(false);
                teacher.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(false);
            }
            else if (storyText.Contains("The police officers thanked Teacher and left the room."))
                {
                pinang.gameObject.SetActive(false);
                pinangpic.gameObject.SetActive(false);
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                policeman.gameObject.SetActive(false);
                policemanpic.gameObject.SetActive(false);
                policewoman.gameObject.SetActive(false);
                policewomanpic.gameObject.SetActive(false);
                teacher.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(false);
            }
            else
            {
                pinang.gameObject.SetActive(false);
                pinangpic.gameObject.SetActive(false);
                you.gameObject.SetActive(false);
                youpic.gameObject.SetActive(false);
                policeman.gameObject.SetActive(false);
                policemanpic.gameObject.SetActive(false);
                policewoman.gameObject.SetActive(true);
                policewomanpic.gameObject.SetActive(true);
                teacher.gameObject.SetActive(false);
                teacherpic.gameObject.SetActive(false);
            }
        }
        else
        {
            ExitDialogueMode();
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.FadeToScene(23);
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
