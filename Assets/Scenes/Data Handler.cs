using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; } // Singleton instance
    public int playerCoins = 0;
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
            isNewGame = false; 
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
        money += 1000;
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

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
        Debug.Log($"Added {amount} money. Total: {money}");
    }

    public void SubtractMoney(int amount)
{
    if (money >= 25)
    {
        money -= 25;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
        Debug.Log($"Subtracted {amount} money. Remaining: {money}");
    }
    else
    {
        Debug.Log("Not enough money to subtract.");
    }
}

     public void AddMoneyLola()
    {
        money += 25;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
    }
    public void MinusMoneyRemmy()
    {
        money -= 10;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
    }

}
