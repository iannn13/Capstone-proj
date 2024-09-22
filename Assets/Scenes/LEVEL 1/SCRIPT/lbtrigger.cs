using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class lbtrigger : MonoBehaviour
{
    public GameObject pickup; // Assign the button GameObject in the Inspector
    [Header("UI Button")]
    [SerializeField] private Button pickupButton;
    // When the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered: " + other.gameObject.name); // Debug message
        if (other.CompareTag("Player"))
        {
            pickup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exited: " + other.gameObject.name); // Debug message
        if (other.CompareTag("Player"))
        {
            pickup.SetActive(false);
        }
    }
}