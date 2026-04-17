using TMPro;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject[] buildingMenus;//Reference to the building menu UI, set in inspector
    [SerializeField] TextMeshProUGUI[] towerCost;
    [SerializeField] TowerStatSO[] towerStats;

    private void Start()
    {
        menuPanel.SetActive(false);
        for (int i = 0; i < towerCost.Length; i++)
        {
            towerCost[i].text = towerStats[i].baseCost.amount.ToString();
        }
    }


    public void OpenMenu(MapTile tile)
    {
        menuPanel.SetActive(true);
        switch (GameManager.instance.gameState)
        {
            case GameState.RoadBuilding:
                SwitchMenu(0);
                break;
            case GameState.Preparing:
            case GameState.Fighting:
                SwitchMenu(1);
                break;
            default:
                Debug.Log("not supposed to open building menu in this gameState");
                break;
        }
    }

    public void CloseMenu(bool shouldUnselect = false)
    {
        if(shouldUnselect)
            GameManager.instance.Unselect();
        menuPanel.SetActive(false);
    }

    public void SwitchMenu(int index)
    {
        for (int i = 0; i < buildingMenus.Length; i++)
        {
            buildingMenus[i].SetActive(i == index);
        }
    }

    public void BuildBuilding(int buildingIndex)
    {
        GameManager.instance.BuildBuilding(buildingIndex);
    }

    public void BuildTower(int towerIndex)
    {
        GameManager.instance.BuildTower(towerIndex);
    }

}
