using UnityEngine;

public class OnTopObj : MonoBehaviour
{
    public GameObject objOptionMenu;
    public MapTile mapTile;

    public void OnSell()
    {
        GameManager.instance.SellBuilding(gameObject);
    }

    public void OnClose()
    {
        GameManager.instance.SelectTile(mapTile);
        objOptionMenu.SetActive(false);
    }

    public void OnSelect()
    {

    }
}
