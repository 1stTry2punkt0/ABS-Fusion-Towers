using DigitalRuby.LightningBolt;
using UnityEngine;

public class ChainLightEffect : ParticlePoolObj
{
    
    private LightningBoltScript lightningBoltScript;
    [SerializeField] float lightningDuration = 1f;
    private Transform startTarget;
    private Transform endTarget;

    protected override void Awake()
    {
        lightningBoltScript = GetComponentInChildren<LightningBoltScript>();
    }

    private void Update()
    {
        if (startTarget != null)
            lightningBoltScript.StartObject.transform.position = startTarget.position;

        if (endTarget != null)
            lightningBoltScript.EndObject.transform.position = endTarget.position;
    }

    public void Initialize(Transform start, Transform end)
    {
        startTarget = start;
        endTarget = end;

        lightningBoltScript.StartObject.transform.position = start.position;
        lightningBoltScript.EndObject.transform.position = end.position;

        Invoke(nameof(Reset), lightningDuration);
    }

    private void Reset()
    {
        pool.Release(this);
    }
}
