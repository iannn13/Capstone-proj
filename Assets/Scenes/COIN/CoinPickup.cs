using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 10; // Coin value
    public string coinID; // Unique identifier for each coin

    private void Start()
    {
        // Ensure the coin is visible if it's a new game
        if (PlayerPrefs.GetInt(coinID, 0) == 1) // Check if the coin is already collected
        {
            Destroy(gameObject); // If collected, destroy the coin
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has collided with the coin
        if (collision.CompareTag("Player"))
        {
            // Add money to the player
            if (DataHandler.Instance != null)
            {
                DataHandler.Instance.AddMoney(coinValue);
            }

            // Mark this coin as collected
            PlayerPrefs.SetInt(coinID, 1); // Store that the coin is collected

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
