using UnityEngine;
using UnityEngine.EventSystems;

public class MapTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    //State of the tile
    public TileType tileType;
    //-meshes for different states
    public Mesh[] tileMeshes;
    //Array of possible blocker elements
    [SerializeField] GameObject[] blockerObjs;
    //offset for different blockerobjects to place them above the tile
    [SerializeField] Vector2[] blockerOffsetY;
    //stored blockerobject
    private GameObject blockerObj;

    [Header("VisualFeedback")]
    [SerializeField] GameObject feedbackObj;
    [SerializeField] Material hoveredMat;
    [SerializeField] Material selectedMat;
    private bool isSelected = false;

    //Method to change the tile type and set its mesh
    public void SetTileType(TileType newType)
    {
        tileType = newType;
        switch (tileType)
        {
            case TileType.free:
                GetComponent<MeshFilter>().mesh = tileMeshes[0];
                //dont make a blocker tile free again after it was a road
                if (blockerObj != null)
                {
                    blockerObj.SetActive(true);
                    tileType = TileType.blocked;
                }
                break;
            case TileType.road:
                GetComponent<MeshFilter>().mesh = tileMeshes[1];
                //disable blocker obj if it was a blocker
                if(blockerObj != null)
                {
                    blockerObj.SetActive(false);
                }
                break;
            case TileType.blocked:
                GetComponent<MeshFilter>().mesh = tileMeshes[0];
                //reactivate the blocker obj if it has one
                if (blockerObj != null)
                {
                    blockerObj.SetActive(true);
                }
                //or get a new random one
                else GetRandomBlocker();
                break;
        }
    }
    //Method to get a random blocker of the list
    private void GetRandomBlocker()
    {
        if (blockerObj == null)
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
            blockerObj = Instantiate(blockerObjs[randomIndex], transform.position + new Vector3(0,offsetY,0), Quaternion.identity);
            //set its parent
            blockerObj.transform.parent = transform;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tileType == TileType.free && !isSelected)
        {
            feedbackObj.SetActive(true);
            feedbackObj.GetComponent<MeshRenderer>().material = hoveredMat;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tileType == TileType.free && !isSelected)
        {
            feedbackObj.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (tileType == TileType.free)
        {
            isSelected = !isSelected;
            feedbackObj.SetActive(isSelected);
            feedbackObj.GetComponent<MeshRenderer>().material = selectedMat;
        }
    }

    public void Unsecelt()
    {
        isSelected = false;
        feedbackObj.SetActive(false);
    }
}