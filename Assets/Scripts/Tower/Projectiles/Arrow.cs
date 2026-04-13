using UnityEngine;

public class Arrow : Projectile
{
    public override void SetTarget()
    {
        if (targetEnemy != null)
        {
            if (!targetEnemy.isDead)
            {
                targetPosition = targetEnemy.hitTransform.position;
                return;
            }
        }
        targetEnemy = null;
        targetPosition.y = 1f; // Default height if no target or target is dead
    }
}