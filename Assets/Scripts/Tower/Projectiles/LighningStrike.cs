using System.Collections;
using UnityEngine;

public class LighningStrike : Projectile
{

    public override void Initialize(BaseTower tower, Enemy target)
    {
        base.Initialize(tower, target);
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void SetTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead)
        {
            targetEnemy = null;
            return;
        }
    }

    private IEnumerator Attack()
    {
        ParticleSpawnManager.instance.SpawnParticle(ParticleType.LightningStrike, transform.position);
        yield return new WaitForSeconds(0.1f);
        SetTarget();
        if (targetEnemy != null)
            parentTower.TargetHit(targetEnemy);
        yield return new WaitForSeconds(3f);
        pool.Release(this);
    }
}
