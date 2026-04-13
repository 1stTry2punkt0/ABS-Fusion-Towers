using UnityEngine;

[CreateAssetMenu(fileName = "BowFusionSO", menuName = "Scriptable Objects/BowFusionSO")]
public class BowFusionSO : TowerStatSO
{
    [SerializeField] float dmgMultiplierForEffect;
    [SerializeField] float effectMultiplierForDmg;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * effectMultiplierForDmg;
        Debug.Log("Base Damage Increase: " + elemental.effectiveness * effectMultiplierForDmg);
        baseAttackSpeed = tower.attackSpeed + duration * 0.5f;
        Debug.Log("Base Attack Speed Increase: " + duration * 0.5f);
        baseRange = tower.range;

        baseDamageRange = tower.damageRange;

        duration = elemental.duration + tower.attackSpeed * 0.5f;
        Debug.Log("Duration Increase: " + tower.attackSpeed * 0.5f);
        effectiveness = elemental.effectiveness + tower.damage * dmgMultiplierForEffect;
        Debug.Log("Effectiveness Increase: " + tower.damage * dmgMultiplierForEffect);
    }
}
