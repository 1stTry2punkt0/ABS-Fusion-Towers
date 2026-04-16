using UnityEngine;

[CreateAssetMenu(fileName = "BallistaFusionSO", menuName = "Scriptable Objects/BallistaFusionSO")]
public class BallistaFusionSO : TowerStatSO
{
    [SerializeField] float dmgMultiplierForEffect;
    [SerializeField] float effectMultiplierForDmg;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * effectMultiplierForDmg;
        Debug.Log("Base Damage Increase by: " + elemental.effectiveness * effectMultiplierForDmg + " to: " + baseDamage);
        baseAttackSpeed = tower.attackSpeed;
        baseRange = tower.range + elemental.duration * 0.8f;
        Debug.Log("Base Range Increase by: " + elemental.duration * 0.8f + " to: " + baseRange);

        baseDamageRange = tower.damageRange;

        duration = elemental.duration + tower.range * 0.1f;
        Debug.Log("Duration Increase by: " + tower.range * 0.1f + " to: " + duration);
        effectiveness = elemental.effectiveness + tower.damage * dmgMultiplierForEffect;
        Debug.Log("Effectiveness Increase by: " + tower.damage * dmgMultiplierForEffect + " to: " + effectiveness);
    }
}
