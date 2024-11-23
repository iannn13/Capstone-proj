using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{
    public StoreItem[] storeItems; 
    public Image itemDisplay;      
    public TMP_Text itemNameText;  
    public TMP_Text costText;      
    public TMP_Text playerStatsText; 
    public Button claimButton;    
    public Transform playerRoom;
    private RoomManager roomManager;

    private int currentIndex = 0; 
    private int playerScore;       
    private int playerCash;       

    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        SceneManager.LoadScene("ROOM", LoadSceneMode.Additive);
        StartCoroutine(InitializePlayerRoom());

        playerScore = PointsManager.Instance != null ? PointsManager.Instance.achievementPoints : 0;
        playerCash = DataHandler.Instance != null ? DataHandler.Instance.GetMoney() : 0;

        // Update UI to reflect current data
        UpdateUI();
    }
    IEnumerator InitializePlayerRoom()
    {
        yield return new WaitForSeconds(0.1f); // Allow PlayerRoom to load completely

        GameObject foundRoom = GameObject.FindGameObjectWithTag("PlayerRoom");
        if (foundRoom != null)
        {
            playerRoom = foundRoom.transform;

            // Hide the PlayerRoom visuals
            PlayerRoomManager roomManager = playerRoom.GetComponent<PlayerRoomManager>();
            if (roomManager != null)
            {
                roomManager.HideRoom();
            }
        }
        else
        {
            Debug.LogError("Player Room not found! Make sure it is tagged and set up correctly.");
        }
    }

    public void NextItem()
    {
        currentIndex = (currentIndex + 1) % storeItems.Length;
        UpdateUI();
        UpdatePlayerStats();
    }

    public void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + storeItems.Length) % storeItems.Length;
        UpdateUI();
        UpdatePlayerStats();
    }

    public void ClaimItem()
    {
        Debug.Log("ClaimItem method triggered");
        // Ensure the roomManager exists before calling SaveRoomState
        if (roomManager != null)
        {
            StoreItem currentItem = storeItems[currentIndex];

            if (playerScore >= currentItem.scoreRequirement && playerCash >= currentItem.cashRequirement)
            {
                playerScore -= currentItem.scoreRequirement;
                playerCash -= currentItem.cashRequirement;

                if (PointsManager.Instance != null)
                    PointsManager.Instance.achievementPoints = playerScore;

                if (DataHandler.Instance != null)
                    DataHandler.Instance.AddMoney(-currentItem.cashRequirement);

                GameObject newItem = Instantiate(currentItem.itemPrefab, playerRoom);

                roomManager.roomItems.Add(newItem); // Add to the list in RoomManager
                roomManager.SaveRoomState(); // Save the room state
            }
        }
    }


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

    public void UpdatePlayerStats()
    {
        // Dynamically reload player stats from PointsManager and DataHandler
        playerScore = PointsManager.Instance != null ? PointsManager.Instance.achievementPoints : 0;
        playerCash = DataHandler.Instance != null ? DataHandler.Instance.GetMoney() : 0;

        // Refresh the UI
        UpdateUI();
    }
}
