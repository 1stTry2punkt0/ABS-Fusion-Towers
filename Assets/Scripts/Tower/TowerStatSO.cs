using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatSO", menuName = "Scriptable Objects/TowerStatSO")]
public class TowerStatSO : ScriptableObject
{
    public TowerType towerName;
    public DamageType damageType;
    public StatusEffect statusEffect;

    [Header("Base Stats")]
    public float baseDamage;
    public float baseAttackSpeed;
    public float baseRange;
    public UpgradeOption[] upgradeOption;

    public Cost baseCost;
    public Cost[] upgradeCosts;
    public Cost fusionCost;

}

public enum DamageType
{
    weapon,
    elemental,
    trueDamage
}

[System.Serializable]
public class UpgradeOption
{
    public Stats statToUpgrade; // The stat that will be upgraded (e.g., damage, attack speed, range)
    public TextSO upgradeName; // Name of the upgrade option for display purposes
    [Tooltip("The amount for the first 5 upgrades in total")]
    public float increaseAmount; // The amount by which the stat will increase when upgraded
}

[System.Serializable]
public class Cost
{
    public int amount;
    public RessourceType ressourceType;
}

public enum Stats
{
    damage,
    attackSpeed,
    range
}

public enum StatusEffect
{
    none,
    burn,
    freeze,
    poison,
    stun
}

public enum TowerType
{
    BowTower
}