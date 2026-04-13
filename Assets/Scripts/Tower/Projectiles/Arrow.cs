using UnityEngine;

public class Arrow : Projectile
{
    public override void SetTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead)
        {
            targetEnemy = null;
            targetPosition.y = 1f; // Default height if no target or target is dead
            return;
        }
        targetPosition = targetEnemy.hitTransform.position;
    }
}
