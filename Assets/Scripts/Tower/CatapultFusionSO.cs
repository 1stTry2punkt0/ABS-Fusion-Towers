using UnityEngine;

[CreateAssetMenu(fileName = "CatapultFusionSO", menuName = "Scriptable Objects/CatapultFusionSO")]
public class CatapultFusionSO : TowerStatSO
{
    [SerializeField] float dmgMultiplierForEffect;
    [SerializeField] float effectMultiplierForDmg;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * effectMultiplierForDmg;
        Debug.Log("Base Damage Increase: " + elemental.effectiveness * effectMultiplierForDmg);
        baseAttackSpeed = tower.attackSpeed ;
        baseRange = tower.range;

        baseDamageRange = tower.damageRange + duration * 0.5f;
        Debug.Log("Base Damage Range Increase: " + duration * 0.5f);

        duration = elemental.duration + tower.damageRange * 0.5f;
        Debug.Log("Duration Increase: " + tower.damageRange * 0.5f);
        effectiveness = elemental.effectiveness + tower.damage * dmgMultiplierForEffect;
        Debug.Log("Effectiveness Increase: " + tower.damage * dmgMultiplierForEffect);
    }
}
