using UnityEngine;
using TMPro;

public class TextSceneObject : MonoBehaviour
{
    [Header("Text for different languages")]
    [SerializeField] string englishText;
    [SerializeField] string germanText;
    
    private TMP_Text textObj; //Reference to the text object in scene

    private void Awake()
    {
        //Get the text component from the game object, if it is not assigned in the inspector
        if (textObj == null)
        {
            textObj = GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        //Subscribe to the language change event when the object is enabled 
        TextManager.OnLanguageChanged += UpdateText;
        //update the text to the current language
        if (TextManager.instance != null)
        {
            UpdateText(TextManager.instance.currentLanguage);
        }
    }

    private void OnDisable()
    {
        //Unsubscribe from the language change event when the object is disabled
        TextManager.OnLanguageChanged -= UpdateText;
    }

    //Methode to update the text depending on the language
    public void UpdateText(Language language)
    {
        switch (language)
        {
            case Language.English:
                textObj.text = englishText;
                break;
            case Language.German:
                textObj.text = germanText;
                break;
            default:
                textObj.text = "Text not found";
                break;
        }
    }
}
