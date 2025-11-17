using System.Collections;
using UnityEngine;
namespace HybridBT
{
    public abstract class BT<T> : MonoBehaviour
    {
        protected Context<T> blackboard;
        [SerializeField] protected NodeData<T> rootData;
        [SerializeField] protected float tickInterval = 0.1f;
        protected WaitForSeconds wait;
        public Node<T> Root { get; protected set; }
        protected void Awake()
        {
            wait = new WaitForSeconds(tickInterval);
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
                yield return wait;
                Root.Evaluate(blackboard);
            }
        }
        public void SetValue(T key, object value) => blackboard.SetValue(key, value);
    }
}
