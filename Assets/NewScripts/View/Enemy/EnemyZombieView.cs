using Controller.EnemyController;
using UnityEngine;

namespace View.EnemyView
{
    public class EnemyZombieView : EnemyBaseView
    {
        protected override void AttackView()
        {
            animator.SetTrigger("Attack");
        }

        #region Animation Events
        public void OnAnimationAttackPoint()
        {
            controller.DealDamageLogic(transform.position, target);
        }
        #endregion
    }
}
