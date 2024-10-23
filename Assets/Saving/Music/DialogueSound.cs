using UnityEngine;

public class DialogueSound : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;  // The audio source for sound effects
    [SerializeField] private AudioClip dialogueOpenClip;  // Clip to play when dialogue opens
    [SerializeField] private AudioClip dialogueLoopClip;   // Clip to loop during dialogue
    [SerializeField] private AudioClip dialogueEndClip;    // Clip to play when dialogue ends

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

        // Start looping background sound during dialogue
        if (audioSource != null && dialogueLoopClip != null)
        {
            audioSource.clip = dialogueLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        // Stop looping sound when dialogue ends
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Play dialogue end sound
        if (audioSource != null && dialogueEndClip != null)
        {
            audioSource.PlayOneShot(dialogueEndClip);
        }
    }
}
