using UnityEngine;

public class SaveDataSO : ScriptableObject
{
    //Settings
    public int language;//index of the language
    public Resolution resolution;//Selected screen resolution
    public bool fullscreen;//Whether the game is in fullscreen mode
    public int quality;//Index of the selected quality level
    public float volume;//~0 to 1

    //Game
    public bool tutorial;
    public int mapIndex;
    public int difficultyIndex;
}