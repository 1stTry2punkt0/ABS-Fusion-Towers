using UnityEngine;

[CreateAssetMenu(fileName = "BowFusionSO", menuName = "Scriptable Objects/BowFusionSO")]
public class BowFusionSO : TowerStatSO
{
    [SerializeField] float dmgMultiplierForEffect;
    [SerializeField] float effectMultiplierForDmg;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * effectMultiplierForDmg;
        Debug.Log("Base Damage Increase by: " + elemental.effectiveness * effectMultiplierForDmg + " to: " + baseDamage);
        baseAttackSpeed = tower.attackSpeed + elemental.duration * 0.2f;
        Debug.Log("Base Attack Speed Increase by: " + elemental.duration * 0.2f + " to: " + baseAttackSpeed);
        baseRange = tower.range;

        baseDamageRange = tower.damageRange;

        duration = elemental.duration + tower.attackSpeed * 0.7f;
        Debug.Log("Duration Increase by: " + tower.attackSpeed * 0.7f + " to: " + duration);
        effectiveness = elemental.effectiveness + tower.damage * dmgMultiplierForEffect;
        Debug.Log("Effectiveness Increase by: " + tower.damage * dmgMultiplierForEffect + " to: " + effectiveness);
    }
}
