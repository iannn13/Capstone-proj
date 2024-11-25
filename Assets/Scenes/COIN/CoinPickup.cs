using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 10; 
    public string coinID; 

    public AudioSource pickupSound; // Sound to play when the coin is picked up


    private void Start()
    {
        if (PlayerPrefs.GetInt(coinID, 0) == 1) 
        {
            Destroy(gameObject); 
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            pickupSound.Play();
            // Add money to the player
            if (DataHandler.Instance != null)
            {
                DataHandler.Instance.AddMoney(coinValue);
            }

            PlayerPrefs.SetInt(coinID, 1); 

            Destroy(gameObject);
        }
    }
}
