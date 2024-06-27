using System.Collections;
using UnityEngine;

public class mamaFadeout : MonoBehaviour
{
    private SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if (rend == null)
        {
            Debug.LogError("SpriteRenderer component missing from this GameObject");
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void startFading()
    {
        StartCoroutine(FadeOut());
    }
}
