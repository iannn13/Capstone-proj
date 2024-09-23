using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IceTrigger : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button Interact;

    [Header("Cue")]
    [SerializeField] private GameObject cue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private Collider2D NellyCollider;

    void Start()
    {
        playerInRange = false;
        cue.SetActive(false);
        NellyCollider = GetComponent<Collider2D>();

        Interact.gameObject.SetActive(true);
        Interact.onClick.AddListener(OnDoorButtonClicked);
        Interact.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !IceDialogue.GetInstance().dialogueIsPlaying)
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
        IceDialogue.GetInstance().EnterDialogueMode(inkJSON);
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
