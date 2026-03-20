using UnityEngine;
using UnityEngine.Pool;

public class ParticlePoolObj : MonoBehaviour
{
    public ObjectPool<ParticlePoolObj> pool;
    [SerializeField] ParticleSystem particle;
    float duration = 0;
    [SerializeField] bool playOnce;

    public void Awake()
    {
        if (!playOnce) return;
        duration = particle.main.duration;

    }

    public float Play()
    {
        particle.Play();
        return duration;
    }

    public void ScaleEffect(float scale)
    {
        particle.transform.localScale = Vector3.one * scale;
    }

    public void SetPool(ObjectPool<ParticlePoolObj> pool)
    {
        this.pool = pool;
    }
}
