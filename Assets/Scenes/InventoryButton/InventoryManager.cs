using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } 
    public GameObject inventoryPanel; 
    public GameObject buttonPrefab; 
    public Button deleteButton; 
    public GameObject confirmDeletePanel; 
    public Button confirmDeleteButton; 
    public Button cancelDeleteButton;
    private List<string> items = new List<string>(); 
    private string selectedItem; 
    private const int maxItems = 5;
    public int ItemsCount => items.Count;

    // List of items that cannot be deleted
    private List<string> undeletableItems = new List<string>() { "" }; 

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

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ClearInventory();
            RefreshInventoryUI();
        }
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
        Debug.Log("Populating inventory with " + items.Count + " items.");

        foreach (var item in items)
        {
            Debug.Log("Creating button for item: " + item);
            CreateButton(item);
        }
    }


    public void CreateButton(string itemName)
    {
        Debug.Log("Creating button for: " + itemName);

        if (buttonPrefab == null)
        {
            Debug.LogError("Button prefab is not assigned!");
            return;
        }

        if (inventoryPanel == null)
        {
            Debug.LogError("Inventory panel is not assigned!");
            return;
        }

        /*        // Check if the inventory has reached the maximum limit
        if (items.Count >= maxItems)
        {
            Debug.LogWarning("Max item limit reached! Cannot create button for: " + itemName);
            return;
        }*/

        // Instantiate the button and add it to the inventory panel
        GameObject newButton = Instantiate(buttonPrefab, inventoryPanel.transform);
        Debug.Log("Button instantiated for: " + itemName);

        // Set button text
        Text buttonText = newButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = itemName;
            Debug.Log("Set button text for: " + itemName);
        }

        // Set button sprite
        Sprite itemSprite = GetItemSprite(itemName);
        Image buttonImage = newButton.GetComponent<Image>();
        if (buttonImage != null && itemSprite != null)
        {
            buttonImage.sprite = itemSprite;
            Debug.Log("Set button sprite for: " + itemName);
        }

        // Add click listener
        Button button = newButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnItemClick(itemName));
            Debug.Log("Click listener added for: " + itemName);
        }
        else
        {
            Debug.LogError("Button component not found!");
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
                Debug.Log("Current item count: " + items.Count);
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

    void LoadInventory()
    {
        int count = PlayerPrefs.GetInt("ItemCount", 0);
        items.Clear();
        for (int i = 0; i < count; i++)
        {
            string item = PlayerPrefs.GetString("Item" + i, string.Empty);
            if (!string.IsNullOrEmpty(item))
            {
                items.Add(item);
                Debug.Log("Loaded item: " + item); // Debug to check
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
        Debug.Log("Refreshing UI. Clearing all buttons.");

        foreach (Transform child in inventoryPanel.transform)
        {
            Debug.Log("Destroying button for: " + child.name);
            Destroy(child.gameObject);
        }

        PopulateInventory();
    }

    public void ClearInventory()
    {
        items.Clear();
        SaveInventory();
    }

}
