using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton instance to allow easy access to the GameManager from other scripts

    [SerializeField] MapManager mapManager; //Reference to the MapManager, set in inspector
    [SerializeField] BuildingMenu buildingMenu; //Reference to the BuildingMenu, set in inspector
    [SerializeField] TowerMenu towerMenu; //Reference to the TowerMenu, set in inspector
    [SerializeField] RessourceManager ressourceManager; //Reference to the RessourceManager, set in inspector

    private MapTile selectedTile;

    [SerializeField] private GameObject[] buildingPrefabs; //Array of building prefabs corresponding to the buildings array
    [SerializeField] private GameObject[] towerPrefabs; //Array of available buildings that can be built on the tiles

    public int wave = 1;
    [SerializeField] private List<EnemySO> enemySOs; //List of enemy prefabs corresponding to the enemies array

    [SerializeField] private TextSceneObject messageObject;
    public TextSO[] invalidMessages;

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
    private void Start()
    {
        ressourceManager.SetDefault();
    }

    public void SelectTile(MapTile tile)
    {
        //Deselect the previously selected tile if there is one
        if (selectedTile != null)
        {
            selectedTile.Unsecelt();
            towerMenu.CloseMenu();
            if (selectedTile == tile)
            {
                selectedTile = null;
                return;
            }
        }
        //Select the new tile
        selectedTile = tile;
        if (selectedTile.tileType == TileType.free)
        {
            buildingMenu.OpenMenu(tile);
        }
        else
        {
            buildingMenu.CloseMenu();
        }
    }

    public bool AmISelected(MapTile tile)
    {
        return selectedTile == tile;
    }

    public void Unselect()
    {
        if (selectedTile!=null)
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
            bool hasRessource = ressourceManager.SpendRessource(buildingPrefabs[buildingIndex].GetComponent<Showplace>().showplaceSO.baseCost);
            if (suc && hasRessource)
            {
                //Call the BuildBuilding method of the selected tile with the given building index
                selectedTile.SetTileType(TileType.building, buildingPrefabs[buildingIndex]);
                mapManager.UpdatePath();
                SelectTile(selectedTile);
                buildingMenu.CloseMenu();
            }
            if (!suc)
            {
                Invalid(invalidMessages[0]);
            }
            if (!hasRessource)
            {
                Invalid(invalidMessages[1]);
            }
        }
    }


    public void SellBuilding(GameObject building)
    {
        if(selectedTile.tileType == TileType.building)
            mapManager.RemoveShowplace(selectedTile.gridPos);

        selectedTile.SetTileType(TileType.free, building);
        SelectTile(selectedTile);
        Destroy(building);
    }

    public void Invalid(TextSO message)
    {
        messageObject.SetText(message);
        messageObject.gameObject.SetActive(true);
        Invoke("CloseMessage", 3f);
    }

    private void CloseMessage()
    {
        messageObject.gameObject.SetActive(false);
    }

    public void BuildTower(int towerIndex)
    {
        if (selectedTile != null)
        {
            BaseTower tower = towerPrefabs[towerIndex].GetComponent<BaseTower>();
            if (ressourceManager.SpendRessource(tower.stats.baseCost))
            {
                //Call the BuildTower method of the selected tile with the given tower index
                selectedTile.SetTileType(TileType.tower, towerPrefabs[towerIndex]);
                buildingMenu.CloseMenu();
                TowerSelected(selectedTile.onTopObj.GetComponent<BaseTower>());
            }
            else
            {
                Invalid(invalidMessages[1]);
            }
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
        ressourceManager.SetDefault();
        wave = 0;
        UpgradeEnemys();

    }

    public void StartWave()
    {
        wave++;
        UpgradeEnemys();
    }

}
