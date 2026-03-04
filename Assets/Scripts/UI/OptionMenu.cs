using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;

    [Header("Fullscreen")]
    [SerializeField] Toggle fullscreenToggle;

    private Resolution[] resolutions; //Array to hold the available screen resolutions

    void Start()
    {
        //Get the available screen resolutions
        resolutions = Screen.resolutions;
        //Clear the existing options in the resolution dropdown
        resolutionDropdown.ClearOptions();
        //Add the available resolutions to the dropdown options
        resolutionDropdown.AddOptions(GetResolutionOptions());
        //Set the dropdown to the current resolution index
        resolutionDropdown.value = SaveDataHolder.instance.loadedState.resolutionIndex; 
        resolutionDropdown.RefreshShownValue(); //Refresh the dropdown to show the current resolution

        //Set the quality dropdown to the current quality level
        qualityDropdown.value = SaveDataHolder.instance.loadedState.quality;
        qualityDropdown.RefreshShownValue(); //Refresh the dropdown to show the current quality level

        //Set the language dropdown to the current language
        languageDropdown.value = (int)TextManager.instance.currentLanguage;
        languageDropdown.RefreshShownValue(); //Refresh the dropdown to show the current language

        //Set the fullscreen toggle to the current fullscreen mode
        fullscreenToggle.isOn = SaveDataHolder.instance.loadedState.fullscreen;
    }

    //Method to get the available screen resolutions and format them as options for the dropdown
    private List<string> GetResolutionOptions()
    {
        //Create a list to hold the formatted resolution options
        List<string> options = new List<string>();
        //Loop through the available resolutions and format them as "width x height" for the dropdown options
        foreach (var resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            options.Add(option);
            //Check if the current resolution matches the screen's current resolution and store its index
            if (SaveDataHolder.instance.loadedState.resolutionIndex == -1 &&
                resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                SaveDataHolder.instance.loadedState.resolutionIndex = options.Count - 1; // Store the index of the current resolution
            }
        }
        //return the list of formatted resolution options for the dropdown
        return options;
    }

    //Method to set the screen resolution based on the selected index from the resolution dropdown
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveDataHolder.instance.loadedState.resolutionIndex = resolutionIndex; //Update the loaded state with the new resolution
    }

    //Method to set the quality level based on the selected index from the quality dropdown
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        SaveDataHolder.instance.loadedState.quality = qualityIndex; //Update the loaded state with the new quality level
    }

    //Method to set the language based on the selected index from the language dropdown
    public void SetLanguage(int languageIndex)
    {
        TextManager.instance.UpdateLanguage((Language)languageIndex);
        SaveDataHolder.instance.loadedState.language = languageIndex;
    }

    //Method to set the fullscreen mode based on the toggle value
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveDataHolder.instance.loadedState.fullscreen = isFullscreen; //Update the loaded state with the new fullscreen mode
    }

    //Method to set the audio volume based on the slider value, converting it to a logarithmic scale for better audio control
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        volumeSlider.value = volume; //Update the slider value to reflect the current volume
        SaveDataHolder.instance.loadedState.volume = volume; //Update the loaded state with the new volume
    }

}
