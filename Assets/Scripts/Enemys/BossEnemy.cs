using UnityEngine;

public abstract class BossEnemy : Enemy
{
    [SerializeField] protected float auraRadius;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Animator animator;
    private static readonly int AnimationHash = Animator.StringToHash("Action");

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
        AuraEffect(targets);
    }

    public void Animate(Animation animation)
    {
        //animator.SetInteger(AnimationHash, animation);
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