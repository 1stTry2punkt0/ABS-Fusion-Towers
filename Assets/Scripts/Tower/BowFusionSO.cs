using UnityEngine;

[CreateAssetMenu(fileName = "BowFusionSO", menuName = "Scriptable Objects/BowFusionSO")]
public class BowFusionSO : TowerStatSO
{
    [SerializeField] float effectXdmgMultiplier;
    [SerializeField] float dmgXeffectMultiplier;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * dmgXeffectMultiplier;
        baseAttackSpeed = tower.attackSpeed + duration * 0.5f;
        baseRange = tower.range;

        baseDamageRange = tower.damageRange;

        duration = elemental.duration + tower.attackSpeed * 0.5f;
        effectiveness = elemental.effectiveness + tower.damage * effectXdmgMultiplier;
    }
}
