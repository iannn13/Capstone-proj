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

    private bool playerInRange;
    private Collider2D bagCollider;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        bag.gameObject.SetActive(false);
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
        uiCanvas3.SetActive(false);
        pickupButton.gameObject.SetActive(false);
        visualCue.SetActive(false);
        bagItem.gameObject.SetActive(false);
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
}
