using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Mark as not written while ABS, Part of 2nd grade of 2nd semester and highly "inpired" by the teachers given script
public static class DataManager
{
    private static readonly string FileName = "save.dat";
    private static readonly string FilePath = Path.Combine(Application.persistentDataPath, FileName);

    //Copie scriptableObject to SaveData
    private static SaveData GetData(SaveDataSO saveDataSO)
    {
        var saveData = new SaveData
        {
            language = saveDataSO.language,
            resolutionIndex = saveDataSO.resolutionIndex,
            fullscreen = saveDataSO.fullscreen,
            quality = saveDataSO.quality,
            volume = saveDataSO.volume,

            tutorial = saveDataSO.tutorial,
            mapIndex = saveDataSO.mapIndex,
            difficultyIndex = saveDataSO.difficultyIndex

        };
        return saveData;

    }

    public static void SaveData(SaveDataSO saveDataSO)
    {
        Debug.Log($"Saving data to: {FilePath}");
        var saveData = GetData(saveDataSO);
        //Get the saveData as Jason string
        string jason = JsonUtility.ToJson(saveData);
        //make the Jason not readable
        byte[] encrypted = SaveSystem.Encrypt(jason);
        //write that unreadable string in a file on the path we created upwards
        File.WriteAllBytes(FilePath, encrypted);
    }

    public static SaveDataSO/*FilledSO*/ LoadData(SaveDataSO saveDataSO/*emptySO*/)
    {
        //if their is no file at the path just take that empty saveDataSO and create the File nothing more to do
        if (!File.Exists(FilePath))
        {
            SaveData(saveDataSO);
            saveDataSO.language = 0;
            saveDataSO.resolutionIndex = -1;
            saveDataSO.fullscreen = true;
            saveDataSO.quality = QualitySettings.GetQualityLevel();
            saveDataSO.volume = 0.5f;

            saveDataSO.tutorial = true;
            saveDataSO.mapIndex = 1;
            saveDataSO.difficultyIndex = 0;
            return saveDataSO;
        }

        //if their is a file take that unreadable byte array  out of the file
        byte[] encrypted = File.ReadAllBytes(FilePath);
        //and make it readable again
        var decrypted = SaveSystem.Decrypt(encrypted);
        //make a SaveData
        SaveData saveData;
        //and try to write the jason as code in it
        try
        {
            saveData = JsonUtility.FromJson<SaveData>(decrypted);
        }
        //if that doesnt work
        catch (Exception ex) // Specify the type of exception to catch
        {
            //Get a debuglog
            Debug.LogError($"Error while loading save data: {ex.Message}");
            //and Take the empty dataSO
            saveData = GetData(saveDataSO);
            SaveData(saveDataSO);
        }

        //Create an Instance of a SO with the gotten Data
        var newSaveState = ScriptableObject.CreateInstance<SaveDataSO>();
        newSaveState.language = saveData.language;
        newSaveState.resolutionIndex = saveData.resolutionIndex;
        newSaveState.fullscreen = saveData.fullscreen;
        newSaveState.quality = saveData.quality;
        newSaveState.volume = saveData.volume;

        newSaveState.tutorial = saveData.tutorial;
        newSaveState.mapIndex = saveData.mapIndex;
        newSaveState.difficultyIndex = saveData.difficultyIndex;
        return newSaveState;
    }
}