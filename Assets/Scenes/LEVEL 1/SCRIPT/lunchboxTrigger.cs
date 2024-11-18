using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lunchboxTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("BAG")]
    [SerializeField] private GameObject boxOrig;

    [Header("UI Button")]
    [SerializeField] private Button pickupButton;

    [Header("pickupNote")]
    [SerializeField] private GameObject uiCanva5;
    [SerializeField] private GameObject pickupnote2;

    [Header("DoorTrigger")]
    [SerializeField] private GameObject cue;


    private bool playerInRange;
    private Collider2D bagCollider;


    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
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
            uiCanva5.gameObject.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
            pickupButton.gameObject.SetActive(false);
            uiCanva5.gameObject.SetActive(false);
        }
    }

    private void OnpickupButtonClicked()
    {
        pickupButton.gameObject.SetActive(false);
        visualCue.SetActive(false);
        boxOrig.gameObject.SetActive(false) ;
        uiCanva5.SetActive(false);
        cue.SetActive(true);

        playerInRange = false;
        bagCollider.enabled = false;
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
