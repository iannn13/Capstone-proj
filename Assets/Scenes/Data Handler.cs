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

    public void Addcash()
    {
        Debug.Log("Attempting to buy item. Current money: " + money); 
        if (money >= 0)
        {
            money += 10;
            UpdateMoneyText();
            PlayerPrefs.SetInt("PlayerMoney", money);
        }
    }



    private void UpdateMoneyText()
    {
        moneyText.text = " " + money.ToString();
    }
}
