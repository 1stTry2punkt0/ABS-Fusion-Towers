using UnityEngine;

[CreateAssetMenu(fileName = "CatapultFusionSO", menuName = "Scriptable Objects/CatapultFusionSO")]
public class CatapultFusionSO : TowerStatSO
{
    [SerializeField] float effectXdmgMultiplier;
    [SerializeField] float dmgXeffectMultiplier;

    public void SetStats(BaseTower tower, ElementalTower elemental)
    {
        baseDamage = tower.damage + elemental.effectiveness * dmgXeffectMultiplier;
        baseAttackSpeed = tower.attackSpeed ;
        baseRange = tower.range;

        baseDamageRange = tower.damageRange + duration * 0.5f;

        duration = elemental.duration + tower.damageRange * 0.5f;
        effectiveness = elemental.effectiveness + tower.damage * effectXdmgMultiplier;
    }
}
