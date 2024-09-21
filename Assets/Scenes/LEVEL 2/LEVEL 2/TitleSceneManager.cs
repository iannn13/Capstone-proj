using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public Animator fadeAnimator;  // Reference to the Animator component handling fade
    public float delayBeforeFade = 3f;  // Delay before fade starts
    public float fadeDuration = 1.5f;   // Duration of the fade animation

    void Start()
    {
        // Start the coroutine to handle the fade-out and scene transition
        StartCoroutine(FadeOutAndLoadNextScene());
    }

    IEnumerator FadeOutAndLoadNextScene()
    {
        // Wait for the initial delay before starting the fade-out
        yield return new WaitForSeconds(delayBeforeFade);

        // Trigger the fade-out animation
        fadeAnimator.SetTrigger("FadeOut");

        // Wait for the fade duration to complete
        yield return new WaitForSeconds(fadeDuration);

        // Load the next scene
        SceneManager.LoadScene(13);
    }
}
