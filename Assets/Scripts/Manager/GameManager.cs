using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton instance to allow easy access to the GameManager from other scripts

    [SerializeField] MapManager mapManager; //Reference to the MapManager, set in inspector
    [SerializeField] BuildingMenu buildingMenu; //Reference to the BuildingMenu, set in inspector
    [SerializeField] TowerMenu towerMenu; //Reference to the TowerMenu, set in inspector

    private MapTile selectedTile;

    [SerializeField] private GameObject[] buildingPrefabs; //Array of building prefabs corresponding to the buildings array
    [SerializeField] private GameObject[] towerPrefabs; //Array of available buildings that can be built on the tiles

    public int wave = 1;
    [SerializeField] private List<EnemySO> enemySOs; //List of enemy prefabs corresponding to the enemies array

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

    public void SelectTile(MapTile tile)
    {
        //Deselect the previously selected tile if there is one
        if (selectedTile != null)
        {
            selectedTile.Unsecelt();
            buildingMenu.CloseMenu();
            towerMenu.OnClose();
            if (selectedTile == tile)
            {
                selectedTile = null;
                return;
            }
        }
        //Select the new tile
        selectedTile = tile;
        switch (selectedTile.tileType)
        {
            case TileType.free:
                buildingMenu.OpenMenu(tile);
                break;
            case TileType.building:
                BuildingSelected();
                break;
            case TileType.tower:
                TowerSelected(tile.onTopObj.GetComponent<BaseTower>());
                break;
        }
    }

    public bool AmISelected(MapTile tile)
    {
        return selectedTile == tile;
    }

    public void Unselect()
    {
        SelectTile(selectedTile);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return mapManager.GetWorldPosition(gridPos);
    }

    public void BuildBuilding(int buildingIndex)
    {
        if (selectedTile != null)
        {
            bool suc = mapManager.AddShowplace(selectedTile.gridPos);
            if (suc)
            {
                //Call the BuildBuilding method of the selected tile with the given building index
                selectedTile.SetTileType(TileType.building, buildingPrefabs[buildingIndex]);
                mapManager.UpdatePath();
                SelectTile(selectedTile);
                buildingMenu.CloseMenu();
            }
            else
            {
                Invalid();
            }
        }
    }

    private void BuildingSelected()
    {
        selectedTile.onTopObj.GetComponent<OnTopObj>().objOptionMenu.SetActive(true);
    }

    public void SellBuilding(GameObject building)
    {
        selectedTile.SetTileType(TileType.free, building);
        mapManager.RemoveShowplace(selectedTile.gridPos);
        SelectTile(selectedTile);
        Destroy(building);
    }

    private void Invalid()
    {

    }

    public void BuildTower(int towerIndex)
    {
        if (selectedTile != null)
        {
            //Call the BuildTower method of the selected tile with the given tower index
            selectedTile.SetTileType(TileType.tower, towerPrefabs[towerIndex]);
            buildingMenu.CloseMenu();
            TowerSelected(selectedTile.onTopObj.GetComponent<BaseTower>());
        }
    }

    public void TowerSelected(BaseTower tower)
    {
        towerMenu.OpenMenu(tower);
    }

    public void Demolish()
    {
        if (selectedTile != null)
        {
            //Call the Demolish method of the selected tile
            selectedTile.SetTileType(TileType.free);
            mapManager.UpdatePath();
        }
    }

    public void UpgradeEnemys()
    {
        foreach (EnemySO enemy in enemySOs)
        {
            enemy.SetLevel(wave);
        }
    }

    public void ResetGame()
    {
        mapManager.ResetMap();
        wave = 0;
        UpgradeEnemys();
    }

    public void StartWave()
    {
        wave++;
        UpgradeEnemys();
    }

}
