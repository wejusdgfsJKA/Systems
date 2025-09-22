using EventBus;
using Spawning;
using UnityEngine;
public class SkeletonSummoner : MonoBehaviour
{
    [SerializeField] protected SpawnableData skeleman;
    [SerializeField] protected Spawner spawner;
    [SerializeField] protected int maxActiveEntities = 20;
    [SerializeField] protected int maxSpawnSize = 1;
    protected int activeEntities;
    [SerializeField] float minRespawnDelay = 5;
    [SerializeField] protected float delay;
    [SerializeField] protected Transform spawnPoint;
    private void Awake()
    {
        if (spawnPoint == null) spawnPoint = transform;
    }
    private void Update()
    {
        if (activeEntities < maxActiveEntities)
        {
            if (delay <= 0)
            {
                delay = minRespawnDelay;
                for (int i = 0; i < maxActiveEntities - activeEntities; i++)
                {
                    if (i >= maxSpawnSize) break;
                    SpawnGoober();
                }
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }
    public void SpawnGoober()
    {
        var k = spawner.Spawn(skeleman, spawnPoint);
        activeEntities++;
        EventBus<EntityDied>.AddActions(k.transform.GetInstanceID(), null, EntityDied);
        k.gameObject.SetActive(true);
    }
    public void EntityDied()
    {
        activeEntities--;
        delay = minRespawnDelay;
    }
}
