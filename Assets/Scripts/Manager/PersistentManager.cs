using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    //Make this gameobject and its childs persistent across scene loads
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
