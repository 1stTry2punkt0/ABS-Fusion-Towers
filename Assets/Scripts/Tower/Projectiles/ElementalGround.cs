using System.Collections.Generic;
using UnityEngine;

public class ElementalGround : MonoBehaviour
{
    float duration;
    float effectiveness = 1f;
    StatusEffect statusEffect;
    private Catapult catapult;
    public List<Enemy> enemies = new List<Enemy>();
    private SphereCollider trigger;
    ParticlePoolObj particle;
    [SerializeField] ParticleSystem effect;

    void Awake()
    {
        trigger = GetComponent<SphereCollider>();
        particle = GetComponentInChildren<ParticlePoolObj>();
    }

    public void Initialize(BaseTower tower)
    {
        catapult = tower as Catapult;
        catapult.activeGrounds.Add(this);
        effectiveness = tower.effectiveness;
        duration = tower.duration;
        statusEffect = tower.stats.statusEffect;
        trigger.radius = tower.damageRange;
        if (effect != null)
        {
            var main = effect.main;
            main.duration = duration;
            effect.Play();
        }
        Invoke(nameof(EndEffect), duration);
    }

    private void EndEffect()
    {
        enemies.ForEach(enemy => EndStatuseffect(enemy));
        enemies.Clear();
        if (effect != null)
        {
            effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        particle.pool.Release(particle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemies.Add(enemy);
            enemy.ApplyStatusEffect(catapult, statusEffect, duration, effectiveness);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemies.Remove(enemy);
            EndStatuseffect(enemy);
        }
    }

    private void EndStatuseffect(Enemy enemy)
    {
        foreach (ElementalGround ground in catapult.activeGrounds)
        {
            if (ground != this && ground.enemies.Contains(enemy))
                return;
        }
        enemy.RemoveStatusEffect(statusEffect, catapult);
    }
}
