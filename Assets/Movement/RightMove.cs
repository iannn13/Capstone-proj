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

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            animator.SetBool("isRunning", false);
            return;
        }

        if (isPressed)
        {
            Player.transform.Translate(Force * Time.deltaTime, 0, 0);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false); // Set the isRunning parameter to false
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
