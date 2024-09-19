using UnityEngine;

public class DialogueSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip dialogueOpenClip;

    private bool isFirstEnable = true;

    private void OnEnable()
    {
        if (!isFirstEnable) // Only play if this is not the first enable (on scene start)
        {
            PlayDialogueOpenSound();
        }
        else
        {
            isFirstEnable = false;
        }
    }

    private void PlayDialogueOpenSound()
    {
        if (audioSource != null && dialogueOpenClip != null)
        {
            audioSource.PlayOneShot(dialogueOpenClip);
        }
    }
}
