using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Burst.Intrinsics;
using UnityEngine.SceneManagement;
using UnityEditor.VersionControl;

public class C2Dial : MonoBehaviour
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

    [Header("Special GameObject")]
    [SerializeField] private GameObject specialObject;

    [Header("panel")]
    [SerializeField] private GameObject panel;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Player Movement")]
    public RightMove rightMove;
    public RightMovePlayer2 player2move;
    public GameObject Player;
    public GameObject Player2;

    [Header("box")]
    [SerializeField] private GameObject box1;
    [SerializeField] private GameObject box2;

    public FadeManager fadeManager;

    [Header("name and pic")]
    [SerializeField] private GameObject man;
    [SerializeField] private GameObject manpic;
    [SerializeField] private GameObject alice;
    [SerializeField] private GameObject alicepic;
    [SerializeField] private GameObject owen;
    [SerializeField] private GameObject owenpic;
    [SerializeField] private GameObject you;
    [SerializeField] private GameObject youpic;
    [SerializeField] private GameObject warsak;
    [SerializeField] private GameObject warsakpic;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static C2Dial instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static C2Dial GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        man.SetActive(false);
        manpic.SetActive(false);
        alice.SetActive(false);
        alicepic.SetActive(false);
        owen.SetActive(false);
        owenpic.SetActive(false);
        you.SetActive(false);
        youpic.SetActive(false);
        warsak.SetActive(false);
        warsakpic.SetActive(false);
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
            Debug.Log("Current Story Text: " + storyText);
            DisplayChoices();


            if (storyText.Contains("The masked man scoffs and you pulled Alice to the Sand box."))
            {
                Vector3 targetPosition = new Vector3(290.47f, Player.transform.position.y, Player.transform.position.z);
                rightMove.SetTargetPosition(targetPosition);
                Vector3 targetPosition2 = new Vector3(289f, Player2.transform.position.y, Player2.transform.position.z);
                player2move.SetTargetPosition(targetPosition2);
                box2.gameObject.SetActive(true);
                box1.gameObject.SetActive(false);
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);
            }
            else if (storyText.Contains("The masked man looked nervous and tried to take Alice by force!") || (storyText.Contains("You got the attention of several civilians and he left the scene."))
                || (storyText.Contains("You and Alice played for 30 minutes and a familiar face arrived.")) || (storyText.Contains("You notice the masked man was no where to found anymore."))
                || (storyText.Contains("The two said their goodbyes and left.")) || (storyText.Contains("The man pushed you away and let go of her arm, He took her."))
                || (storyText.Contains("He is lying, Ask another question.")) || (storyText.Contains("The two said their goodbyes and left.")))
            {
                box2.gameObject.SetActive(true);
                box1.gameObject.SetActive(false); 
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);
            }
            else if (storyText.Contains("Little Girl, I'm your dad's friend. He called me to pick you up.") || (storyText.Contains("What? Does your dad know this boy?"))
                || (storyText.Contains("I don't know it yet, I'm just picking her up to get to know her.")))
            {
                box1.gameObject.SetActive(true);
                box2.gameObject.SetActive(false);
                man.SetActive(true);
                manpic.SetActive(true);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);
            }
            else if (storyText.Contains("I don't know you, Mister.") || (storyText.Contains("Who is he?")) || (storyText.Contains("Why?")) || (storyText.Contains("What if he is really my dad's friend?"))
                || (storyText.Contains("That's weird.")) || (storyText.Contains("Yes, he is my cousin.")) || (storyText.Contains("Okay.")) || (storyText.Contains("Okay.")) || (storyText.Contains("Hello, dad. We are playing sand castle."))
                || (storyText.Contains("Is he your friend, Dad?")) || (storyText.Contains("I love to!")) || (storyText.Contains("I think my dad is waiting for me at home.")))
            {
                box1.gameObject.SetActive(true);
                box2.gameObject.SetActive(false);
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(true);
                alicepic.SetActive(true);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);
            }
            else if (storyText.Contains("Oh my! What happen?") || (storyText.Contains("Uncle, There's a suspicious guy who came to pick up Alice. He says that he is your friend."))
                || (storyText.Contains("I'm glad both of you are okay.")) || (storyText.Contains("Are you walking home alone? We can give you a ride."))
                || (storyText.Contains("No, I should bring you home and tell your mom about this."))  || (storyText.Contains("Hello, Aris."))
                || (storyText.Contains("No, Alice. I'm glad you stayed here. Thank you for telling, Aris.")))
            {
                box1.gameObject.SetActive(true);
                box2.gameObject.SetActive(false);
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(true);
                owenpic.SetActive(true);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);
            }
            else if (storyText.Contains("Leave them alone!") || (storyText.Contains("Someone call the police!")) || (storyText.Contains("Are you okay?")))
            {
                box1.gameObject.SetActive(true);
                box2.gameObject.SetActive(false);
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(true);
                warsakpic.SetActive(true);
            }
            else if (storyText.Contains("I'm taking her!"))
            {
                box1.gameObject.SetActive(true);
                box2.gameObject.SetActive(false);
                man.SetActive(true);
                manpic.SetActive(true);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);
                ShowGameOver();
            }
           else if (storyText.Contains("Hi sweetie, It's time to go home."))
            {
                FadeManager.Instance.FadeOut(() => Debug.Log("FadeOut Complete!"));
                specialObject.SetActive(true);
                box1.gameObject.SetActive(true);
                box2.gameObject.SetActive(false);
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(true);
                owenpic.SetActive(true);
                you.SetActive(false);
                youpic.SetActive(false);
                warsak.SetActive(false);
                warsakpic.SetActive(false);

            }
            else
            {
                box1.gameObject.SetActive(true);
                man.SetActive(false);
                manpic.SetActive(false);
                alice.SetActive(false);
                alicepic.SetActive(false);
                owen.SetActive(false);
                owenpic.SetActive(false);
                you.SetActive(true);
                youpic.SetActive(true);

            }
        }
        else
        {
            ExitDialogueMode();
            panel.gameObject.SetActive(false);
        }
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
