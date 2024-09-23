using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bagTriggertutorial : MonoBehaviour
{
    [Header("Bagcue")]
    [SerializeField] private GameObject BagCue;

    [Header("BAG")]
    [SerializeField] private GameObject BAG;

    [Header("UI Button")]
    [SerializeField] private Button lunchboxinteract;

    [Header("Text")]
    [SerializeField] private GameObject Move;

    [Header("baginven")]
    [SerializeField] private GameObject baginventory;

    [Header("arrowpickup")]
    [SerializeField] private GameObject arrowpickup;

    [Header("textforpickup")]
    [SerializeField] private GameObject forpickup;

    [Header("cue")]
    [SerializeField] private GameObject CUE;

    [Header("arrow")]
    [SerializeField] private GameObject Arrow;

    private bool playerInRange;
    private Collider2D bagcollider;


    private void Awake()
    {
        playerInRange = false;
        BagCue.SetActive(false);
        BAG.gameObject.SetActive(false);
        bagcollider = GetComponent<Collider2D>();
        Move.gameObject.SetActive(true);
        baginventory.gameObject.SetActive(false);

        arrowpickup.gameObject.SetActive(false);
        forpickup.gameObject.SetActive(false);

        lunchboxinteract.gameObject.SetActive(true);
        lunchboxinteract.onClick.AddListener(OnpickupButtonClicked);
        lunchboxinteract.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (playerInRange)
        {
            lunchboxinteract.gameObject.SetActive(true);
            arrowpickup.gameObject.SetActive(true);
            BAG.gameObject.SetActive(true);
            BagCue.gameObject.SetActive(true);
            forpickup.gameObject.SetActive(true);
            Move.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(false);
            


        }
        else
        {
           
            lunchboxinteract.gameObject.SetActive(false);
            Move.gameObject.SetActive(true);
        }
    }

    private void OnpickupButtonClicked()
    {
        lunchboxinteract.gameObject.SetActive(false);
        BagCue.SetActive(false);
        BAG.gameObject.SetActive(false);
        forpickup.gameObject.SetActive(false);

        playerInRange = false;
        bagcollider.enabled = false;
        Move.gameObject.SetActive(false);
        baginventory.gameObject.SetActive(true);
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
