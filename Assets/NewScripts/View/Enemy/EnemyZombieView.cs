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
