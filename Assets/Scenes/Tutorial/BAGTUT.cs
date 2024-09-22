using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button pickup;

    [Header("Cue")]
    [SerializeField] private GameObject bagcue;

    [Header("Bag Image")]
    [SerializeField] private GameObject bagImage; // Declare the BagImage variable

    private bool playerInRange;
    private Collider2D bagtrigger;

    void Start()
    {
        playerInRange = true;
        bagcue.SetActive(false);
        bagtrigger = GetComponent<Collider2D>();
        pickup.gameObject.SetActive(false);
        bagImage.SetActive(false); // Initialize the BagImage
    }

    private void Update()
    {
        if (playerInRange)
        {
            bagcue.SetActive(true);
            pickup.gameObject.SetActive(true);
            bagImage.SetActive(true); // Use SetActive to show the BagImage
        }
    
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true; // Player entered the trigger area
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false; // Player left the trigger area
        }
    }
}
