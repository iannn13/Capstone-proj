using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private void Awake()
    {
        // Ensure this GameObject persists across scene changes
        DontDestroyOnLoad(gameObject);

        // Make sure there's only one instance of this GameObject (singleton pattern)
        if (FindObjectsOfType<BackgroundMusicController>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}