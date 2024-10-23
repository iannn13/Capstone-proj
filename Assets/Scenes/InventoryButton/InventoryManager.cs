using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton instance
    public GameObject inventoryPanel; // Assign your inventory panel in the Inspector
    public GameObject buttonPrefab; // Assign your button prefab in the Inspector
    public Button deleteButton; // Assign your delete button in the Inspector
    private List<string> items = new List<string>(); // List to hold inventory items
    private string selectedItem; // To keep track of the selected item
    private const int maxItems = 5; // Maximum items in inventory

    void Awake()
    {
        // Ensure this object is not destroyed on loading a new scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object across scenes
    }

    void Start()
    {
        LoadInventory(); // Load inventory items on start
        deleteButton.onClick.AddListener(DeleteSelectedItem); // Add listener for delete button
        deleteButton.interactable = false; // Disable delete button initially
    }

    // Populate inventory with existing items
    void PopulateInventory()
    {
        foreach (var item in items)
        {
            CreateButton(item);
        }
    }

    // Method to create a button for an item
    public void CreateButton(string itemName)
    {
        if (buttonPrefab == null)
        {
            Debug.LogError("buttonPrefab is not assigned!");
            return;
        }

        if (inventoryPanel == null)
        {
            Debug.LogError("inventoryPanel is not assigned!");
            return;
        }

        // Check the maximum limit
        if (items.Count >= maxItems)
        {
            Debug.LogWarning("Maximum inventory limit reached!");
            return;
        }

        // Instantiate the button from prefab and add it to the inventory panel
        GameObject newButton = Instantiate(buttonPrefab, inventoryPanel.transform);

        // Check if the button contains a Text component and set the item name
        Text buttonText = newButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = itemName;
        }
        else
        {
            Debug.LogError("Text component not found in the button prefab!");
        }

        // Get the sprite for the item and assign it to the button's Image component
        Sprite itemSprite = GetItemSprite(itemName);
        Image buttonImage = newButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (itemSprite != null)
            {
                buttonImage.sprite = itemSprite;
            }
            else
            {
                Debug.LogWarning("Item sprite not found for: " + itemName);
            }
        }
        else
        {
            Debug.LogError("Image component not found in the button prefab!");
        }

        // Add a click listener to the button
        Button button = newButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnItemClick(itemName)); // Use lambda to pass item name
        }
        else
        {
            Debug.LogError("Button component not found in the prefab!");
        }
    }

    // Method to add an item to the inventory
    public void AddItem(string itemName)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            if (items.Count < maxItems) // Check for max items before adding
            {
                items.Add(itemName); // Add to the list
                CreateButton(itemName); // Create a button for the new item
                SaveInventory(); // Save the inventory after adding
            }
            else
            {
                Debug.LogWarning("Cannot add more items, inventory is full.");
            }
        }
        else
        {
            Debug.LogError("Item name is null or empty!");
        }
    }

    // Method to save the inventory to PlayerPrefs
    public void SaveInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            PlayerPrefs.SetString("Item" + i, items[i]);
        }
        PlayerPrefs.SetInt("ItemCount", items.Count);
        PlayerPrefs.Save();
    }

    // Method to load the inventory from PlayerPrefs
    public void LoadInventory()
    {
        int count = PlayerPrefs.GetInt("ItemCount", 0);
        items.Clear();
        for (int i = 0; i < count; i++)
        {
            string item = PlayerPrefs.GetString("Item" + i, string.Empty);
            if (!string.IsNullOrEmpty(item))
            {
                items.Add(item);
                CreateButton(item); // Create buttons for loaded items
            }
        }
    }

    Sprite GetItemSprite(string itemName)
    {
        string path = itemName; // Adjust the path as needed
        Sprite itemSprite = Resources.Load<Sprite>(path);

        if (itemSprite == null)
        {
            Debug.LogWarning("Sprite for item " + itemName + " not found at path: " + path);
        }

        return itemSprite;
    }

    // Handle item button click
    void OnItemClick(string itemName)
    {
        selectedItem = itemName; // Store selected item
        Debug.Log("Clicked on: " + itemName); // Handle item click (e.g., use or equip the item)
        deleteButton.interactable = true; // Enable delete button
    }

    // Delete the selected item from the inventory
    void DeleteSelectedItem()
    {
        if (!string.IsNullOrEmpty(selectedItem))
        {
            items.Remove(selectedItem); // Remove the item from the list
            Debug.Log("Deleted item: " + selectedItem);
            RefreshInventoryUI(); // Refresh the UI to reflect changes
            deleteButton.interactable = false; // Disable delete button again
            selectedItem = null; // Reset selected item
            SaveInventory(); // Save the inventory after deletion
        }
        else
        {
            Debug.LogError("No item selected for deletion!");
        }
    }

    // Refresh the inventory UI after an item is deleted
    private void RefreshInventoryUI()
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject); // Clear the current UI
        }

        PopulateInventory(); // Repopulate the inventory UI
    }
}
