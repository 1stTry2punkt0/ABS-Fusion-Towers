using System.Collections;
using UnityEngine;

public class Bomb : Projectile
{
    public override void SetTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead)
        {
            targetEnemy = null;
            targetPosition.y = 1f; // Default height if no target or target is dead
            return;
        }
        Vector3 position = targetEnemy.transform.position;
        position.y += targetEnemy.heightOffset; // Aim for the upper part of the enemy
        targetPosition = position;
    }

    protected override bool FindNextTarget()
    {
        foreach(Collider hit in Physics.OverlapSphere(transform.position, parentTower.damageRange, GameManager.instance.enemyLayer))
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null || enemy == targetEnemy) continue;
            parentTower.TargetHit(hit.gameObject.GetComponent<Enemy>());
        }
        return false;
    }

}
