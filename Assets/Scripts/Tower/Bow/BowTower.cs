using UnityEngine;

public class BowTower : BaseTower
{
    private float attackCooldown;
    private ProjectileType projectile = ProjectileType.Arrow;

    [SerializeField] BowFusionSO[] fusions;

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
        ProjectileSpawnManager.instance.SpawnProjectile(projectile, this, targetEnemyData); // Spawn an arrow projectile
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }

    public override void Fuse(ElementalTower otherTower)
    {
        base.Fuse(otherTower);
        BowFusionSO fusionTower = null;
        switch(otherTower.elementalAttack)
        {
            case ParticleType.FireErruption:
                fusionTower = Instantiate(fusions[0]);
                projectile = ProjectileType.FireArrow;
                break;
            case ParticleType.IceErruption:
                fusionTower = Instantiate(fusions[1]);
                projectile = ProjectileType.IceArrow;
                break;
            case ParticleType.LightningStrike:
                fusionTower = Instantiate(fusions[2]);
                projectile = ProjectileType.LightningArrow;
                break;
        }

        fusionTower.SetStats(this, otherTower);
        stats = fusionTower;
        Initialize();
        level = 7;
    }

}
