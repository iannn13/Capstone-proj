using UnityEngine;

public class PlayerRoom : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayerRoom");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
