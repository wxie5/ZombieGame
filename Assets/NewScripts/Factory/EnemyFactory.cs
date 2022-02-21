using UnityEngine;
using System.Collections.Generic;
using Model.EnemyModel;
using View.EnemyView;
using Controller.EnemyController;

namespace Factory
{
    public class EnemyFactory : BaseFactory
    {
        private Dictionary<int, EnemyStatsCSV> enemyStatsBuffer;

        protected override void Start()
        {
            filePath = "Prefabs/Enemy/";

            enemyStatsBuffer = CSVReader.ReadEnemyStatsCSV();
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

            // set up model event (this is also important, for UI especially)
            model.OnCurHealthChange += view.HPBarChange;
            model.OnDead += view.HPBarHide;
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

            // set up view event (this is important for VFX, audio effects, etc)
            view.OnBoomStart += GameFactoryManager.Instance.VFXFact.InstBloodExplosion;
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

            // set up view event (this is important for VFX, audio effects, etc)
            view.OnShotProjectile += GameFactoryManager.Instance.ProjFact.InstPosionProj;
        }
        #endregion
    }
}
