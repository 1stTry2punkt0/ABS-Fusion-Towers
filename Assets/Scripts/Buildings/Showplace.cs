using UnityEngine;

public class Showplace : MonoBehaviour, IOnTopObj
{
    public ShowplaceSO showplaceSO;
    public MapTile mapTile { get; set; }
    public Cost sellValue { get; set; }

    public void OnSell()
    {
        RessourceManager.instance.GainRessource(sellValue);
        GameManager.instance.SellBuilding(gameObject);
    }

    public void OnSelect()
    {
        ShowplaceMenu.instance.OpenMenu(this);
    }

    public void DeSelect()
    {
        ShowplaceMenu.instance.CloseMenu();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sellValue = showplaceSO.baseCost;
    }
}
