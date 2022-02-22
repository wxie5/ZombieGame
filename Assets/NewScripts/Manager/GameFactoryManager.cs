using UnityEngine;

namespace Factory
{
    [RequireComponent(typeof(EnemyFactory), typeof(VFXFactory), typeof(ProjectileFactory))]
    public class GameFactoryManager : Singleton<GameFactoryManager>
    {
        private EnemyFactory enemyFact;
        private VFXFactory vfxFact;
        private ProjectileFactory projFact;

        public EnemyFactory EnemyFact
        {
            get { return enemyFact; }
        }

        public VFXFactory VFXFact
        {
            get { return vfxFact; }
        }

        public ProjectileFactory ProjFact
        {
            get { return projFact; }
        }

        private void Start()
        {
            enemyFact = GetComponent<EnemyFactory>();
            vfxFact = GetComponent<VFXFactory>();
            projFact = GetComponent<ProjectileFactory>();

            // for debug only
            //enemyFact.InstantiateZombie(Vector3.zero);
            //enemyFact.InstantiateRunner(new Vector3(1f, 0f, 1f));
            //enemyFact.InstantiateTank(new Vector3(-1f, 0f, -1f));
            //enemyFact.InstantiateBoomer(new Vector3(-5, 0f, -5f));
            //enemyFact.InstantiatePosion(new Vector3(-5, 0f, -5f));
        }
    }
}
