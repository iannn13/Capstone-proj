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
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject boxOrig;

    [Header("UI Button")]
    [SerializeField] private Button pickupButton;


    private bool playerInRange;
    private Collider2D bagCollider;


    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        box.gameObject.SetActive(false);
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
        pickupButton.gameObject.SetActive(false);
        visualCue.SetActive(false);
        box.gameObject.SetActive(true);
        boxOrig.gameObject.SetActive(false) ;
   
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
