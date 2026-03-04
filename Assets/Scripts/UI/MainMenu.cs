using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private List<GameObject> MenuPanel;

    private void Start()
    {
        // Ensure only the main menu panel is active at the start
        ShowMenu(0);
    }

    //Method to switch between shown menu panels based on the index provided
    public void ShowMenu(int index) //0 = Main Menu, 1 = Start Game, 2 = Options
    {
        //Hide all menu panels first
        foreach (var panel in MenuPanel)
        {
            panel.SetActive(false);
        }
        //Show the selected menu panel based on the index
        MenuPanel[index].SetActive(true);
    }

    //Method to quit the application
    public void Quit()
    {
        SaveDataHolder.instance.SaveData(); //Save the current game state before quitting
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    //Load the last saved game state if time for it
    public void Continue()
    {
        // Implement logic to continue the game, such as loading the last saved state or scene
        Debug.Log("Continue button clicked. Implement continue logic here.");
    }
}
