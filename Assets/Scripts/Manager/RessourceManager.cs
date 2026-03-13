using UnityEngine;
using TMPro;

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager instance; //Singleton instance to allow easy access to the RessourceManager from other scripts

    [SerializeField] TMPro.TextMeshProUGUI goldText;
    [SerializeField] TMPro.TextMeshProUGUI faithText;
    [SerializeField] TMPro.TextMeshProUGUI healthText;

    public int gold { get; private set; }
    public int faith { get; private set; }
    private int maxHealth;
    public int health { get; private set; }

    private void Awake()
    {
        //make sure their is only one instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDefault()
    {
        int difficultyIndex = 0;
        if(SaveDataHolder.instance != null)
            difficultyIndex = SaveDataHolder.instance.loadedState.difficultyIndex;
        switch (difficultyIndex)
        {
            case 0:
                gold = 1000;
                faith = 1000;
                maxHealth = 150;
                health = maxHealth;
                break;
            case 1:
                gold = 500;
                faith = 500;
                maxHealth = 100;
                health = maxHealth;
                break;
            case 2:
                gold = 250;
                faith = 250;
                maxHealth = 50;
                health = maxHealth;
                break;
        }
        goldText.text = gold.ToString();
        faithText.text = faith.ToString();
        healthText.text = ((health / maxHealth) *100).ToString() + "%";
    }

    public bool SpendRessource(Cost cost)
    {
        switch (cost.ressourceType)
        {
            case RessourceType.gold:
                return UpdateGold(-cost.amount);
            case RessourceType.faith:
                return UpdateFaith(-cost.amount);
            case RessourceType.health:
                return UpdateHealth(-cost.amount);
            default:
                return false;
        }
    }

    public void GainRessource(Cost cost)
    {
        switch (cost.ressourceType)
        {
            case RessourceType.gold:
                UpdateGold(cost.amount);
                break;
            case RessourceType.faith:
                UpdateFaith(cost.amount);
                break;
            case RessourceType.health:
                UpdateHealth(cost.amount);
                break;
        }
    }

    private bool UpdateGold(int changeAmount)
    {
        if (changeAmount < 0 && gold < -changeAmount)
        {
            return false;
        }
        gold += changeAmount;
        goldText.text = gold.ToString();
        return true;
    }

    private bool UpdateFaith(int changeAmount)
    {
        if (changeAmount < 0 && faith < -changeAmount)
        {
            return false;
        }
        faith += changeAmount;
        faithText.text = faith.ToString();
        return true;
    }

    private bool UpdateHealth(int changeAmount)
    {
        if ( changeAmount < 0 && health < -changeAmount)
        {
            return false;
        }
        health += changeAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthText.text = ((health / maxHealth) *100).ToString() + "%";
        return true;
    }

}

public enum RessourceType
{
    gold,
    faith,
    health
}