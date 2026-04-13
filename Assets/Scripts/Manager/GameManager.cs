using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton instance to allow easy access to the GameManager from other scripts

    public GameState gameState;

    [SerializeField] MapManager mapManager; //Reference to the MapManager, set in inspector
    [SerializeField] BuildingMenu buildingMenu; //Reference to the BuildingMenu, set in inspector
    [SerializeField] TowerMenu towerMenu; //Reference to the TowerMenu, set in inspector
    [SerializeField] RessourceManager ressourceManager; //Reference to the RessourceManager, set in inspector
    [SerializeField] Endscreen endscreen;

    private MapTile selectedTile;

    [SerializeField] private GameObject[] buildingPrefabs; //Array of building prefabs corresponding to the buildings array
    [SerializeField] private GameObject[] towerPrefabs; //Array of available buildings that can be built on the tiles
    public List<BaseTower> towerList;

    public BaseTower weaponFusion;
    public ElementalTower elementalFusion;
    [SerializeField] GameObject fusionOverlay;

    public int wave => WaveManager.instance.currentWave;
    [SerializeField] private List<EnemySO> enemySOs; //List of enemy prefabs corresponding to the enemies array

    [SerializeField] private TextSceneObject messageObject;
    public TextSO[] invalidMessages;

    public LayerMask enemyLayer;

    public float timeScale = 1f;

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
        ResetGame();
        SwitchTimeScale(SaveDataHolder.instance.loadedState.speedup);
        ApplyTimeScale();
    }

    public void SwitchTimeScale(bool speedup)
    {
        if (speedup)
        {
            this.timeScale = 2.5f;
        }
        else
        {
            this.timeScale = 1f;
        }
        SaveDataHolder.instance.loadedState.speedup = speedup;
    }

    public void ApplyTimeScale()
    {
        Time.timeScale = this.timeScale;
    }


    public void ResetGame()
    {
        mapManager.ResetMap();
        ressourceManager.SetDefault();
        WaveManager.instance.NewGame();
        UpgradeEnemys();
        gameState = GameState.RoadBuilding;
    }

    public void EndGame(bool won)
    {
        if (gameState == GameState.Ended) return;
        gameState = GameState.Ended;
        BaseTower valueTower = null;
        if (towerList.Count > 0)
        {
            //sort tower list by dmgdealt
            valueTower = towerList.OrderByDescending(t => t.dmgDealt).First();
        }
        endscreen.ShowEndscreen(won, valueTower);
    }

    public void SelectTile(MapTile tile)
    {
        //Deselect the previously selected tile if there is one
        if (selectedTile != null)
        {
            selectedTile.Unselect();
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
        if(gameState != GameState.RoadBuilding)
        {
            Invalid(invalidMessages[2]);
            return;
        }
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
        if (gameState == GameState.RoadBuilding)
            gameState = GameState.Preparing;
        
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

    public void Fusion(BaseTower tower)
    {
        gameState = GameState.Fusing;
        fusionOverlay.SetActive(true);
        tower.mapTile.HighlightTile(true);
        if (tower is ElementalTower)
        {
            if (elementalFusion != null)
            {
                Invalid(invalidMessages[3]);
                return;
            }
            elementalFusion = (ElementalTower)tower;
        }
        else
        {
            if (weaponFusion != null)
            {
                Invalid(invalidMessages[3]);
                return;
            }
            weaponFusion = tower;
        }
        if(weaponFusion != null && elementalFusion != null)
        {
            if (ressourceManager.SpendRessource(weaponFusion.stats.fusionCost))
            {
                //Fuse the towers
                weaponFusion.Fuse(elementalFusion);
                //Destroy the elemental tower
                elementalFusion.Fuse(elementalFusion);
                //Reset the fusion variables
                CancelFusion();
            }
            else
            {
                Invalid(invalidMessages[1]);
            }
        }
    }

    public void CancelFusion()
    {
        if (weaponFusion != null)
        {
            weaponFusion.mapTile.HighlightTile(false);
            weaponFusion = null;
        }
        if (elementalFusion != null)
        {
            elementalFusion.mapTile.HighlightTile(false);
            elementalFusion = null;
        }
        gameState = GameState.Fighting;
        fusionOverlay.SetActive(false);
        Unselect();
    }

    public void TowerSelected(BaseTower tower)
    {
        tower.OnSelect();
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
            enemy.SetLevel(wave + SaveDataHolder.instance.loadedState.difficultyIndex * 2);
        }
    }
}

public enum GameState
{
    RoadBuilding,
    Preparing,
    Fighting,
    Ended,
    Fusing
}