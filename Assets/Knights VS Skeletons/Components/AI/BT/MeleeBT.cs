using BT;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
namespace KvS.BT
{
    public class MeleeBT : BehaviourTree<int>
    {
        protected static readonly int Target = "Target".GetHashCode(),
            DistanceToTarget = "DistToTarget".GetHashCode(),
            PreviousTargetPosition = "PrevTargetPos".GetHashCode();
        [SerializeField] protected float meleeRange = 2;
        [Tooltip("Hook this to whatever actually does the attack.")]
        [SerializeField] protected UnityEvent attackEvent;
        [SerializeField] protected float regularSpeed = 3.5f, chaseSpeed = 3.5f;
        [Tooltip("The chase node will only recalculate the Agent's path when the target's position has changed " +
            "by at least this much.")]
        [SerializeField] protected float chaseErrorThreshold;
        protected NavMeshAgent agent;
        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
        }
        protected override void SetupTree()
        {
            localMemory.SetData<Transform>(Target, null);
            localMemory.SetData(DistanceToTarget, Mathf.Infinity);
            localMemory.SetData(PreviousTargetPosition, Vector3.zero);

            root = new Selector("Root");

            root.AddChild(CreateCombatSubtree());
        }
        protected virtual Selector CreateCombatSubtree()
        {
            var s = new Selector("Combat");
            s.AddDecorator(new Decorator("HasTarget", () =>
            {
                return localMemory.GetData<Transform>(Target) != null;
            })).MonitorValue(localMemory, Target);
            s.AddService(new Service("DistanceToTarget", () =>
            {

                localMemory.SetData(DistanceToTarget, Vector3.Distance(transform.position,
                    localMemory.GetData<Transform>(Target).position));
            }));

            //attack node
            var attack = new LeafNode("AttackEvent", () =>
            {
                var targetPos = localMemory.GetData<Transform>(Target).position;
                transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
                attackEvent?.Invoke();
                return NodeState.RUNNING;
            }, () =>
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            });
            attack.AddDecorator(new Decorator("IsInRange", () =>
            {
                return localMemory.GetData<float>(DistanceToTarget) <= meleeRange;
            })).MonitorValue(localMemory, DistanceToTarget);

            s.AddChild(attack);

            //chase node
            s.AddChild(new LeafNode("Chase", () =>
            {
                var prevPos = localMemory.GetData<Vector3>(PreviousTargetPosition);
                var targetPos = localMemory.GetData<Transform>(Target).position;
                if ((prevPos - targetPos).magnitude > chaseErrorThreshold)
                {
                    agent.destination = targetPos;
                    localMemory.SetData(PreviousTargetPosition, targetPos);
                }
                return NodeState.RUNNING;
            }, () =>
            {
                var targetPos = localMemory.GetData<Transform>(Target).position;
                agent.destination = targetPos;
                localMemory.SetData(PreviousTargetPosition, targetPos);
                agent.isStopped = false;
                agent.speed = chaseSpeed;
            }));

            return s;
        }
        public void SetTarget(Transform target)
        {
            localMemory.SetData(Target, target);
            if (target == null)
            {
                localMemory.SetData(DistanceToTarget, Mathf.Infinity);
            }
        }
    }
}
