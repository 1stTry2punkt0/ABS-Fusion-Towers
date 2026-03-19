using UnityEngine;

public class Merchant : MonoBehaviour
{
    public Transform spawnPoint;
    public Cost goods;
    private bool movementEnabled = false;
    private Vector3 targetPosition;
    [SerializeField] float speed;
    private float distanceToTarget;
    private int currentWaypointIndex = 0;

    public void StartRound()
    {
        goods.amount = 100 + WaveManager.instance.currentWave * 10;
        transform.position = spawnPoint.position;
        currentWaypointIndex = 0;
        targetPosition = GameManager.instance.GetWorldPosition(EnemySpawnManager.instance.enemyPath[currentWaypointIndex]);
        movementEnabled = true;
        gameObject.SetActive(true);
    }

    public void EndRound()
    {
        movementEnabled = false;
        gameObject.SetActive(false);
        WaveManager.instance.EndWave();
    }

    void Update()
    {
        if (!movementEnabled) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //Rotate towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            if (lookRotation != transform.rotation)
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < speed * Time.deltaTime)
        {
            transform.position = targetPosition;
            currentWaypointIndex++;

            if (currentWaypointIndex < EnemySpawnManager.instance.enemyPath.Count)
            {
                targetPosition = GameManager.instance.GetWorldPosition(EnemySpawnManager.instance.enemyPath[currentWaypointIndex]);
            }
            else
            {
                RessourceManager.instance.GainRessource(goods);
                EndRound();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.stats.moveType != MoveType.Fly)
                EndRound();
        }
    }
}
