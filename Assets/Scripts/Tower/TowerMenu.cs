using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerMenu : MonoBehaviour
{
    public static TowerMenu instance;
    [HideInInspector]
    public int gridSizeX = 19;

    [SerializeField] GameObject menuPanel;
    [SerializeField] RectTransform panelRectTransform;

    [SerializeField] Image icon;
    [SerializeField] TextSceneObject nameText;
    [SerializeField] TextMeshProUGUI dmgDealt;
    [SerializeField] TMPro.TextMeshProUGUI level;
    [SerializeField] Image[] upgradeProgress;
    [SerializeField] TextSceneObject[] upgradeProgressText;
    [SerializeField] TowerUI[] towerUI;
    [SerializeField] GameObject FusionUpgrade;

    [HideInInspector]
    public BaseTower selectedTower;
    private int currentTargetSelectionIndex = 0;
    [SerializeField] TextSceneObject targetSelectionText;
    [SerializeField] TextSO[] targetSelectionNames;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    public void UpdateDmg()
    {
        dmgDealt.text = "Dmg: " + selectedTower.dmgDealt.ToString();
    }

    public void OpenMenu(BaseTower tower)
    {
        selectedTower = tower;
        float halfwidth = panelRectTransform.rect.width / 2;
        if(tower.mapTile.gridPos.x > gridSizeX)
        {
            panelRectTransform.anchoredPosition = new Vector2(halfwidth, panelRectTransform.anchoredPosition.y);
            panelRectTransform.anchorMin = new Vector2(0f, 0f);
            panelRectTransform.anchorMax = new Vector2(0f, 1f);
            panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            panelRectTransform.anchoredPosition = new Vector2(-halfwidth, panelRectTransform.anchoredPosition.y);
            panelRectTransform.anchorMin = new Vector2(1f, 0f);
            panelRectTransform.anchorMax = new Vector2(1f, 1f);
            panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        menuPanel.SetActive(true);
        UpdateMenu();
    }

    private void UpdateMenu()
    {
        TowerUI ui = Array.Find(towerUI, t => t.towerType == selectedTower.towerName);
        if (ui != null)
        {
            icon.sprite = ui.icon;
            nameText.SetText(ui.nameText);
            UpdateDmg();
            currentTargetSelectionIndex = (int)selectedTower.targetSelectionType;
            ChangeTargetSelection(0); // Update the target selection text
            UpdateUpgrades();
        }
    }

    public TextSO GetTowerName(TowerType type)
    {
        return Array.Find(towerUI, t => (t.towerType == type)).nameText;
    }

    private void UpdateUpgrades()
    {
        //Debug.Log("Updating upgrades for " + selectedTower.name + " With Level: " + selectedTower.level);
        level.text = "Level: " + selectedTower.level;
        for (int i = 0; i < upgradeProgress.Length; i++)
        {
            if (i < selectedTower.optionlvl.Length)
            {
                upgradeProgress[i].fillAmount = (float)selectedTower.optionlvl[i] / 5f;
            }
            else
            {
                upgradeProgress[i].fillAmount = 0;
            }
            upgradeProgressText[i].SetText(selectedTower.stats.upgradeOption[i].upgradeName);
        }
        if(selectedTower.level > 5)
        {
            FusionUpgrade.SetActive(true);
        }
        else
        {
            FusionUpgrade.SetActive(false);
        }
    }

    public void ChangeTargetSelection(int direction)
    {
        currentTargetSelectionIndex += direction;
        if (currentTargetSelectionIndex < 0)
            currentTargetSelectionIndex = Enum.GetNames(typeof(targetSelection)).Length - 1;
        else if (currentTargetSelectionIndex >= Enum.GetNames(typeof(targetSelection)).Length)
            currentTargetSelectionIndex = 0;

        targetSelectionText.SetText(targetSelectionNames[currentTargetSelectionIndex]);
        selectedTower.SetTargetSelection( (targetSelection)currentTargetSelectionIndex );
    }

    public void CloseMenu(bool shouldUnselect = false, BaseTower tower = null)
    {
        if (shouldUnselect)
            GameManager.instance.Unselect();

        if (tower != selectedTower) return;
        selectedTower = null;
        menuPanel.SetActive(false);
    }

    public void CloseMenuButton()
    {
        CloseMenu(true);
    }

    public void OnUpgrade(int optionIndex)
    {
        if (optionIndex < selectedTower.optionlvl.Length)
        {
            // Implement upgrade logic here, e.g., check for resources, apply upgrade effects, etc.
            selectedTower.Upgrade(optionIndex);
            UpdateUpgrades();
        }
    }

    public void OnFusionUpgrade()
    {
        // Implement fusion upgrade logic here, e.g., check for resources, apply fusion effects, etc.
        selectedTower.OnFusion();
        CloseMenu(true);
    }

    public void OnSell()
    {
        // Implement sell logic here, e.g., refund resources, remove tower from the game, etc.
        selectedTower.OnSell();
        CloseMenu(true);
    }
}

[Serializable]
public class  TowerUI
{
    public TowerType towerType;
    public Sprite icon;
    public TextSO nameText;
}

