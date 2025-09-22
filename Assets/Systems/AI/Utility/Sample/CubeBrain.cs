using UnityEngine;
namespace Sample
{
    public class CubeBrain : UtilityBrain
    {
        [SerializeField] Transform target;
        protected override void SetupContext()
        {
            base.SetupContext();
            Context.SetData(UtilityAI.ContextDataKeys.Target, target);
        }
        protected override void UpdateContext()
        {

        }
        private void Update()
        {
            Execute(Time.deltaTime);
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 10);
        }
    }
}