using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Usurper : BossEnemy
{
    [SerializeField] float buffAmount;
    private List<ParticlePoolObj> effects = new List<ParticlePoolObj>();

    public override void AuraEffect(Collider[] targets)
    {
        movementEnabled = false;
        foreach (Collider target in targets)
        {
            Enemy enemy = target.gameObject.GetComponent<Enemy>();
            enemy.speedBuff = buffAmount;
            //Maybe visualize heal
            ParticlePoolObj effect = ParticleSpawnManager.instance.SpawnParticle(ParticleType.LongBuff, enemy.hitTransform.position);
            effect.StartCoroutine(effect.FollowTarget(enemy.hitTransform));
            effects.Add(effect);
        }
        StartCoroutine(EndEffect(targets));
    }

    private IEnumerator EndEffect(Collider[] targets)
    {
        yield return new WaitForSeconds(2.8f);
        movementEnabled = true;
        yield return new WaitForSeconds(1.2f);
        foreach (Collider target in targets)
        {
            target.gameObject.GetComponent<Enemy>().speedBuff = 0;
        }
        //cancelvisualize
        foreach (ParticlePoolObj effect in effects)
        {
            if (effect != null)
                effect.pool.Release(effect);
        }
        effects.Clear();
    }

    protected override void Die()
    {
        base.Die();
        foreach (ParticlePoolObj effect in effects)
        {
            if (effect != null)
                effect.pool.Release(effect);
        }
        effects.Clear();
    }
}