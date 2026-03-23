using TMPro;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    [SerializeField] GameObject endscreen;

    [SerializeField] TextSO[] resultText;
    [SerializeField] TextSceneObject result;

    [SerializeField] GameObject stats;
    [SerializeField] TextSceneObject valueTowerName;
    [SerializeField] TextMeshProUGUI valueTowerValue;

    public void ShowEndscreen(bool won, BaseTower tower)
    {
        endscreen.SetActive(true);
        if (won)
            result.SetText(resultText[0]);
        else
            result.SetText(resultText[1]);

        if (tower != null)
        {
            valueTowerName.SetText(TowerMenu.instance.GetTowerName(tower.towerName));
            valueTowerValue.text = tower.dmgDealt.ToString();
        }
        else
        {
            stats.SetActive(false);
        }
    }
}
