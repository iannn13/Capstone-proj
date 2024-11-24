using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> claimedItems = new List<GameObject>(); // Stores the claimed items
    private string saveFilePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Define the file path where data will be saved
        saveFilePath = Application.persistentDataPath + "/claimedItems.json";

        LoadClaimedItems(); // Load saved claimed items when the game starts
    }

    // Save the claimed items to a JSON file
    public void SaveClaimedItems()
    {
        // Ensure claimedItems list is not null
        if (claimedItems == null)
        {
            Debug.LogWarning("claimedItems is null. Reinitializing the list.");
            claimedItems = new List<GameObject>();
        }

        List<ClaimedItemData> itemDataList = new List<ClaimedItemData>();

        foreach (GameObject item in claimedItems)
        {
            if (item == null)
            {
                Debug.LogWarning("One of the claimed items is missing (null). Skipping.");
                continue;  // Skip this item if it's null
            }

            ClaimedItemData data = new ClaimedItemData
            {
                itemName = item.name,
                position = item.transform.position,
                isActive = item.activeSelf // Save the activation state
            };

            itemDataList.Add(data);
        }

        // Save the data as JSON
        string json = JsonUtility.ToJson(new ItemListWrapper { items = itemDataList });
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Claimed items saved.");
    }

    // Load the claimed items from the JSON file
    public void LoadClaimedItems()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);

            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(json);
            if (wrapper != null)
            {
                foreach (ClaimedItemData data in wrapper.items)
                {
                    // Find the item by its name and activate it in the scene
                    GameObject itemToActivate = GetItemByName(data.itemName);
                    if (itemToActivate != null)
                    {
                        // Set the item position and activation state
                        itemToActivate.transform.position = data.position;
                        itemToActivate.SetActive(data.isActive); // Set active state based on saved data
                        if (!claimedItems.Contains(itemToActivate)) // Avoid adding duplicates
                        {
                            claimedItems.Add(itemToActivate); // Add to the claimed items list
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Item {data.itemName} not found in the scene.");
                    }
                }

                Debug.Log("Claimed items loaded.");
            }
        }
        else
        {
            Debug.Log("No saved claimed items found.");
        }
    }

    // Helper method to find the item by its name in the scene (instead of instantiating from a prefab)
    private GameObject GetItemByName(string itemName)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == itemName && obj.scene.isLoaded)
            {
                return obj;
            }
        }
        Debug.LogWarning($"Item {itemName} not found in the scene.");
        return null;
    }

}

// Wrapper class to handle lists in Unity's JSON utility
[System.Serializable]
public class ItemListWrapper
{
    public List<ClaimedItemData> items;
}

// Data class to hold item information (name, position, and active state)
[System.Serializable]
public class ClaimedItemData
{
    public string itemName;  // Name of the item (used as the unique identifier)
    public UnityEngine.Vector3 position; // The position where the item was placed
    public bool isActive;    // The active state of the item
}
