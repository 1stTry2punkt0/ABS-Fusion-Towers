using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    [SerializeField] Image icon;
    [SerializeField] TextSceneObject nameText;
    [SerializeField] TMPro.TextMeshProUGUI level;
    [SerializeField] Image[] upgradeProgress;
    [SerializeField] TextSceneObject[] upgradeProgressText;
    [SerializeField] TowerUI[] towerUI;
    [SerializeField] GameObject FusionUpgrade;

    [HideInInspector]
    public BaseTower selectedTower;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu(BaseTower tower)
    {
        selectedTower = tower;
        tower.OnSelect(true);
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
            UpdateUpgrades();
        }
    }

    private void UpdateUpgrades()
    {
        Debug.Log("Updating upgrades for " + selectedTower.name + " With Level: " + selectedTower.level);
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

    public void OnClose(bool shouldUnselect = false)
    {
        if (shouldUnselect)
            GameManager.instance.Unselect();

        if (selectedTower != null)
            selectedTower.OnSelect(false);
        selectedTower = null;
        menuPanel.SetActive(false);
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
        
        UpdateUpgrades();
    }

    public void OnSell()
    {
        // Implement sell logic here, e.g., refund resources, remove tower from the game, etc.
        selectedTower.OnSell();
        OnClose(true);
    }
}

[Serializable]
public class  TowerUI
{
    public TowerType towerType;
    public Sprite icon;
    public TextSO nameText;
}

