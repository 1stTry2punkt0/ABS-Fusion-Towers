using UnityEngine;
using UnityEngine.UI;

public class ShowplaceMenu : MonoBehaviour
{
    public static ShowplaceMenu instance;

    [SerializeField] GameObject showplaceMenu;

    [SerializeField] Image icon;
    [SerializeField] TextSceneObject showplaceName;
    [SerializeField] TextSceneObject lore;
    [SerializeField] TextSceneObject effect;

    private Showplace currentShowplace;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenMenu(Showplace showplace)
    {
        Debug.Log("Opening showplace menu for " + showplace.showplaceSO.showplaceName.englishText);
        currentShowplace = showplace;
        icon.sprite = showplace.showplaceSO.icon;
        showplaceName.SetText(showplace.showplaceSO.showplaceName);
        lore.SetText(showplace.showplaceSO.lore);
        effect.SetText(showplace.showplaceSO.effect);
        showplaceMenu.SetActive(true);
    }

    public void CloseMenu(bool shouldUnselect = false)
    {
        if (shouldUnselect)
            GameManager.instance.Unselect();

        currentShowplace = null;

        showplaceMenu.SetActive(false);
    }

    public void SellShowplace()
    {
        currentShowplace.OnSell();
        CloseMenu();
    }
}
