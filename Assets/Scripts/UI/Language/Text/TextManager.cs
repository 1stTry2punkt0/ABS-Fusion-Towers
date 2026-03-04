using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager instance; //Singleton instance to allow easy access to the TextManager from other scripts
    public Language currentLanguage = Language.English; //Variable to store the current language, default is English

    //Automatic LanguageUpdate
    public static event System.Action<Language> OnLanguageChanged;

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

    //Method to update all fixed text objects when the language changes
    public void UpdateLanguage(Language language)
    {
        currentLanguage = language;
        OnLanguageChanged?.Invoke(currentLanguage);
    }
}

//Enum for languages
public enum Language
{
    English,
    German
}