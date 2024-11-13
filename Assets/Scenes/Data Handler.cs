using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; } // Singleton instance
    public Text moneyText;

    private int money;
    public static bool isNewGame; // Flag to check if starting a new game

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
        if (isNewGame)
        {
            ResetCash();
            // Optionally, reset inventory here if needed
            isNewGame = false; // Reset the flag so it's only effective at the start of a new game
        }
        else
        {
            money = PlayerPrefs.GetInt("PlayerMoney", 50); // Load saved money if not a new game
        }

        UpdateMoneyText();
    }

    public int GetMoney() => money;

    public void BuyItem()
    {
        if (money >= 10)
        {
            money -= 10;
            UpdateMoneyText();
            PlayerPrefs.SetInt("PlayerMoney", money);
        }
        else
        {
            Debug.Log("Not enough money to buy the item.");
        }
    }

    public void BuyItemBread()
    {
        if (money >= 5)
        {
            money -= 5;
            UpdateMoneyText();
            PlayerPrefs.SetInt("PlayerMoney", money);
        }
        else
        {
            Debug.Log("Not enough money to buy the item.");
        }
    }

    public void AddCash()
    {
        money += 10;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
    }

    public void ResetCash()
    {
        money = 0;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
    }

    public void PickupCash()
    {
        money = 50;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
    }

    private void UpdateMoneyText()
    {
        moneyText.text = " " + money.ToString();
    }
}
