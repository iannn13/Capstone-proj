using UnityEngine;

public class PlayerRoomManager : MonoBehaviour
{
    public GameObject roomCanvas; // Assign your PlayerRoom Canvas or root GameObject here

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep this GameObject across scenes

        GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayerRoom");
        if (objs.Length > 1)
        {
            Destroy(gameObject); // Destroy duplicate PlayerRoom instances
            return;
        }
    }

    public void HideRoom()
    {
        if (roomCanvas != null)
        {
            roomCanvas.SetActive(false); // Deactivate the visual representation of PlayerRoom
        }
    }

    public void ShowRoom()
    {
        if (roomCanvas != null)
        {
            roomCanvas.SetActive(true); // Activate when the PlayerRoom is needed
        }
    }
}
