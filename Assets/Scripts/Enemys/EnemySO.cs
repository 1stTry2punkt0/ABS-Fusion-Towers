using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatSO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    string enemyName;
    public float maxHp;
    [SerializeField] float baseHp;
    [SerializeField] [Range(0f, 0.5f)] float hpScale;
    public float amor;
    [SerializeField] float baseAmor;
    [SerializeField][Range(0f, 0.5f)] float amorScale;
    public float resistance;
    [SerializeField] float baseResistance;
    [SerializeField][Range(0f, 0.5f)] float resistanceScale;
    public float speed;
    [SerializeField] float baseSpeed;
    [SerializeField][Range(0f, 0.5f)] float speedScale;
    public MoveType moveType;

    public void SetLevel(int level)
    {
        maxHp = baseHp  + baseHp * level * hpScale;
        amor = Mathf.Min(baseAmor + baseAmor * level * amorScale, 90);
        resistance = Mathf.Min(baseResistance + baseResistance * level * resistanceScale,90);
        speed = baseSpeed + baseSpeed * level * speedScale;
    }
}

public enum MoveType
{
    Walk,
    Fly
}