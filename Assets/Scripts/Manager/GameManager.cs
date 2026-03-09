using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton instance to allow easy access to the GameManager from other scripts

    [SerializeField] MapManager mapManager; //Reference to the MapManager, set in inspector
    [SerializeField] BuildingMenu buildingMenu; //Reference to the BuildingMenu, set in inspector

    private MapTile selectedTile;

    [SerializeField] private GameObject[] buildingPrefabs; //Array of building prefabs corresponding to the buildings array
    [SerializeField] private GameObject[] towerPrefabs; //Array of available buildings that can be built on the tiles

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
            if (selectedTile == tile)
            {
                buildingMenu.CloseMenu();
                selectedTile = null;
                return;
            }
        }
        //Select the new tile
        selectedTile = tile;
        if (selectedTile.tileType == TileType.free)
            buildingMenu.OpenMenu(tile);
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
            }
            else
            {
                Invalid();
            }
        }
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
            mapManager.UpdatePath();
        }
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

    public void UpgradeTower(int index)
    {

    }
}
