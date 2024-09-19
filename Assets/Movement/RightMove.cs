using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed = false;
    public GameObject Player;
    public float Force;
    public Animator animator;

    private MamaDialogue dialogueManager;

    private void Start()
    {
        dialogueManager = MamaDialogue.GetInstance();

        if (Player == null)
        {
            Debug.LogError("Player is not assigned in the inspector");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in the inspector");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager != null && dialogueManager.dialogueIsPlaying)
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
            return;
        }

        if (isPressed && Player != null)
        {
            Player.transform.Translate(Force * Time.deltaTime, 0, 0);
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
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
}
