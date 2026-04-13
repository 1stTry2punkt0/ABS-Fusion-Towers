using UnityEngine;

[CreateAssetMenu(fileName = "BallistaFusionSO", menuName = "Scriptable Objects/BallistaFusionSO")]
public class BallistaFusionSO : TowerStatSO
{
    [SerializeField] float dmgMultiplierForEffect;
    [SerializeField] float effectMultiplierForDmg;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * effectMultiplierForDmg;
        Debug.Log("Base Damage Increase: " + elemental.effectiveness * effectMultiplierForDmg);
        baseAttackSpeed = tower.attackSpeed;
        baseRange = tower.range + duration * 0.5f;
        Debug.Log("Base Range Increase: " + duration * 0.5f);

        baseDamageRange = tower.damageRange;

        duration = elemental.duration + tower.range * 0.5f;
        Debug.Log("Duration Increase: " + tower.range * 0.5f);
        effectiveness = elemental.effectiveness + tower.damage * dmgMultiplierForEffect;
        Debug.Log("Effectiveness Increase: " + tower.damage * dmgMultiplierForEffect);
    }
}
