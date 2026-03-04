using System;
using UnityEngine;
[Serializable]
public class SaveData
{
    //Settings
    public int language;//index of the language
    public int resolutionIndex;//Selected screen resolution
    public bool fullscreen;//Whether the game is in fullscreen mode
    public int quality;//Index of the selected quality level
    public float volume;//~0 to 1

    //Game
    public bool tutorial;
    public int mapIndex;
    public int difficultyIndex;
}