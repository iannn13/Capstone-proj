using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("FadeManager initialized and set to persist across scenes.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        StartCoroutine(FadeIn());
    }

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned!");
            return;
        }

        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Debug.Log("Starting FadeIn");
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // Start fully opaque
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
        Debug.Log("FadeIn complete");
    }

    public IEnumerator FadeOut(System.Action onComplete)
    {
        Debug.Log("Starting FadeOut");
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f; // Start fully transparent
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        Debug.Log("FadeOut complete");
        onComplete?.Invoke();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
