using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject mapTile;//Prefab for all map tiles
    [SerializeField] Mesh[] tilemeshfree;//Meshes for free and road tiles
    private int mapSizeX = 38;//count of tiles in x direction
    private int mapSizeZ = 21;//count of tiles in z direction
    private float tileSize = 2f;//size of each tile, used for positioning
    
    private GameObject[,] grid;//Array grid to store references to all map tiles for easy access

    [Header("Road")]
    [SerializeField] Vector2 roadStart;//grid coordinates for the start of the road
    [SerializeField] Vector2 roadEnd;//grid coordinates for the end of the road
    [SerializeField] Vector2 goal;//grid coordinates for the goal
    public List<Vector2> showplaces = new List<Vector2>();//grid coordinates for showplaces
    private List<GameObject> roadObjs = new List<GameObject>();//List to store references to all road tiles for easy access

    private void Awake()
    {
        //Initialize the grid array based on the map size
        grid = new GameObject[mapSizeX, mapSizeZ];
    }

    //For testing start Everything in Start
    void Start()
    {
        GenerateMap();
        UpdatePath();
    }

    //Method to Generate the base map and store the references
    private void GenerateMap()
    {
        //for each combination of x and z coordinates
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                //switch colors based on position
                int tileColor = (x + z) % 2;
                //calculate position
                Vector3 position = new Vector3(x * tileSize - mapSizeX +1, 0, z * tileSize - mapSizeZ +1.1f);
                //Instantiate the tile
                GameObject tile = Instantiate(mapTile, position, Quaternion.identity);
                //set parent and name for nice looking hirarchy
                tile.transform.parent = transform;
                tile.name = $"{x} X {z}";

                //store the tile in the grid array
                grid[x, z] = tile;

                //Get the tile script
                MapTile tilebehavior = tile.GetComponent<MapTile>();
                //set its default mesh
                tilebehavior.tileMeshes[0] = tilemeshfree[tileColor];
                //Add some blocked tiles on sides
                if ((z < 8 || z > mapSizeZ - 9) && x < mapSizeX - 4)
                {
                    float randomValue = Random.Range(0f, 100f);
                    //Higher chance to become blocked if more distance to the middle
                    if (z < 8)
                    {
                        if (15 - z * 2 > randomValue) tilebehavior.SetTileType(TileType.blocked);
                        else tilebehavior.SetTileType(TileType.free);
                    }
                    else
                    {
                        if (15 - (mapSizeZ - 1 - z) * 2 > randomValue) tilebehavior.SetTileType(TileType.blocked);
                        else tilebehavior.SetTileType(TileType.free);
                    }
                }
                else
                {
                    tilebehavior.SetTileType(TileType.free);
                }
                //make sure key road tiles are road tiles
                if(x == roadStart.x && z == roadStart.y || x == roadEnd.x && z == roadEnd.y || x == goal.x && z == goal.y)
                {
                    tilebehavior.SetTileType(TileType.road);
                }
            }
        }
    }

    //Method to update the path
    public void UpdatePath()
    {
        //clear old path
        foreach(GameObject road in roadObjs)
        {
            //reset to free tiles
            road.GetComponent<MapTile>().SetTileType(TileType.free);
        }
        roadObjs.Clear();

        //get the new path corners
        List<Vector2> pathCorners = CalculatePath();
        //go frough the list
        for (int i = 0; i < pathCorners.Count - 2; i++)
        {
            //store start corner and end corner of the line
            Vector2 start = pathCorners[i];
            Vector2 end = pathCorners[i + 1];
            //change tiles on the line to road tiles
            if (start.x == end.x)
            {
                int x = (int)start.x;
                for (int z = Mathf.Min((int)start.y, (int)end.y); z <= Mathf.Max((int)start.y, (int)end.y); z++)
                {
                    grid[x, z].GetComponent<MapTile>().SetTileType(TileType.road);
                    roadObjs.Add(grid[x, z]);
                }
            }
            else if (start.y == end.y)
            {
                int z = (int)start.y;
                for (int x = Mathf.Min((int)start.x, (int)end.x); x <= Mathf.Max((int)start.x, (int)end.x); x++)
                {
                    grid[x, z].GetComponent<MapTile>().SetTileType(TileType.road);
                    roadObjs.Add(grid[x, z]);
                }
            }
        }
    }

    //Method to calculate the path depending on showplaces
    public List<Vector2> CalculatePath()
    {
        //create the returning list
        List<Vector2> pathCorners = new List<Vector2>();
        //add the start
        pathCorners.Add(roadStart);
        //copy showplaces to keep track of not used showplaces
        List<Vector2> showplacesCopy = new List<Vector2>(showplaces);
        //create orderd list of showplaces
        List<Vector2> showplaceOrder = new List<Vector2>();
        //store last position
        Vector2 current = roadStart;
        //As long as there ist stile a tile in the copy list
        while (showplacesCopy.Count > 0)
        {
            //Store closest position of the next showplace .Set one position as default closest
            Vector2 closest = showplacesCopy[0];
            //Store the value of distance in x direction
            float distX = closest.x - current.x;
            //check every last showplaces if its closer
            foreach (Vector2 s in showplacesCopy)
            {
                float d = s.x - current.x;
                if (d < distX)
                {
                    //if closer store it
                    distX = d;
                    closest = s;
                }
                if(d== distX)
                {
                    //if equal check y distance
                    if (s.y < closest.y)
                    {
                        closest = s;
                    }
                }
            }
            //add the closest to orderd list
            showplaceOrder.Add(closest);
            //remove from list of remaining showplaces
            showplacesCopy.Remove(closest);
            //store the new last position
            current = closest;
        }
        //check each showplace for its best corner for the path
        foreach (Vector2 showplace in showplaceOrder)
        {
            //store the last added path corner
            Vector2 lastCorner = pathCorners[pathCorners.Count - 1];
            //get all posible corners next to the showplace
            Vector2[] neighbors = new Vector2[]
            {
            showplace + new Vector2( 1, 1),
            showplace + new Vector2(-1, 1),
            showplace + new Vector2( 1,-1),
            showplace + new Vector2(-1,-1)
            };
            //store the closest to last corner, set one as default
            Vector2 bestNeighbor = neighbors[0];
            //store the best distance
            float bestDist = Vector2.Distance(lastCorner, bestNeighbor);
            //Check each one if its closer than the stored
            foreach (var n in neighbors)
            {
                float d = Vector2.Distance(lastCorner, n);
                if (d < bestDist)
                {
                    //if so store the new data
                    bestDist = d;
                    bestNeighbor = n;
                }
            }
            //Add another corner if needed to connect the last one and the showplace corner
            if (lastCorner.x != bestNeighbor.x)
            {
                Vector2 horizontalCorner = new Vector2(bestNeighbor.x, lastCorner.y);
                if (horizontalCorner != lastCorner)
                    pathCorners.Add(horizontalCorner);
            }

            if (lastCorner.y != bestNeighbor.y)
            {
                Vector2 verticalCorner = new Vector2(bestNeighbor.x, bestNeighbor.y);
                if (verticalCorner != pathCorners[pathCorners.Count - 1])
                    pathCorners.Add(verticalCorner);
            }
        }
        //Add another corner to connect to the end if needed
        if (pathCorners[pathCorners.Count - 1].x != roadEnd.x)
        {
            Vector2 lastCorner = pathCorners[pathCorners.Count - 1];
            Vector2 verticalCorner = new Vector2(roadEnd.x, lastCorner.y);
            if (verticalCorner != lastCorner)
                pathCorners.Add(verticalCorner);
        }
        if (pathCorners[pathCorners.Count - 1].y != roadEnd.y)
        {
            Vector2 lastCorner = pathCorners[pathCorners.Count - 1];
            Vector2 horizontalCorner = new Vector2(lastCorner.x, roadEnd.y);
            if (horizontalCorner != lastCorner)
                pathCorners.Add(horizontalCorner);
        }
        //add end and goal to corners
        pathCorners.Add(roadEnd);
        pathCorners.Add(goal);
        //return the list
        return pathCorners;
    }

}

//enum of states of a tile
public enum TileType
{
    road,
    free,
    blocked
}