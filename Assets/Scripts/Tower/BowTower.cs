using UnityEngine;

public class BowTower : BaseTower
{
    private float attackCooldown;

    public override void Initialize()
    {
        towerName = stats.towerName;
        level = 1;

        // Base stats
        range = stats.baseRange;
        rangeCollider.radius = range; // Ensure the collider matches the range
        damage = stats.baseDamage;
        attackSpeed = stats.baseAttackSpeed; // attacks per second

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

    public override void TargetHit(Enemy enemy)
    {
        if(enemy == null || enemy.isDead)
            return; 
        enemy.TakeDamage(damage, DamageType.weapon);        
    }

    public override void OnFusion(BaseTower otherTower)
    {
        // Fusion logic comes later
    }

    public override void OnSell()
    {
        // Refund logic
    }

    public override void OnSelect()
    {
        // UI highlight logic
    }
}
