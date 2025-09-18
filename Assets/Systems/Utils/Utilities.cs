using UnityEngine;
using UnityEngine.AI;
namespace Utilities
{
    public class Utilities
    {
        public static Vector3? GetRandomPointOnMesh(float radius, NavMeshAgent agent, int nrOfTries = 5)
        {
            NavMeshHit hit;
            Vector3 v = agent.transform.position + Random.onUnitSphere * radius;
            int c = 0;
            while (!NavMesh.SamplePosition(v, out hit, radius, agent.areaMask))
            {
                v = agent.transform.position + Random.onUnitSphere * radius;
                c++;
                if (c > nrOfTries) return null;
            }
            return hit.position;
        }
    }
}
