using UnityEngine;

public class Arrow : Projectile
{
    public override void SetTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead)
        {
            targetPosition.y = 1f; // Default height if no target or target is dead
            return;
        }
        Vector3 position = targetEnemy.transform.position;
        position.y += targetEnemy.heightOffset; // Aim for the upper part of the enemy
        targetPosition = position;
    }
}
