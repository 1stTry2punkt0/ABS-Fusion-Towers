using UnityEngine;
using UnityEngine.Pool;

public class ParticlePoolObj : MonoBehaviour
{
    public ObjectPool<ParticlePoolObj> pool;
    [SerializeField] protected ParticleSystem particle;
    protected float duration = 0;
    [SerializeField] bool playOnce;

    protected virtual void Awake()
    {
        if (!playOnce) return;
        duration = particle.main.duration;

    }

    public float Play()
    {
        if(particle != null)
            particle.Play();
        return duration;
    }

    public virtual void ScaleEffect(float scale)
    {
        particle.transform.localScale = Vector3.one * scale;
    }

    public void SetPool(ObjectPool<ParticlePoolObj> pool)
    {
        this.pool = pool;
    }
}
