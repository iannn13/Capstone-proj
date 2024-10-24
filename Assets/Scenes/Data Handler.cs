using UnityEngine;
using UnityEngine.UI;

public class DataHandler : MonoBehaviour
{
    public Text moneyText;

    private int money;

    void Start()
    {
        money = PlayerPrefs.GetInt("PlayerMoney", 50);
        UpdateMoneyText();
    }

    public void BuyItem()
    {
        Debug.Log("Attempting to buy item. Current money: " + money);
        if (money >= 10)
        {
            money -= 10;
            UpdateMoneyText();
            PlayerPrefs.SetInt("PlayerMoney", money);
            Debug.Log("Item bought! Money left: " + money);
        }
        else
        {
            Debug.Log("Not enough money to buy the item. Current money: " + money);
        }
    }

    public void AddCash()
    {
        Debug.Log("Attempting to add cash. Current money: " + money);
        money += 10;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", money);
    }

    // Method to reset the money to 0
    public void ResetCash()
    {
        money = 0; // Reset cash to 0
        UpdateMoneyText(); // Update the money display
        PlayerPrefs.SetInt("PlayerMoney", money); // Save the updated value to PlayerPrefs
        Debug.Log("Cash reset to 0.");
    }

    private void UpdateMoneyText()
    {
        moneyText.text = " " + money.ToString();
    }
}
