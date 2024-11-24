using System;
using System.Collections;
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
    public Button eatButton;
    public GameObject eatButtonPanel;

    public Button leftButton;
    public Button rightButton;

    public float boostMultiplier = 2f; 
    public float boostDuration = 5f; 
    private float originalRightForce; 
    private float originalLeftForce;  
    public RightMove rightMove;       
    public LeftMove leftMove;

    public Slider speedBoostTimerSlider; // Reference to the Slider UI element
    private bool isBoostActive = false;

    public int ItemsCount => items.Count;

    // List of items that cannot be deleted
    private List<string> undeletableItems = new List<string>() { "LunchBox" }; 

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
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ClearInventory();
            RefreshInventoryUI();
        }
        else
        {
            LoadInventory();
        }
        deleteButton.onClick.AddListener(ShowDeleteConfirmation);
        deleteButton.interactable = false;
        confirmDeleteButton.onClick.AddListener(DeleteSelectedItem);
        cancelDeleteButton.onClick.AddListener(HideDeleteConfirmation);
        confirmDeletePanel.SetActive(false);

        leftButton.onClick.AddListener(OnLeftButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);
    }

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

    void HideDeleteConfirmation()
    {
        confirmDeletePanel.SetActive(false);
    }

    void DeleteSelectedItem()
    {
        if (!string.IsNullOrEmpty(selectedItem))
        {

            if (undeletableItems.Contains(selectedItem))
            {
                Debug.LogWarning("Cannot delete item: " + selectedItem + " because it is protected.");
                HideDeleteConfirmation(); 
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
            if (items.Count < maxItems) 
            {
                items.Add(itemName); 
                CreateButton(itemName); 
                SaveInventory(); 
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
        string path = itemName; 
        Sprite itemSprite = Resources.Load<Sprite>(path);

        if (itemSprite == null)
        {
            Debug.LogWarning("Sprite for item " + itemName + " not found at path: " + path);
        }

        return itemSprite;
    }


    string[] edibleItems = new string[] { "Oleo Cookies", "Skeetels", "Mani Nuts", "Mang John Chips",
                                          "Spanish Bread", "Crinkles", "Pan de Coco", "Egg Pie",
                                          "Strawberry Ice cream", "Watermelon Ice cream", "Ube Ice cream", "Choco Ice cream" };

    void OnItemClick(string itemName)
    {
        selectedItem = itemName;
        Debug.Log("Clicked on: " + itemName);

 
        if (Array.Exists(edibleItems, element => element == itemName))
        {
            eatButtonPanel.SetActive(true);
            eatButton.onClick.RemoveAllListeners(); 
            eatButton.onClick.AddListener(() => EatItem(itemName)); 
        }

        deleteButton.interactable = true;
    }


    public void EatItem(string itemName)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            items.Remove(itemName);
            Debug.Log("Ate item: " + itemName);
            ActivateSpeedBoost();

            RefreshInventoryUI();

            SaveInventory();

            eatButtonPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("No item selected for eating!");
        }
    }
    private void ActivateSpeedBoost()
    {
        if (isBoostActive) return; // Prevent multiple boosts at the same time

        isBoostActive = true;

        // Apply boost to RightMove
        if (rightMove != null)
        {
            originalRightForce = rightMove.Force;
            rightMove.Force *= boostMultiplier;
        }
        else
        {
            Debug.LogError("RightMove script is not assigned!");
        }

        // Apply boost to LeftMove
        if (leftMove != null)
        {
            originalLeftForce = leftMove.Force;
            leftMove.Force *= boostMultiplier;
        }
        else
        {
            Debug.LogError("LeftMove script is not assigned!");
        }

        // Initialize and activate slider
        if (speedBoostTimerSlider != null)
        {
            speedBoostTimerSlider.maxValue = boostDuration;
            speedBoostTimerSlider.value = boostDuration;
            speedBoostTimerSlider.gameObject.SetActive(true);
        }

        // Start timer and reset coroutine
        StartCoroutine(SpeedBoostTimer());
    }

    private IEnumerator SpeedBoostTimer()
    {
        float remainingTime = boostDuration;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // Update the slider value
            if (speedBoostTimerSlider != null)
            {
                speedBoostTimerSlider.value = remainingTime;
            }

            yield return null;
        }

        // Hide the slider and reset speed
        if (speedBoostTimerSlider != null)
        {
            speedBoostTimerSlider.gameObject.SetActive(false);
        }

        ResetSpeed();
        isBoostActive = false;
    }

    private void ResetSpeed()
    {
        // Reset RightMove speed
        if (rightMove != null)
        {
            rightMove.Force = originalRightForce;
        }

        // Reset LeftMove speed
        if (leftMove != null)
        {
            leftMove.Force = originalLeftForce;
        }
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

    void OnLeftButtonClicked()
    {
        selectedItem = null;
        eatButtonPanel.SetActive(false);
        deleteButton.interactable = false;
        Debug.Log("Left button clicked: Deselected item.");
    }

    // Right button click handler
    void OnRightButtonClicked()
    {
        selectedItem = null;
        eatButtonPanel.SetActive(false);
        deleteButton.interactable = false;
        Debug.Log("Right button clicked: Deselected item.");
    }

}
