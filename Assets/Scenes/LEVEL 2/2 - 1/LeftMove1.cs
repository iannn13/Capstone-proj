using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftMove1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    public GameObject Player;
    public GameObject Wall; // Reference to the wall
    public float Force;
    public Animator animator;

    // Distance between player and wall
    public float wallOffset = 1f; // Wall will be 1 unit behind the player
    private bool wallIsFixed = false; // Flag to check if wall should be fixed

    private void Start()
    {
        if (Player == null)
        {
            Debug.LogError("Player is not assigned in the inspector");
        }

        if (Wall == null)
        {
            Debug.LogError("Wall is not assigned in the inspector");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in the inspector");
        }
    }

    void Update()
    {
        if (isPressed && Player != null && !wallIsFixed)
        {
            // Move player left if not at the wall
            Player.transform.Translate(-Force * Time.deltaTime, 0, 0);
            if (animator != null)
            {
                animator.SetBool("isRunningleft", true);
            }

            // Update wall position to be behind the player
            Wall.transform.position = new Vector2(Player.transform.position.x - wallOffset, Wall.transform.position.y);
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isRunningleft", true);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;

        // Set wall as fixed if the player presses the left button
        if (!wallIsFixed)
        {
            wallIsFixed = true;
            Wall.transform.position = new Vector2(Player.transform.position.x - wallOffset, Wall.transform.position.y); // Fix wall position
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        // Allow wall to follow again when the button is released
        wallIsFixed = false;
    }
}
