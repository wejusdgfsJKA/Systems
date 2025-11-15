using System.Collections;
using UnityEngine;
namespace HybridBT
{
    public abstract class BT<T> : MonoBehaviour
    {
        protected Context<T> blackboard;
        public NodeData<T> rootData;
        [SerializeField] protected float tickInterval = 0.1f;
        protected WaitForSeconds wait;
        protected Node<T> root;
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
            root = rootData.ObtainNode(blackboard);
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
                root.Evaluate(blackboard);
            }
        }
        public void SetValue(T key, object value) => blackboard.SetValue(key, value);
    }
}
