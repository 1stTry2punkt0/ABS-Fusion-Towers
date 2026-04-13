using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] Sprite[] mapPreviews; //Array to hold the map preview images for each level
    [SerializeField] Image mapPreviewImage; //Reference to the UI Image component that will display the map preview
    [SerializeField] TMP_Dropdown levelDropdown; //Reference to the dropdown that allows the player to select a level
    [SerializeField] TMP_Dropdown difficultyDropdown; //Reference to the dropdown that allows the player to select a difficulty level

    private void Start()
    {
        //Initialize the level dropdown to the current map index stored in the loaded state (subtract 1 to match dropdown index)
        levelDropdown.value = SaveDataHolder.instance.loadedState.mapIndex - 1;
        levelDropdown.RefreshShownValue(); //Refresh the dropdown to show the current level selection
        //Initialize the difficulty dropdown to the current difficulty index stored in the loaded state
        difficultyDropdown.value = SaveDataHolder.instance.loadedState.difficultyIndex;
        difficultyDropdown.RefreshShownValue(); //Refresh the dropdown to show the current difficulty selection
        //Update the map preview image based on the initial level selection
        mapPreviewImage.sprite = mapPreviews[SaveDataHolder.instance.loadedState.mapIndex - 1];
    }

    public void OnLevelSelected(int index)
    {
        //Update the map preview image based on the selected level index
        mapPreviewImage.sprite = mapPreviews[index]; //Set the map preview image to the corresponding sprite from the array based on the selected index
        SaveDataHolder.instance.loadedState.mapIndex = index + 1; //Update the loaded state with the selected map index (add 1 to match sceneindex)
    }

    public void OnDifficultySelected(int index)
    {
        SaveDataHolder.instance.loadedState.difficultyIndex = index; //Update the loaded state with the selected difficulty index
        Debug.Log(index);
    }

    public void StartGame()
    {
        SaveDataHolder.instance.SaveData();
        SceneManager.LoadScene(SaveDataHolder.instance.loadedState.mapIndex); //Load the selected level scene based on the map index stored in the loaded state
    }
}
