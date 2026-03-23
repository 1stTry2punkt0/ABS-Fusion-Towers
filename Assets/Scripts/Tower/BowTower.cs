using UnityEngine;

public class BowTower : BaseTower
{
    private float attackCooldown;

    public override void Initialize()
    {
        base.Initialize();

        attackCooldown = 0f;
    }

    private void FixedUpdate()
    {
        // Cooldown handling
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.fixedDeltaTime;
        }
    }

    public override void Attack()
    {
        if (targetEnemyData == null)
            return;

        if (attackCooldown > 0f)
            return;
        ProjectileSpawnManager.instance.SpawnProjectile(ProjectileType.Arrow, this, targetEnemyData); // Spawn an arrow projectile
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }

    public override void OnFusion(BaseTower otherTower)
    {
        // Fusion logic comes later
    }

}
