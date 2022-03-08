using Controller.EnemyController;
using UnityEngine;
using System.Collections.Generic;
using System;
using Model.EnemyModel;

namespace View.EnemyView
{
    public class EnemyZombieView : EnemyBaseView
    {
        public override void AttackView()
        {
            animator.SetTrigger("Attack");
        }

        #region Animation Events
        public void OnAnimationAttackPoint()
        {
            DealDamageSingle();
        }
        #endregion
    }
}
