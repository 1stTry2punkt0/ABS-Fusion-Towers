using UnityEngine;
using UnityEngine.EventSystems;

public class MapTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    //State of the tile
    public TileType tileType;
    //Gridcoordinates of the tile for pathfinding
    public Vector2Int gridPos;
    //-meshes for different states
    public Mesh[] tileMeshes;
    //Array of possible blocker elements
    [SerializeField] GameObject[] blockerObjs;
    //offset for different blockerobjects to place them above the tile
    [SerializeField] Vector2[] blockerOffsetY;
    //stored blockerobject
    public GameObject onTopObj;

    [Header("VisualFeedback")]
    [SerializeField] GameObject feedbackObj;
    [SerializeField] Material hoveredMat;
    [SerializeField] Material selectedMat;
    [SerializeField] Material invalidMat;

    //Method to change the tile type and set its mesh
    public void SetTileType(TileType newType, GameObject prefab = null)
    {
        tileType = newType;
        switch (tileType)
        {
            case TileType.free:
                GetComponent<MeshFilter>().mesh = tileMeshes[0];
                //dont make a blocker tile free again after it was a road
                if (onTopObj != prefab)
                {
                    onTopObj.SetActive(true);
                    tileType = TileType.blocked;
                }
                else
                {
                    onTopObj = null;
                }
                break;
            case TileType.road:
                GetComponent<MeshFilter>().mesh = tileMeshes[1];
                //disable blocker obj if it was a blocker
                if(onTopObj != null)
                {
                    onTopObj.SetActive(false);
                }
                break;
            case TileType.blocked:
                GetComponent<MeshFilter>().mesh = tileMeshes[0];
                //reactivate the blocker obj if it has one
                if (onTopObj != null)
                {
                    onTopObj.SetActive(true);
                }
                //or get a new random one
                else GetRandomBlocker();
                break;
            case TileType.building:
                onTopObj = Instantiate(prefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                onTopObj.transform.parent = transform;
                onTopObj.GetComponent<OnTopObj>().mapTile = this;
                break;
            case TileType.tower:
                onTopObj = Instantiate(prefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                onTopObj.transform.parent = transform;
                onTopObj.GetComponent<OnTopObj>().mapTile = this;
                break;

        }
    }

    public void ResetTile()
    {
        if (onTopObj != null)
        {
            Destroy(onTopObj);
            onTopObj = null;
        }
    }

    //Method to get a random blocker of the list
    private void GetRandomBlocker()
    {
        if (onTopObj == null)
        {
            int randomIndex = Random.Range(0, blockerObjs.Length);
            //adjust hight depending on blocker
            float offsetY = 0f;
            for (int i = 0; i < blockerOffsetY.Length; i++)
            {
                if (blockerOffsetY[i].x > randomIndex)
                {
                    offsetY = blockerOffsetY[i].y;
                    break;
                }
            }
            //Instantiate the blocker
            onTopObj = Instantiate(blockerObjs[randomIndex], transform.position + new Vector3(0,offsetY,0), Quaternion.identity);
            //set its parent
            onTopObj.transform.parent = transform;
        }
    }

    //Method to Handle Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tileType == TileType.free && !GameManager.instance.AmISelected(this))
        {
            feedbackObj.SetActive(true);
            feedbackObj.GetComponent<MeshRenderer>().material = hoveredMat;
        }
    }
    //Method to Handle Hover Exit
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tileType == TileType.free && !GameManager.instance.AmISelected(this))
        {
            feedbackObj.SetActive(false);
        }
    }
    //Method to Handle Click
    public void OnPointerDown(PointerEventData eventData)
    {
        if(tileType == TileType.blocked || tileType == TileType.road)
        {
            Invalid();
            return;
        }
        GameManager.instance.SelectTile(this);
        feedbackObj.SetActive(GameManager.instance.AmISelected(this));
        feedbackObj.GetComponent<MeshRenderer>().material = selectedMat;                
    }

    //Method to unselect the tile
    public void Unsecelt()
    {
        feedbackObj.SetActive(false);
    }
    //Method to show invalid feedback
    public void Invalid()
    {
        feedbackObj.SetActive(true);
        feedbackObj.GetComponent<MeshRenderer>().material = invalidMat;
        Invoke("Unsecelt", 1f);
    }
}