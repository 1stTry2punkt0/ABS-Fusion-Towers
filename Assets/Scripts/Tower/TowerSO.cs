using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatSO", menuName = "Scriptable Objects/TowerSO")]
public class TowerSO : ScriptableObject
{
    
}

public enum DamageType
{
    weapon,
    elemental,
    trueDamage
}

public enum StatusEffect
{
    burn,
    freeze,
    poison,
    stun
}