using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // Start is called before the first frame update
    void Start()
    {
        playerInRange = false;
        cue.SetActive(false);
        doorCollider = GetComponent<Collider2D>();


        doorButton.gameObject.SetActive(true);
        doorButton.onClick.AddListener(OndoorButtonClicked);
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

    private void OndoorButtonClicked()
    {
        SceneManager.LoadSceneAsync(3);
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
