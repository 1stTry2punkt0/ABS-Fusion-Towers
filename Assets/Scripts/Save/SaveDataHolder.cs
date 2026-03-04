using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataHolder : MonoBehaviour
{
    public static SaveDataHolder instance; //Singleton instance to allow easy access to the SaveDataHolder from other scripts

    [Header("MenueReferences")]
    private OptionMenu optionMenu;

    public SaveDataSO loadedState;

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


    void Start()
    {
        optionMenu = FindFirstObjectByType<OptionMenu>();
        LoadData();
    }

    public void LoadData()
    {
        //load data
        var saveState = ScriptableObject.CreateInstance<SaveDataSO>();
        loadedState = DataManager.LoadData(saveState);
        //set everything depending on loaded data
        TextManager.instance.UpdateLanguage((Language)loadedState.language);
        optionMenu.SetVolume(loadedState.volume);
    }

    public void SaveData()
    {
        DataManager.SaveData(loadedState);
    }
}