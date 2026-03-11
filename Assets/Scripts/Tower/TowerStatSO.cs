using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatSO", menuName = "Scriptable Objects/TowerStatSO")]
public class TowerStatSO : ScriptableObject
{
    public TextSO towerName;
    public DamageType damageType;
    public StatusEffect statusEffect;

    [Header("Base Stats")]
    public float baseDamage;
    public float baseAttackSpeed;
    public float baseRange;
    public UpgradeOption[] upgradeOption;
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
    [Tooltip("The amount for the first 5 upgrades in total")]
    public float increaseAmount; // The amount by which the stat will increase when upgraded
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