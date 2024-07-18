using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class doorTrigger : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button doorButton;

    [Header("Cue")]
    [SerializeField] private GameObject cue;

    private bool playerInRange;
    private Collider2D doorCollider;

    void Start()
    {
        playerInRange = false;
        cue.SetActive(false);
        doorCollider = GetComponent<Collider2D>();

        doorButton.gameObject.SetActive(true);
        doorButton.onClick.AddListener(OnDoorButtonClicked);
        doorButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            cue.SetActive(true);
            doorButton.gameObject.SetActive(true);
        }
        else
        {
            cue.SetActive(false);
            doorButton.gameObject.SetActive(false);
        }
    }

    private void OnDoorButtonClicked()
    {
        Debug.Log("Door button clicked");
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (FadeManager.Instance != null)
        {
            yield return FadeManager.Instance.FadeOut(() => SceneManager.LoadSceneAsync(3));
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
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
