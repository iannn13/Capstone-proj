using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nellyTsrigger : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button Interact;

    [Header("Cue")]
    [SerializeField] private GameObject cue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private Collider2D NellyCollider;

    [Header("Inventory Full Panel")]
    [SerializeField] private GameObject inventoryFullPanel;
    [SerializeField] private Button inventoryFullOkButton;

    [Header("Insufficient Funds Panel")]
    [SerializeField] private GameObject insufficientFundsPanel;
    [SerializeField] private Button insufficientFundsOkButton;

    void Start()
    {
        playerInRange = false;
        cue.SetActive(false);
        NellyCollider = GetComponent<Collider2D>();

        Interact.gameObject.SetActive(true);
        Interact.onClick.AddListener(OnDoorButtonClicked);
        Interact.gameObject.SetActive(false);
        inventoryFullOkButton.onClick.AddListener(() => inventoryFullPanel.SetActive(false));
        insufficientFundsOkButton.onClick.AddListener(() => insufficientFundsPanel.SetActive(false));
    }

    private void Update()
    {
        if (playerInRange && !NellyDialogue.GetInstance().dialogueIsPlaying)
        {
            cue.SetActive(true);
            Interact.gameObject.SetActive(true);
        }
        else
        {
            cue.SetActive(false);
            Interact.gameObject.SetActive(false);
        }
    }

    private void OnDoorButtonClicked()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.ItemsCount >= 5)
        {
            // Show the "Inventory Full" panel if the inventory is full
            inventoryFullPanel.SetActive(true);
        }
        else if (DataHandler.Instance != null && DataHandler.Instance.GetMoney() < 5)
        {
            insufficientFundsPanel.SetActive(true); 
        }
        else
        {
            NellyDialogue.GetInstance().EnterDialogueMode(inkJSON);
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (FadeManager.Instance != null)
        {
            yield return FadeManager.Instance.FadeOut(() => SceneManager.LoadSceneAsync(10));
        }
        else
        {
            Debug.LogError("FadeManager instance is null!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger");
            playerInRange = false;
        }
    }
}
