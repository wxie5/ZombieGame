using Controller.EnemyController;
using UnityEngine;

namespace View.EnemyView
{
    public class EnemyBoomerView : EnemyBaseView
    {
        // view events
        public event VFXEvent OnBoomStart;

        protected override void AttackView()
        {
            controller.DealDamageLogic(transform.position);
            OnBoomStart?.Invoke(transform.position);
            GetHitView(float.PositiveInfinity);
        }

        protected override void ChangeToAttack()
        {
            base.ChangeToAttack();

            controller.ResetCoolDown();
        }

        protected override void OnDestroy()
        {
            // unsubscribe view events
            OnBoomStart -= Factory.GameFactoryManager.Instance.VFXFact.InstBloodExplosion;

            base.OnDestroy();
        }
    }
}
