using UnityEngine;

public abstract class BossEnemy : Enemy
{
    [SerializeField] protected float auraRadius;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioClip screamSound;

    public override void Initialize()
    {
        base.Initialize();
        Animate(Animation.Walk);
    }

    protected override void SetTarget()
    {
        base.SetTarget();
        ActivateEffect();
    }

    public void ActivateEffect()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, auraRadius, layerMask);
        //Animation controller Scream
        Animate(Animation.Scream);
        //Play Scream Sound
        AudioManager.instance.PlaySoundFXClip(screamSound, transform, 1f);
        AuraEffect(targets);
    }

    public void Animate(Animation animation)
    {
        switch(animation)
        {
            case Animation.Walk:
                animator.SetTrigger("TrMove");
                break;
            case Animation.Scream:
                animator.SetTrigger("TrScream");
                break;
            case Animation.Die:
                animator.SetTrigger("TrDie");
                break;
        }
    }

    protected override void Die()
    {
        movementEnabled = false;
        Animate(Animation.Die);
        AudioManager.instance.PlaySoundFXClip(screamSound, transform, 1f);
        Invoke("Disappear", 3f);
    }

    public abstract void AuraEffect(Collider[] targets);
}

public enum Animation 
{
    Walk,
    Scream,
    Die
}