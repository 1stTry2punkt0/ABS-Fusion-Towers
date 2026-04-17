using UnityEngine;

public class Ballista : BaseTower
{
    private float attackCooldown;
    [SerializeField] GameObject bolt;
    [SerializeField] BallistaFusionSO[] fusions;
    private ProjectileType projectile = ProjectileType.Bolt;

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
            if (attackCooldown < 1f && !bolt.activeSelf) bolt.SetActive(true);
        }
    }

    public override void Attack()
    {
        if (targetEnemyData == null)
            return;

        if (attackCooldown > 0f)
            return;
        bolt.SetActive(false);
        ProjectileSpawnManager.instance.SpawnProjectile(projectile, this, targetEnemyData); // Spawn an arrow projectile
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }

    public override void Fuse(ElementalTower otherTower)
    {
        base.Fuse(otherTower);
        BallistaFusionSO fusionTower = null;
        switch (otherTower.elementalAttack)
        {
            case ParticleType.FireErruption:
                fusionTower = Instantiate(fusions[0]);
                projectile = ProjectileType.FireBolt;
                break;
            case ParticleType.IceErruption:
                fusionTower = Instantiate(fusions[1]);
                projectile = ProjectileType.IceBolt;
                break;
            case ParticleType.LightningStrike:
                fusionTower = Instantiate(fusions[2]);
                projectile = ProjectileType.LightningBolt;
                break;
        }

        fusionTower.SetStats(this, otherTower);
        stats = fusionTower;
        Initialize();
        level = 7;
    }

}
