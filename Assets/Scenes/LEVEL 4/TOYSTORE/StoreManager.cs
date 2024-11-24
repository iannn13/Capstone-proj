using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditorInternal.Profiling.Memory.Experimental;


public class StoreManager : MonoBehaviour
{
    public StoreItem[] storeItems;  // Array of store items
    public Image itemDisplay;  // UI Image to display item image
    public TMP_Text itemNameText;  // UI Text to display item name
    public TMP_Text costText;  // UI Text to display item cost (score & cash)
    public TMP_Text playerStatsText;  // UI Text to display player's score and cash
    public Button claimButton;  // Button to claim the item

    private int currentIndex = 0;  // Index to track the current store item
    private int playerScore;  // Player's current score
    private int playerCash;  // Player's current cash

    void Start()
    {
        // Initialize player stats
        playerScore = PointsManager.Instance != null ? PointsManager.Instance.achievementPoints : 0;
        playerCash = DataHandler.Instance != null ? DataHandler.Instance.GetMoney() : 0;

        // Update UI to reflect current data
        UpdateUI();
    }

    // Navigate to the next item in the store
    public void NextItem()
    {
        currentIndex = (currentIndex + 1) % storeItems.Length;  // Loop back to first item after last
        UpdateUI();
        UpdatePlayerStats();
    }

    // Navigate to the previous item in the store
    public void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + storeItems.Length) % storeItems.Length;  // Loop back to last item if at first
        UpdateUI();
        UpdatePlayerStats();
    }

    public GameObject[] itemsInScene;
    // Claim the current item if requirements are met
    public void ClaimItem()
    {
        StoreItem currentItem = storeItems[currentIndex];

        // Check if the player meets the requirements
        if (playerScore >= currentItem.scoreRequirement && playerCash >= currentItem.cashRequirement)
        {
            // Deduct player score and cash
            playerScore -= currentItem.scoreRequirement;
            playerCash -= currentItem.cashRequirement;

            PointsManager.Instance.achievementPoints = playerScore;
            DataHandler.Instance.AddMoney(-currentItem.cashRequirement);

            // Find and activate the item in the scene
            foreach (var item in itemsInScene)
            {
                if (item.name == currentItem.itemToActivateName)
                {
                    item.SetActive(true); // Activate the item

                    // Add the activated item to GameManager's claimedItems list
                    GameManager.Instance.claimedItems.Add(item);

                    // Save the list of claimed items
                    GameManager.Instance.SaveClaimedItems();

                    break; // Exit the loop once the item is found and activated
                }
            }

            // Update the UI after the transaction is complete
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Player does not meet the requirements to claim this item.");
        }
    }



    // Update the UI with the current item's data and player stats
    private void UpdateUI()
    {
        // Get the current item
        StoreItem currentItem = storeItems[currentIndex];

        // Update item display and stats
        itemDisplay.sprite = currentItem.itemImage;
        itemNameText.text = currentItem.itemName;
        costText.text = $"Score: {currentItem.scoreRequirement} | Cash: {currentItem.cashRequirement}";
        playerStatsText.text = $"Score: {playerScore} | Cash: {playerCash}";

        // Enable or disable the claim button based on requirements
        claimButton.interactable = playerScore >= currentItem.scoreRequirement && playerCash >= currentItem.cashRequirement;
    }

    // Update player stats dynamically from PointsManager and DataHandler
    public void UpdatePlayerStats()
    {
        playerScore = PointsManager.Instance != null ? PointsManager.Instance.achievementPoints : 0;
        playerCash = DataHandler.Instance != null ? DataHandler.Instance.GetMoney() : 0;

        // Refresh the UI to reflect the updated stats
        UpdateUI();
    }

    // Method for handling the main menu button press
    public void OnMainMenuButtonPressed()
    {
        GameManager.Instance.SaveClaimedItems();

        SceneManager.LoadScene("Main Menu"); // Load the main menu scene
    }
}
