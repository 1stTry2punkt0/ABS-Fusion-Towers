using UnityEngine;

public class Arrow : Projectile
{
    public override void SetTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead)
        {
            return;
        }
        targetTransform = targetEnemy.transform;
    }
}
