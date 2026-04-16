using UnityEngine;

[CreateAssetMenu(fileName = "CatapultFusionSO", menuName = "Scriptable Objects/CatapultFusionSO")]
public class CatapultFusionSO : TowerStatSO
{
    [SerializeField] float dmgMultiplierForEffect;
    [SerializeField] float effectMultiplierForDmg;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * effectMultiplierForDmg;
        Debug.Log("Base Damage Increase by: " + elemental.effectiveness * effectMultiplierForDmg + " to: " + baseDamage);
        baseAttackSpeed = tower.attackSpeed ;
        baseRange = tower.range;

        baseDamageRange = tower.damageRange + elemental.duration * 0.5f;
        Debug.Log("Base Damage Range Increase by: " + elemental.duration * 0.5f + " to: " + baseDamageRange);

        duration = elemental.duration + tower.damageRange * 0.5f;
        Debug.Log("Duration Increase by: " + tower.damageRange * 0.5f + " to: " + duration);
        effectiveness = elemental.effectiveness + tower.damage * dmgMultiplierForEffect;
        Debug.Log("Effectiveness Increase by: " + tower.damage * dmgMultiplierForEffect + " to: " + effectiveness);
    }
}
