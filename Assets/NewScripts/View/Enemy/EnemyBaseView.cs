using UnityEngine;
using UnityEngine.AI;
using Controller.EnemyController;

namespace View.EnemyView
{
    public class EnemyBaseView : MonoBehaviour, IEnemyView
    {
        // Timer
        [SerializeField] protected float getPlayerFreq = 0.5f;
        protected float curGetPlayerTimer;

        // Components
        protected NavMeshAgent agent;
        protected Animator animator;

        // AI state
        protected EnemyState curState;
        protected bool isDead;

        // Local UI
        protected HPBar hpbar;

        // view delegate
        public delegate void ProjectileEvent(Vector3 vec, Quaternion dir, float dmg);
        public delegate void VFXEvent(Vector3 vec);

        public bool IsDead
        {
            get { return isDead; }
        }

        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            curState = EnemyState.Idle;
            curGetPlayerTimer = 0f;
            isDead = false;
            hpbar = GetComponent<HPBar>();
        }

        public void HPBarChange(float curHealth, float maxHealth)
        {
            hpbar.SetFill(curHealth, maxHealth);
        }

        public void HPBarHide(int score)
        {
            hpbar.DisableHPBar();
        }

        public virtual void GetHitView(float damage) { }
    }
}
