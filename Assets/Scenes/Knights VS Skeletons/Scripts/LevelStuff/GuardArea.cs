using EventBus;
using Spawning;
using UnityEngine;
public class GuardArea : MonoBehaviour
{
    [SerializeField] protected SpawnableData knight;
    [SerializeField] protected Spawner spawner;
    [SerializeField] protected Transform[] guardArea;
    [SerializeField] protected int maxGuards = 1;
    protected AssignGuardArea @event;
    protected int assignedGuards;
    [SerializeField] float minRespawnDelay = 5;
    [SerializeField] protected float delay;
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected int maxRespawns = 5;
    private void Awake()
    {
        @event = new(guardArea);
        if (spawnPoint == null) spawnPoint = guardArea[0];
    }
    private void Update()
    {
        if (maxRespawns > 0 && maxGuards > assignedGuards)
        {
            if (delay <= 0)
            {
                for (int i = assignedGuards; i < maxGuards; i++)
                {
                    SpawnSentry();
                }
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }
    public void SpawnSentry()
    {
        var k = spawner.Spawn(knight, spawnPoint);
        assignedGuards++;
        k.gameObject.name += assignedGuards.ToString();
        EventBus<AssignGuardArea>.Raise(k.transform.GetInstanceID(), @event);
        EventBus<EntityDied>.AddActions(k.transform.GetInstanceID(), null, SentryDied);
        k.gameObject.SetActive(true);
        maxRespawns--;
    }
    public void SentryDied()
    {
        assignedGuards--;
        delay = minRespawnDelay;
    }
    protected void OnDrawGizmos()
    {
        if (guardArea != null)
        {
            Gizmos.color = maxRespawns > 0 ? Color.magenta : Color.red;
            var spawn = spawnPoint != null ? spawnPoint.position : transform.position;
            Gizmos.DrawWireSphere(spawn, 1);
            if (guardArea.Length < 1)
            {
                return;
            }
            for (int i = 0; i < guardArea.Length - 1; i++)
            {
                if (guardArea[i] != null && guardArea[i + 1] != null)
                {
                    Gizmos.DrawLine(guardArea[i].position, guardArea[i + 1].position);
                }
            }
            if (guardArea[0] != null && guardArea[guardArea.Length - 1] != null)
            {
                Gizmos.DrawLine(guardArea[guardArea.Length - 1].position, guardArea[0].position);
            }
        }
    }
}
