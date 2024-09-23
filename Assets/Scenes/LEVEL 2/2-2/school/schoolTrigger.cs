using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class schoolTrigger : MonoBehaviour
{
    [Header("GameObject to Show")]
    [SerializeField] private GameObject objectToShow;

    [Header("Button to Hide GameObject")]
    [SerializeField] private Button hideButton;

    [Header("School")]
    [SerializeField] private GameObject school;

    [Header("After School")]
    [SerializeField] private GameObject wall;




    private bool playerInRange;
    private Collider2D doorCollider;

    void Start()
    {
        playerInRange = false;
        objectToShow.SetActive(false);
        hideButton.gameObject.SetActive(false);
        wall.SetActive(false);
        doorCollider = GetComponent<Collider2D>();

        hideButton.onClick.AddListener(OnHideButtonClicked);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger, starting fade-out and fade-in.");
            playerInRange = true;
            StartCoroutine(FadeOutAndFadeInObject());
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger.");
            playerInRange = false;
        }
    }

    private IEnumerator FadeOutAndFadeInObject()
    {
        if (FadeManager.Instance != null)
        {
            Debug.Log("Starting screen fade-out...");
            yield return FadeManager.Instance.FadeOut(() =>
            {
                Debug.Log("Fade-out complete, now fading in object.");
                objectToShow.SetActive(true);
                StartCoroutine(FadeInObject());
            });
        }
        else
        {
            Debug.LogError("FadeManager instance is null!");
        }
    }

    private IEnumerator FadeInObject()
    {
        CanvasGroup canvasGroup = objectToShow.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = objectToShow.AddComponent<CanvasGroup>();
        }

        objectToShow.SetActive(true);
        canvasGroup.alpha = 0f;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        hideButton.gameObject.SetActive(true);
        wall.SetActive(true);
        StartCoroutine(FadeInButton());
    }

    private IEnumerator FadeInButton()
    {
        CanvasGroup buttonCanvasGroup = hideButton.GetComponent<CanvasGroup>();
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = hideButton.gameObject.AddComponent<CanvasGroup>();
        }

        buttonCanvasGroup.alpha = 0f;
        hideButton.gameObject.SetActive(true);

        while (buttonCanvasGroup.alpha < 1f)
        {
            buttonCanvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    private void OnHideButtonClicked()
    {
        StartCoroutine(FadeOutAndHideObject());
    }

    private IEnumerator FadeOutAndHideObject()
    {
        CanvasGroup canvasGroup = objectToShow.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = objectToShow.AddComponent<CanvasGroup>();
        }

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        objectToShow.SetActive(false);
        
        StartCoroutine(FadeOutButton());
        yield return FadeManager.Instance.FadeIn();
        school.SetActive(false);
       
    }

    private IEnumerator FadeOutButton()
    {
        CanvasGroup buttonCanvasGroup = hideButton.GetComponent<CanvasGroup>();
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = hideButton.gameObject.AddComponent<CanvasGroup>();
        }

        while (buttonCanvasGroup.alpha > 0f)
        {
            buttonCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        hideButton.gameObject.SetActive(false);
    }
}
