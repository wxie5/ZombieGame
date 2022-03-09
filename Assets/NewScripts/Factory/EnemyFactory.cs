using UnityEngine;
using System.Collections.Generic;
using Model.EnemyModel;
using View.EnemyView;

//This script is created and wrote by Wei Xie
//Modified by Jiacheng Sun
namespace Factory
{
    public class EnemyFactory : BaseFactory
    {
        //Props
        [SerializeField] private GameObject[] props;
        private Dictionary<int, EnemyStatsCSV> enemyStatsBuffer;
        private int zombieCount;
        private EndlessModeManager endlessModeManager;

        protected override void Start()
        {
            filePath = "Prefabs/Enemy/";
            enemyStatsBuffer = CSVReader.ReadEnemyStatsCSV();
            if(GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                endlessModeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>();
            }
        }

        #region Public Instantiator
        public void InstantiateZombie(Vector3 instantiatePos)
        {
            InstantiateNormalZombie(instantiatePos, 0, "Zombie");
        }

        public void InstantiateRunner(Vector3 instantiatePos)
        {
            InstantiateNormalZombie(instantiatePos, 1, "Runner");
        }

        public void InstantiateTank(Vector3 instantiatePos)
        {
            InstantiateNormalZombie(instantiatePos, 2, "Tank");
        }

        public void InstantiateBoomer(Vector3 instantiatePos)
        {
            InstantiateBoomerZombie(instantiatePos, 4, "Boomer");
        }

        public void InstantiatePosion(Vector3 instantiatePos)
        {
            InstantiatePosionZombie(instantiatePos, 3, "Posion");
        }

        public void InstantiateCoward(Vector3 instantiatePos)
        {
            InstantiateCowardZombie(instantiatePos, 5, "Coward");
        }
        #endregion

        #region Private Zombie Instantiator (For Different Type)
        private void InstantiateNormalZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyBaseModel model = InitModel(id);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyZombieView view = zombieGO.GetComponent<EnemyZombieView>();

            view.SetUp(model);

            view.OnDead += InstantiateProps;

            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;
        }

        private void InstantiateBoomerZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyBaseModel model = InitModel(id);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyBoomerView view = zombieGO.GetComponent<EnemyBoomerView>();

            view.SetUp(model);

            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;

            view.OnBoomStart += GameFactoryManager.Instance.VFXFact.InstBloodExplosion;
            view.OnDead += InstantiateProps;
        }

        private void InstantiatePosionZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyBaseModel model = InitModel(id);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyPosionView view = zombieGO.GetComponent<EnemyPosionView>();

            view.SetUp(model);

            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;

            view.OnShotProjectile += GameFactoryManager.Instance.ProjFact.InstPosionProj;
            view.OnDead += InstantiateProps;
        }

        private void InstantiateCowardZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyBaseModel model = InitModel(id);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyCowardView view = zombieGO.GetComponent<EnemyCowardView>();

            view.SetUp(model);

            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;

            view.OnDead += InstantiateProps;
        }
        #endregion
        private EnemyBaseModel InitModel(int id)
        {
            // get model
            EnemyStatsCSV stats = enemyStatsBuffer[id];
            EnemyBaseModel model = new EnemyBaseModel(id, stats.enemyName, stats.baseHealth, stats.moveSpeed,
                stats.baseDmg, stats.alertDistance, stats.attackRange, stats.stopDistance, stats.attackRate,
                stats.attackRateDefault, stats.score, stats.baseHealthMulti, stats.baseDmgMulti, stats.defaultLevel,
                stats.maxLevel);

            if (endlessModeManager != null)
            {
                model.OnDead += endlessModeManager.onDeadAddScore;
            }
            model.OnDead += OndeadHandler;

            zombieCount++;

            return model;
        }

        public void OndeadHandler(int score)
        {
            zombieCount--;
        }
        public int GetZombieCount()
        {
            return zombieCount;
        }

        public void InstantiateProps(Vector3 position)
        {
            if (Random.Range(0, 100) < 15)
            {
                int randomInstID = Random.Range(0, props.Length);
                Vector3 instantiatePosition = position;
                instantiatePosition.y += 0.5f;
                Instantiate(props[randomInstID], instantiatePosition, transform.rotation, null);
            }
        }
    }
}
