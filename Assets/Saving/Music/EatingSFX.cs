using UnityEngine;
using UnityEngine.UI;

public class EatingSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}
