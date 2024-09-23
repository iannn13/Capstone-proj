using MyGameNamespace;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ItemSpritePair
{
    public string itemName; // The name of the item (e.g., "PhoneNumber")
    public Sprite itemSprite; // The sprite representing the item
}
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance; // Singleton instance for easy access

    public List<Image> itemSlots; // UI Image components for the inventory slots
    public Dictionary<string, Sprite> itemSprites; // Maps item names to their sprites
    public Sprite phoneNumberSprite;
    private PlayerDataManager playerDataManager; // Reference to PlayerDataManager

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the inventory UI across scenes (optional)
            itemSprites = new Dictionary<string, Sprite>();
            itemSprites.Add("PhoneNumber", phoneNumberSprite);
            // Example: Populate the dictionary with item names and sprites
            // itemSprites.Add("PhoneNumber", phoneNumberSprite); // Make sure to assign phoneNumberSprite in Inspector or by code
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }

    void Start()
    {
        // Get reference to the PlayerDataManager to access collectedItems
        playerDataManager = FindObjectOfType<PlayerDataManager>();
       
        if (playerDataManager == null)
        {
            Debug.LogError("PlayerDataManager not found in the scene!");
        }

        // Initial update of the inventory display
        UpdateInventoryDisplay(playerDataManager.collectedItems);
    }

    // Call this method to update the inventory UI
    public void UpdateInventoryDisplay(List<string> collectedItems)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (i < collectedItems.Count)
            {
                string itemName = collectedItems[i];
                if (itemSprites.ContainsKey(itemName))
                {
                    itemSlots[i].sprite = itemSprites[itemName]; // Set the item image in the slot
                    itemSlots[i].enabled = true; // Show the image
                }
                else
                {
                    Debug.LogWarning("Sprite not found for item: " + itemName);
                    itemSlots[i].enabled = false; // Hide the slot if the sprite is missing
                }
            }
            else
            {
                itemSlots[i].enabled = false; // Hide unused slots
            }
        }
    }

    // Call this method when an item is collected
    public void CollectItem(string itemName)
    {
        if (playerDataManager != null && !playerDataManager.collectedItems.Contains(itemName))
        {
            // Add the item to the collectedItems list in PlayerDataManager
            playerDataManager.collectedItems.Add(itemName);
            Debug.Log("Collected: " + itemName);

            // Update the inventory UI
            UpdateInventoryDisplay(playerDataManager.collectedItems);
        }
    }
}
