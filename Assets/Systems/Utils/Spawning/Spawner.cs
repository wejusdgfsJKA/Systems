using UnityEngine;
namespace Spawning
{
    public class Spawner : MonoBehaviour
    {
        public Spawnable Spawn(Spawnable spawnable, Transform spawnPoint, System.Action<Spawnable> executeBeforeSpawn = null)
        {
            return Spawn(spawnable, spawnPoint.position, spawnPoint.rotation, executeBeforeSpawn);
        }
        public Spawnable Spawn(Spawnable spawnable, Vector3 position, System.Action<Spawnable> executeBeforeSpawn = null)
        {
            return Spawn(spawnable, position, Quaternion.identity, executeBeforeSpawn);
        }
        public Spawnable Spawn(Spawnable spawnable, Vector3 position, Quaternion rotation, System.Action<Spawnable> executeBeforeSpawn = null)
        {
            var s = Obtain(spawnable);
            executeBeforeSpawn?.Invoke(s);
            s.transform.SetPositionAndRotation(position, rotation);
            s.gameObject.SetActive(true);
            return s;
        }
        public virtual Spawnable Obtain(Spawnable spawnable)
        {
            var s = Instantiate(spawnable);
            s.Initialize();
            return s;
        }
    }
}