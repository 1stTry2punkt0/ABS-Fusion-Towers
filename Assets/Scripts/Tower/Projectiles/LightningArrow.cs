using System.Linq;
using UnityEngine;

public class LightningArrow : ElementalArrow
{
    [SerializeField] int maxChainTargets = 9;
    protected override bool FindNextTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead) return false;
        Collider[] hitColliders = GetTargets();
        ChainLightUser.PlayChain(hitColliders, ChainBehavior.Ordered);
        float multiplier = 1;
        foreach (Collider hit in hitColliders)
        {
            multiplier -= 0.1f; // Decrease damage by 10% for each additional enemy hit
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null || enemy == targetEnemy)
            {
                continue;
            }
            parentTower.TargetHit(enemy, multiplier);
        }
        return false;
    }

    private Collider[] GetTargets()
    {
        Collider[] colliders = new Collider[maxChainTargets];
        Enemy currentEnemy = targetEnemy;
        colliders[0] = targetEnemy.GetComponent<Collider>();
        for (int i = 1; i < maxChainTargets; i++)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(currentEnemy.transform.position, parentTower.range * 0.25f, GameManager.instance.enemyLayer);
            Enemy nextEnemy = null;
            float closestDistance = Mathf.Infinity;
            foreach (Collider collider in nearbyColliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy == null || colliders.Contains(enemy.GetComponent<Collider>())) continue;
                float distance = Vector3.Distance(currentEnemy.transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nextEnemy = enemy;
                }
            }
            if (nextEnemy == null)
            {
                System.Array.Resize(ref colliders, i);
                break;
            }
            colliders[i] = nextEnemy.GetComponent<Collider>();
            currentEnemy = nextEnemy;
        }
        return colliders;
    }
}
