using UnityEngine;
using UnityEngine.AI;

namespace LazyPan {
    public class AgentUtil : Singleton<AgentUtil> {
        public NavMeshPath GetAgentPath(NavMeshAgent agent, Vector3 targetPoint) {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPoint, path);
            return path;
        }
    }
}
