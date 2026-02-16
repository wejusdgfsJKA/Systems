using System.Collections;
using UnityEngine;
namespace HybridBT
{
    /// <summary>
    /// Base behaviour tree class. Contains a blackboard and root node.
    /// </summary>
    /// <typeparam name="T">Key data type.</typeparam>
    public abstract class BT<T> : MonoBehaviour
    {
        protected Context<T> blackboard;
        [SerializeField] protected NodeData<T> rootData;
        /// <summary>
        /// Time between tree ticks.
        /// </summary>
        [SerializeField] protected float tickInterval = 0.1f;
        protected WaitForSeconds intervalWait;
        [SerializeField] protected bool shouldRun = true;
        protected WaitUntil pauseWait;
        public Node<T> Root { get; protected set; }
        protected Coroutine coroutine;
        protected virtual void Awake()
        {
            intervalWait = new WaitForSeconds(tickInterval);
            pauseWait = new WaitUntil(() => shouldRun);
            blackboard = new(transform);
            SetupBlackboard();
            SetupTree();
        }
        protected abstract void SetupBlackboard();
        /// <summary>
        /// In the base version, sets up the root node from the root node data.
        /// </summary>
        protected virtual void SetupTree()
        {
            Root = rootData.ObtainNode(blackboard);
        }
        protected virtual void OnEnable()
        {
           coroutine= StartCoroutine(TickLoop());
        }
        protected virtual void OnDisable()
{
    StopCoroutine(coroutine);
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
        /// <summary>
        /// Sets shouldRun to false, pausing the tree.
        /// </summary>
        public void Pause() => shouldRun = false;
        /// <summary>
        /// Sets shouldRun to true, resuming the tree.
        /// </summary>
        public void Resume() => shouldRun = true;
    }
}
