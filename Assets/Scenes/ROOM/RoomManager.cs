using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomItems = new List<GameObject>(); // List of items in the room

    void Start()
    {
        LoadRoomState(); // Load saved items when the PlayerRoom scene starts
    }

    // Call this method when you need to save the room state
    public void SaveRoomState()
    {
        for (int i = 0; i < roomItems.Count; i++)
        {
            // Save item name and position as strings
            PlayerPrefs.SetString($"Item_{i}_Name", roomItems[i].name);
            PlayerPrefs.SetString($"Item_{i}_Position", JsonUtility.ToJson(roomItems[i].transform.position));
        }

        // Save the total count of items
        PlayerPrefs.SetInt("ItemCount", roomItems.Count);
        PlayerPrefs.Save();
        Debug.Log("Room state saved.");
    }

    // Call this method when you need to load the room state
    public void LoadRoomState()
    {
        // Clear current room items (optional, depending on your requirements)
        foreach (GameObject item in roomItems)
        {
            Destroy(item);
        }
        roomItems.Clear();

        // Load the number of items
        int itemCount = PlayerPrefs.GetInt("ItemCount", 0);
        for (int i = 0; i < itemCount; i++)
        {
            string itemName = PlayerPrefs.GetString($"Item_{i}_Name", "");
            string positionJson = PlayerPrefs.GetString($"Item_{i}_Position", "");
            if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(positionJson))
            {
                // Instantiate item at the saved position
                Vector3 position = JsonUtility.FromJson<Vector3>(positionJson);
                GameObject prefab = FindPrefabByName(itemName);
                if (prefab != null)
                {
                    GameObject item = Instantiate(prefab, position, Quaternion.identity);
                    roomItems.Add(item);
                }
            }
        }
        Debug.Log("Room state loaded.");
    }

    // Helper function to find a prefab by its name
    private GameObject FindPrefabByName(string itemName)
    {
        // Replace this with your actual prefab lookup logic
        foreach (GameObject prefab in Resources.LoadAll<GameObject>("ItemPrefabs"))
        {
            if (prefab.name == itemName)
                return prefab;
        }
        return null;
    }
}
