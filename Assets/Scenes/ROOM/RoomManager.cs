using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomItems = new List<GameObject>(); // List of items in the room
    public Transform playerRoom;

    private void Awake()
    {
        // Ensure only one instance of RoomManager exists
        if (FindObjectsOfType<RoomManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        LoadRoomState(); // Load saved items when the PlayerRoom scene starts
    }



    // Call this method when you need to save the room state
    public void SaveRoomState()
    {
        Debug.Log("Saving room state...");
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
        // Clear current items
        foreach (var item in roomItems)
        {
            Destroy(item);
        }
        roomItems.Clear();

        int itemCount = PlayerPrefs.GetInt("ItemCount", 0);

        for (int i = 0; i < itemCount; i++)
        {
            string itemName = PlayerPrefs.GetString($"Item_{i}_Name", null);
            string itemPositionJson = PlayerPrefs.GetString($"Item_{i}_Position", null);

            if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemPositionJson))
            {
                // Find the item prefab by name in Resources folder
                GameObject itemPrefab = Resources.Load<GameObject>(itemName);
                if (itemPrefab != null)
                {
                    Vector3 position = JsonUtility.FromJson<Vector3>(itemPositionJson);
                    GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity, transform);
                    roomItems.Add(newItem);
                }
            }
        }
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
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayerRoom")
        {
            // Reassign the playerRoom GameObject after the scene is loaded
            GameObject foundRoom = GameObject.FindGameObjectWithTag("PlayerRoom");
            if (foundRoom != null)
            {
                playerRoom = foundRoom.transform;
            }
            else
            {
                Debug.LogError("PlayerRoom GameObject not found!");
            }
        }
    }

}
