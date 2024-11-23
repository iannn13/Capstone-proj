using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 10; 
    public string coinID; 

    public AudioClip pickupSound; // Sound to play when the coin is picked up
    private AudioSource audioSource; // AudioSource component

    private void Start()
    {
        if (PlayerPrefs.GetInt(coinID, 0) == 1) 
        {
            Destroy(gameObject); 
        }

          audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Debug.Log(audioSource ? "AudioSource successfully assigned." : "Failed to assign AudioSource.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
                Debug.Log("Playing pickup sound.");
            }
            else{
                Debug.LogError("Pickup sound or AudioSource is missing.");
            }
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
