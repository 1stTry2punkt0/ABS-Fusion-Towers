using System.Collections;
using UnityEngine;

public class ElementalArrow : Arrow
{
    [SerializeField] ParticleSystem effect;
    [SerializeField] GameObject arrow;
    private bool hitTarget = false;

    public override void Initialize(BaseTower tower, Enemy target)
    {
        base.Initialize(tower, target);
        hitTarget = false;
        if (effect != null)
        {
            effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            effect.Play();
        }
    }

    protected override void Update()
    {
        if (hitTarget) return;
        base.Update();
    }

    protected override void Disable()
    {
        hitTarget = true;
        if(arrow != null)
            arrow.SetActive(false); // Hide the arrow immediately
        StartCoroutine(WaitForTrail());
    }

    private IEnumerator WaitForTrail()
    {
        if (effect != null)
        {
            effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        yield return new WaitForSeconds(0.7f); // Adjust the delay as needed
        if (arrow != null)
            arrow.SetActive(true); // Reset the arrow for the next use
        pool.Release(this);
    }
}