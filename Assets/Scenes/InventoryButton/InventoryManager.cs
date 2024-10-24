using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton instance
    public GameObject inventoryPanel; // Assign your inventory panel in the Inspector
    public GameObject buttonPrefab; // Assign your button prefab in the Inspector
    public Button deleteButton; // Assign your delete button in the Inspector
    public GameObject confirmDeletePanel; // Assign your confirmation panel in the Inspector
    public Button confirmDeleteButton; // Assign your confirm button in the Inspector
    public Button cancelDeleteButton; // Assign your cancel button in the Inspector
    private List<string> items = new List<string>(); // List to hold inventory items
    private string selectedItem; // To keep track of the selected item
    private const int maxItems = 5; // Maximum items in inventory

    // List of items that cannot be deleted
    private List<string> undeletableItems = new List<string>() { "LunchBox" }; // Add items here

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadInventory();
        deleteButton.onClick.AddListener(ShowDeleteConfirmation);
        deleteButton.interactable = false;
        confirmDeleteButton.onClick.AddListener(DeleteSelectedItem);
        cancelDeleteButton.onClick.AddListener(HideDeleteConfirmation);
        confirmDeletePanel.SetActive(false);
    }

    // Show the confirmation panel when delete button is clicked
    void ShowDeleteConfirmation()
    {
        if (!string.IsNullOrEmpty(selectedItem))
        {
            if (undeletableItems.Contains(selectedItem))
            {
                Debug.LogWarning("This item cannot be deleted: " + selectedItem);
                return;
            }
            confirmDeletePanel.SetActive(true);
        }
    }

    // Hide the confirmation panel if user cancels the deletion
    void HideDeleteConfirmation()
    {
        confirmDeletePanel.SetActive(false);
    }

    // Method to delete the selected item
    void DeleteSelectedItem()
    {
        if (!string.IsNullOrEmpty(selectedItem))
        {
            // Check if the selected item is in the undeletable list
            if (undeletableItems.Contains(selectedItem))
            {
                Debug.LogWarning("Cannot delete item: " + selectedItem + " because it is protected.");
                HideDeleteConfirmation(); // Hide the panel if deletion is not allowed
                return;
            }

            items.Remove(selectedItem);
            Debug.Log("Deleted item: " + selectedItem);
            RefreshInventoryUI();
            deleteButton.interactable = false;
            selectedItem = null;
            SaveInventory();
            HideDeleteConfirmation();
        }
        else
        {
            Debug.LogError("No item selected for deletion!");
        }
    }
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
        selectedItem = itemName; 
        Debug.Log("Clicked on: " + itemName); 
        deleteButton.interactable = true;
    }

    private void RefreshInventoryUI()
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject); 
        }

        PopulateInventory(); 
    }
}
