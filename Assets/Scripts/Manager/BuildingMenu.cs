using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject[] buildingMenus;//Reference to the building menu UI, set in inspector

    

    public void OpenMenu(MapTile tile)
    {
        menuPanel.SetActive(true);
        SwitchMenu(0);
    }

    public void CloseMenu()
    {
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

    public void UpgradeTower(int upgradeIndex)
    {
        GameManager.instance.UpgradeTower(upgradeIndex);
    }
}
