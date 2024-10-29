using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class NellyDialogue : MonoBehaviour
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

    [Header("Move Button")]
    [SerializeField] private GameObject movebutton;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static NellyDialogue instance;

    [Header("Data Handler")]
    [SerializeField] private DataHandler dataHandler;

    [Header("Inventory Manager")]
    [SerializeField] private InventoryManager inventoryManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Chismis Dialogue in the scene");
        }
        instance = this;
    }

    public static NellyDialogue GetInstance()
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
            Debug.Log("Current Story Text: " + storyText);
            DisplayChoices();

            if (storyText.Contains("Spanish Bread"))
            {
                Debug.Log("Adding 'Spanish Bread' to the inventory...");
                if (inventoryManager != null)
                {
                    inventoryManager.AddItem("Spanish Bread");
                    Debug.Log("'Spanish Bread' added to the inventory!");
                }
                else
                {
                    Debug.LogError("InventoryManager not assigned!");
                }
            }
            if (storyText.Contains("Crinkles"))
            {
                Debug.Log("Adding 'Crinkles' to the inventory...");
                if (inventoryManager != null)
                {
                    inventoryManager.AddItem("Crinkles");
                    Debug.Log("'Crinkles' added to the inventory!");
                }
                else
                {
                    Debug.LogError("InventoryManager not assigned!");
                }
            }
            if (storyText.Contains("Pan de Coco"))
            {
                Debug.Log("Adding 'Pan de Coco' to the inventory...");
                if (inventoryManager != null)
                {
                    inventoryManager.AddItem("Pan de Coco");
                    Debug.Log("'Pan de Coco' added to the inventory!");
                }
                else
                {
                    Debug.LogError("InventoryManager not assigned!");
                }
            }
            if (storyText.Contains("Egg Pie"))
            {
                Debug.Log("Adding 'Egg Pie' to the inventory...");
                if (inventoryManager != null)
                {
                    inventoryManager.AddItem("Egg Pie");
                    Debug.Log("'Egg Pie' added to the inventory!");
                }
                else
                {
                    Debug.LogError("InventoryManager not assigned!");
                }
            }


            if (storyText.Contains("bought"))
            {

                Debug.Log("Calling BuyItem()...");


                DataHandler dataHandler = FindObjectOfType<DataHandler>();
                if (dataHandler != null)
                {
                    dataHandler.BuyItemBread();
                    Debug.Log("BuyItem called!");
                }

                currentStory.variablesState["boughtItem"] = "";
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
