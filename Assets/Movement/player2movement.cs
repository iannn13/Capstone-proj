using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightMovePlayer2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed = false;
    public GameObject Player2;
    public float Force;

    public Vector3 targetPosition; // The target position to walk towards
    public bool moveToTarget = false;

    private bool stopMoving = false; // To track if player should stop moving

    private void Start()
    {
        if (Player2 == null)
        {
            Debug.LogError("Player2 is not assigned in the inspector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMoving)
        {
            if (isPressed && Player2 != null)
            {
                // Move right continuously
                Player2.transform.Translate(Force * Time.deltaTime, 0, 0);
            }
            else if (moveToTarget && Player2 != null)
            {
                // Move towards the target position
                float step = Force * Time.deltaTime; // Speed to move
                Player2.transform.position = Vector3.MoveTowards(Player2.transform.position, targetPosition, step);

                // Stop moving if the target is reached
                if (Vector3.Distance(Player2.transform.position, targetPosition) < 0.1f)
                {
                    moveToTarget = false;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
        moveToTarget = true;
    }

    // Stop movement when the player2 touches a trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopTrigger"))
        {
            stopMoving = true; // Stops movement when entering the trigger
        }
    }

    // Optional: Restart movement if the player exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StopTrigger"))
        {
            stopMoving = false; // Resumes movement when exiting the trigger
        }
    }
}
