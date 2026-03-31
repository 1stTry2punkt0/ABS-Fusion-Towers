using UnityEngine;

[CreateAssetMenu(fileName = "BallistaFusionSO", menuName = "Scriptable Objects/BallistaFusionSO")]
public class BallistaFusionSO : TowerStatSO
{
    [SerializeField] float effectXdmgMultiplier;
    [SerializeField] float dmgXeffectMultiplier;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * dmgXeffectMultiplier;
        baseAttackSpeed = tower.attackSpeed;
        baseRange = tower.range + duration * 0.5f;

        baseDamageRange = tower.damageRange;

        duration = elemental.duration + tower.range * 0.5f;
        effectiveness = elemental.effectiveness + tower.damage * effectXdmgMultiplier;
    }
}
