using Controller.EnemyController;
using UnityEngine;

namespace View.EnemyView
{
    public class EnemyPosionView : EnemyBaseView
    {
        // view events
        public event ProjectileEvent OnShotProjectile;

        protected override void AttackView()
        {
            animator.SetTrigger("Attack");
        }

        #region Animation Events
        public void OnAnimationAttackPoint()
        {
            Vector3 offset = new Vector3(0f, 1.5f, 0f) + transform.forward * 0.5f;
            Quaternion dir = Quaternion.LookRotation(target.position - transform.position);
            OnShotProjectile?.Invoke(transform.position + offset, dir, controller.DealProjectileLogic());
        }
        #endregion

        protected override void OnDestroy()
        {
            // unsubscribe view events
            OnShotProjectile -= Factory.GameFactoryManager.Instance.ProjFact.InstPosionProj;

            base.OnDestroy();
        }
    }
}
