using UnityEngine;
using UnityEngine.AI;
namespace Utilities
{
    public class Utilities
    {
        public static float GetNavmeshDistance(Vector3 start, Vector3 end, NavMeshAgent agent)
        {
            NavMeshPath path = new();
            if (agent.CalculatePath(end, path))
            {
                float distance = 0;
                for (int i = 1; i < path.corners.Length; i++)
                {
                    distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
                return distance;
            }
            return float.PositiveInfinity;
        }
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
