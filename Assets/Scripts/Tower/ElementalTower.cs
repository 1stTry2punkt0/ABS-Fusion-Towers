using System.Collections;
using UnityEngine;

public class ElementalTower : BaseTower
{
    [SerializeField] ParticleType elementalAttack;
    [SerializeField] float offset;
    private float attackCooldown;
    [SerializeField] GameObject attackLight;

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
        StartCoroutine(ElementalAttack());
        //ProjectileSpawnManager.instance.SpawnProjectile(ProjectileType.LightningStrike, this, targetEnemyData); // Spawn an arrow projectile
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }

    protected virtual IEnumerator ElementalAttack()
    {
        attackLight.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Vector3 prediction = targetEnemyData.transform.forward;
        Vector3 pos = targetEnemyData.transform.position;
        pos += prediction * offset;
        pos.y = 1;
        ParticleSpawnManager.instance.SpawnParticle(elementalAttack, pos);
        yield return new WaitForSeconds(0.1f);
        if (targetEnemyData != null)
            TargetHit(targetEnemyData);
        yield return new WaitForSeconds(0.3f);
        attackLight.SetActive(false);
    }


    public override void OnFusion(BaseTower otherTower)
    {
        // Fusion logic comes later
    }

}
