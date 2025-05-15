using UnityEngine;
using TMPro; // namespace para TextMeshPro

public class MoneyManager : MonoBehaviour
{
    public int money = 500; // dinheiro inicial
    public TextMeshProUGUI moneyText; // referência ao texto no Canvas

    void Start()
    {
        UpdateMoneyText();
    }

    // Método para atualizar o texto na UI
    public void UpdateMoneyText()
    {
        moneyText.text = money.ToString();
    }

    // Método para adicionar dinheiro
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    // Método para gastar dinheiro, retorna true se possível gastar
    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();
            return true;
        }
        return false;
    }
}
