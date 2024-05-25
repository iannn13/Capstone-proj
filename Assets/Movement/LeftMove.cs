using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            animator.SetBool("isRunningleft", false);
            return;
        }

        if (isPressed)
        {
            Player.transform.Translate(Force * Time.deltaTime, 0, 0);
            animator.SetBool("isRunningleft", true);
        }
        else
        {
            animator.SetBool("isRunningleft", false); // Set the isRunning parameter to false
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed= false;
    }
}
