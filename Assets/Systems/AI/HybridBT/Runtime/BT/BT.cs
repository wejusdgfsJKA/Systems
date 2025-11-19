using System.Collections;
using UnityEngine;
namespace HybridBT
{
    public abstract class BT<T> : MonoBehaviour
    {
        protected Context<T> blackboard;
        [SerializeField] protected NodeData<T> rootData;
        [SerializeField] protected float tickInterval = 0.1f;
        protected WaitForSeconds intervalWait;
        [SerializeField] protected bool shouldRun = true;
        protected WaitUntil pauseWait;
        public Node<T> Root { get; protected set; }
        protected virtual void Awake()
        {
            intervalWait = new WaitForSeconds(tickInterval);
            pauseWait = new WaitUntil(() => shouldRun);
            blackboard = new(transform);
            SetupBlackboard();
            SetupTree();
        }
        protected abstract void SetupBlackboard();
        protected virtual void SetupTree()
        {
            Root = rootData.ObtainNode(blackboard);
        }
        protected virtual void OnEnable()
        {
            StartCoroutine(TickLoop());
        }
        protected IEnumerator TickLoop()
        {
            while (gameObject.activeSelf)
            {
                yield return intervalWait;
                yield return pauseWait;
                Root.Evaluate(blackboard);
            }
        }
        public void SetValue(T key, object value) => blackboard.SetData(key, value);
        public void Pause() => shouldRun = false;
        public void Resume() => shouldRun = true;
    }
}
