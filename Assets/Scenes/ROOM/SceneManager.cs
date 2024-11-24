using UnityEngine;
using UnityEngine.EventSystems;

public class SceneManagerController : MonoBehaviour
{
    public GameObject roomSection;        // Reference to the room section GameObject
    public GameObject toyStoreSection;    // Reference to the toy store section GameObject

    void Start()
    {
        // Initially show the Room and hide the Toy Store
        ShowRoom();
    }

    // Show the Room section and hide the Toy Store
    public void ShowRoom()
    {
        GameManager.Instance.LoadClaimedItems();
        roomSection.SetActive(true);
        toyStoreSection.SetActive(false);
    }

    // Show the Toy Store section and hide the Room
    public void ShowToyStore()
    {
        roomSection.SetActive(false);
        toyStoreSection.SetActive(true);
    }
}
