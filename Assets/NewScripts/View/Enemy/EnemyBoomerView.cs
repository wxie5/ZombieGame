using Controller.EnemyController;
using UnityEngine;

namespace View.EnemyView
{
    public class EnemyBoomerView : EnemyBaseView
    {
        public event VFXEvent OnBoomStart;

        public override void AttackView()
        {
            OnBoomStart?.Invoke(transform.position);
            DealDamageAll();
            GetHitView(float.PositiveInfinity);
        }

        protected override void OnDestroy()
        {
            // unsubscribe view events
            OnBoomStart -= Factory.GameFactoryManager.Instance.VFXFact.InstBloodExplosion;

            base.OnDestroy();
        }
    }
}
