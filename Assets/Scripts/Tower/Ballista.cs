using UnityEngine;

public class Ballista : BaseTower
{
    private float attackCooldown;
    [SerializeField] GameObject bolt;

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
        ProjectileSpawnManager.instance.SpawnProjectile(ProjectileType.Bolt, this, targetEnemyData); // Spawn an arrow projectile
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }

    public override void TargetHit(Enemy enemy)
    {
        if(enemy == null || enemy.isDead)
            return; 
        dmgDealt += enemy.TakeDamage(damage, DamageType.weapon);


        Debug.Log($"Hit target: {targetEnemyData.name} with {targetEnemyData.currentHealth} HP for {dmgDealt} DMG");
    }

    public override void OnFusion(BaseTower otherTower)
    {
        // Fusion logic comes later
    }

}
