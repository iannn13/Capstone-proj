using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{
    public List<StoreItem> storeItems;  // Change to List to dynamically modify it
    public Image itemDisplay;
    public TMP_Text itemNameText;
    public TMP_Text costText;
    public TMP_Text playerStatsText;
    public Button claimButton;
    public GameObject startButton;

    public GameObject[] itemsInScene; 

    private int currentIndex = 0;
    private int playerScore;
    private int playerCash;

    void Start()
    {
        startButton.gameObject.SetActive(true);
        playerScore = PointsManager.Instance != null ? PointsManager.Instance.achievementPoints : 0;
        playerCash = DataHandler.Instance != null ? DataHandler.Instance.GetMoney() : 0;

        UpdateUI();
    }

    public void NextItem()
    {
        currentIndex = (currentIndex + 1) % storeItems.Count; 
        UpdateUI();
        UpdatePlayerStats();
    }

    public void Startbutton()
    {
        startButton.gameObject.SetActive(false);
        UpdateUI();
        UpdatePlayerStats();
    }

    public void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + storeItems.Count) % storeItems.Count; 
        UpdateUI();
        UpdatePlayerStats();
    }

    public void ClaimItem()
    {
        StoreItem currentItem = storeItems[currentIndex];

        if (playerScore >= currentItem.scoreRequirement && playerCash >= currentItem.cashRequirement && !currentItem.isClaimed)
        {

            playerCash -= currentItem.cashRequirement;

            PointsManager.Instance.achievementPoints = playerScore;
            DataHandler.Instance.AddMoney(-currentItem.cashRequirement);

            // Find and activate the item in the scene
            foreach (var item in itemsInScene)
            {
                if (item.name == currentItem.itemToActivateName)
                {
                    item.SetActive(true);
                    GameManager.Instance.claimedItems.Add(item);
                    GameManager.Instance.SaveClaimedItems();
                    break;
                }
            }

            currentItem.isClaimed = true;

            storeItems.RemoveAt(currentIndex);

            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Player does not meet the requirements to claim this item, or the item is already claimed.");
        }
    }

    private void UpdateUI()
    {
        // Get the current item
        if (storeItems.Count == 0)
        {
            itemDisplay.sprite = null;
            itemNameText.text = "No more items";
            costText.text = "N/A";
            playerStatsText.text = $"Score: {playerScore} | Cash: {playerCash}";
            claimButton.gameObject.SetActive(false); // Hide the claim button if no items are left
            return;
        }

        StoreItem currentItem = storeItems[currentIndex];

        // Update item display and stats
        itemDisplay.sprite = currentItem.itemImage;
        itemNameText.text = currentItem.itemName;
        costText.text = $"Score: {currentItem.scoreRequirement} | Cash: {currentItem.cashRequirement}";
        playerStatsText.text = $"Score: {playerScore} | Cash: {playerCash}";

        // Enable or disable the claim button based on requirements and claimed status
        claimButton.interactable = playerScore >= currentItem.scoreRequirement && playerCash >= currentItem.cashRequirement && !currentItem.isClaimed;
    }

    public void UpdatePlayerStats()
    {
        playerScore = PointsManager.Instance != null ? PointsManager.Instance.achievementPoints : 0;
        playerCash = DataHandler.Instance != null ? DataHandler.Instance.GetMoney() : 0;

        // Refresh the UI to reflect the updated stats
        UpdateUI();
    }

    public void OnMainMenuButtonPressed()
    {
        GameManager.Instance.SaveClaimedItems();
        SceneManager.LoadScene("Main Menu"); // Load the main menu scene
    }
}
