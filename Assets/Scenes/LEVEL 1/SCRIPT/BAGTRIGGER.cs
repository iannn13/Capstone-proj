using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BAGTRIGGER : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("BAG")]
    [SerializeField] private GameObject bag;

    [Header("UI Button")]
    [SerializeField] private Button pickupButton;

    [Header("BagItem")]
    [SerializeField] private GameObject bagItem;

    [Header("BagNote")]
    [SerializeField] private GameObject uiCanvas3;
    [SerializeField] private GameObject bagNote;

    [Header("pickupNote")]
    [SerializeField] private GameObject uiCanva4;
    [SerializeField] private GameObject pickupnote;

    [Header("Wall")]
    [SerializeField] private GameObject wall;

    private bool playerInRange;
    private Collider2D bagCollider;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        bag.gameObject.SetActive(false);
        wall.gameObject.SetActive(true);
        bagItem.gameObject.SetActive(true);
        bagCollider = GetComponent<Collider2D>();


        pickupButton.gameObject.SetActive(true);
        pickupButton.onClick.AddListener(OnpickupButtonClicked);
        pickupButton.gameObject.SetActive(false); 
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            pickupButton.gameObject.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
            pickupButton.gameObject.SetActive(false);
        }
    }

    private void OnpickupButtonClicked()
    {
        bag.gameObject.SetActive(true);
        wall.gameObject.SetActive(false);
        uiCanvas3.SetActive(false);
        uiCanva4.SetActive(true);
        pickupButton.gameObject.SetActive(false);
        visualCue.SetActive(false);
        bagItem.gameObject.SetActive(false);
        playerInRange = false;
        bagCollider.enabled = false;

        StartCoroutine(FadeOutUIAfterDelay(uiCanva4, 1.5f, 1.0f));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private IEnumerator FadeOutUIAfterDelay(GameObject uiElement, float delay, float duration)
    {
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiElement.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1.0f;
        yield return new WaitForSeconds(delay);

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / duration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = 0;
        uiElement.SetActive(false);
    }
}
