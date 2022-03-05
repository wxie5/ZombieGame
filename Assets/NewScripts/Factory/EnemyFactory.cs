using UnityEngine;
using System.Collections.Generic;
using Model.EnemyModel;
using View.EnemyView;
using Controller.EnemyController;
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
        private MultiplayerEndlessModeManager multiplayerEndlessModeManager;
        private AIMultiplayerEndlessModeManager aIMultiplayerEndlessMode;

        protected override void Start()
        {
            filePath = "Prefabs/Enemy/";
            enemyStatsBuffer = CSVReader.ReadEnemyStatsCSV();
            if(GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                endlessModeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>();
            }
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<MultiplayerEndlessModeManager>())
            {
                multiplayerEndlessModeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MultiplayerEndlessModeManager>();
            }
            if(GameObject.FindGameObjectWithTag("Manager").GetComponent<AIMultiplayerEndlessModeManager>())
            {
                aIMultiplayerEndlessMode = GameObject.FindGameObjectWithTag("Manager").GetComponent<AIMultiplayerEndlessModeManager>();
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
        #endregion

        #region Private Zombie Instantiator (For Different Type)
        private void InstantiateNormalZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyStatsCSV stats = enemyStatsBuffer[id];
            EnemyBaseModel model = new EnemyBaseModel(id, stats.enemyName, stats.baseHealth, stats.moveSpeed,
                stats.baseDmg, stats.alertDistance, stats.attackRange, stats.stopDistance, stats.attackRate,
                stats.attackRateDefault, stats.score, stats.baseHealthMulti, stats.baseDmgMulti, stats.defaultLevel,
                stats.maxLevel);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyZombieView view = zombieGO.GetComponent<EnemyZombieView>();

            // get controller
            EnemyBaseController<EnemyZombieView> controller = new EnemyBaseController<EnemyZombieView>(view, model);

            // set up view (this step is extremely important)
            view.SetUp(controller);

            // set up view events
            view.OnDead += InstantiateProps;

            // set up model event (this is also important, for UI especially)
            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;
            if (endlessModeManager != null)
            {
                model.OnDead += endlessModeManager.onDeadAddScore;
            }
            if (multiplayerEndlessModeManager != null)
            {
                model.OnDead += multiplayerEndlessModeManager.onDeadAddScore;
            }
            if (aIMultiplayerEndlessMode != null)
            {
                model.OnDead += aIMultiplayerEndlessMode.onDeadAddScore;
            }
            model.OnDead += OndeadHandler;

            zombieCount++;
        }

        private void InstantiateBoomerZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyStatsCSV stats = enemyStatsBuffer[id];
            EnemyBaseModel model = new EnemyBaseModel(id, stats.enemyName, stats.baseHealth, stats.moveSpeed,
                stats.baseDmg, stats.alertDistance, stats.attackRange, stats.stopDistance, stats.attackRate,
                stats.attackRateDefault, stats.score, stats.baseHealthMulti, stats.baseDmgMulti, stats.defaultLevel,
                stats.maxLevel);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyBoomerView view = zombieGO.GetComponent<EnemyBoomerView>();

            // get controller
            EnemyBaseController<EnemyBoomerView> controller = new EnemyBaseController<EnemyBoomerView>(view, model);

            // set up view (this step is extremely important)
            view.SetUp(controller);

            // set up model event (this is also important, for UI, audio effects, especially)
            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;
            if (endlessModeManager != null)
            {
                model.OnDead += endlessModeManager.onDeadAddScore;
            }
            if (multiplayerEndlessModeManager != null)
            {
                model.OnDead += multiplayerEndlessModeManager.onDeadAddScore;
            }
            if (aIMultiplayerEndlessMode != null)
            {
                model.OnDead += aIMultiplayerEndlessMode.onDeadAddScore;
            }
            model.OnDead += OndeadHandler;

            // set up view event (this is important for VFX, audio effects, etc)
            view.OnBoomStart += GameFactoryManager.Instance.VFXFact.InstBloodExplosion;
            view.OnDead += InstantiateProps;

            zombieCount++;
        }

        private void InstantiatePosionZombie(Vector3 instantiatePos, int id, string prefabName)
        {
            // get model
            EnemyStatsCSV stats = enemyStatsBuffer[id];
            EnemyBaseModel model = new EnemyBaseModel(id, stats.enemyName, stats.baseHealth, stats.moveSpeed,
                stats.baseDmg, stats.alertDistance, stats.attackRange, stats.stopDistance, stats.attackRate,
                stats.attackRateDefault, stats.score, stats.baseHealthMulti, stats.baseDmgMulti, stats.defaultLevel,
                stats.maxLevel);

            // get view
            GameObject zombiePrefab = Resources.Load<GameObject>(filePath + prefabName);
            GameObject zombieGO = Instantiate(zombiePrefab, instantiatePos, Quaternion.identity);
            EnemyPosionView view = zombieGO.GetComponent<EnemyPosionView>();

            // get controller
            EnemyBaseController<EnemyPosionView> controller = new EnemyBaseController<EnemyPosionView>(view, model);

            // set up view (this step is extremely important)
            view.SetUp(controller);

            // set up model event (this is also important, for UI, audio effects, especially)
            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;
            if (endlessModeManager != null)
            {
                model.OnDead += endlessModeManager.onDeadAddScore;
            }
            if (multiplayerEndlessModeManager != null)
            {
                model.OnDead += multiplayerEndlessModeManager.onDeadAddScore;
            }
            if (aIMultiplayerEndlessMode != null)
            {
                model.OnDead += aIMultiplayerEndlessMode.onDeadAddScore;
            }
            model.OnDead += OndeadHandler;

            // set up view event (this is important for VFX, audio effects, etc)
            view.OnShotProjectile += GameFactoryManager.Instance.ProjFact.InstPosionProj;
            view.OnDead += InstantiateProps;

            zombieCount++;
        }
        #endregion
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
