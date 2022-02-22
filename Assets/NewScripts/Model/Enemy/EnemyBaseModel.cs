using System;

namespace Model.EnemyModel
{
    [Serializable]
    public class EnemyBaseModel : IEnemyModel
    {
        #region Enemy Stats
        // identity stats (primary key & candidate keys)
        private int enemyID;
        private string enemyName;

        // base stats
        private float baseHealth;
        //private float baseMoveSpeed;
        private float baseDmg;
        //private float baseAttackRate;
        private int score;
        private int maxLevel;
        private float attackRateDefault;

        // current stats
        private float curMaxHealth;
        private float curHealth;
        private float curMoveSpeed;
        private float curDmg;
        private float curAttackRate;
        private int curLevel;
        private bool isDead;

        // AI stats
        private float alertDistance;
        private float attackRange;
        private float stopDistance;

        // multiplier
        private float baseHealthMulti;
        private float baseDmgMulti;
        #endregion

        #region Constructor
        public EnemyBaseModel(int enemyID, string enemyName, float baseHealth, float moveSpeed, float baseDmg, float alertDistance,
            float attackRange, float stopDistance, float attackRate, float attackRateDefault, int score, float baseHealthMulti,
            float baseDmgMulti, int defaultLevel, int maxLevel)
        {
            this.enemyID = enemyID;
            this.enemyName = enemyName;
            this.baseHealth = baseHealth;
            this.curMoveSpeed = moveSpeed;
            this.baseDmg = baseDmg;
            this.alertDistance = alertDistance;
            this.attackRange = attackRange;
            this.stopDistance = stopDistance;
            this.curAttackRate = attackRate;
            this.attackRateDefault = attackRateDefault;
            this.score = score;
            this.baseHealthMulti = baseHealthMulti;
            this.baseDmgMulti = baseDmgMulti;
            this.maxLevel = maxLevel;
            CurLevel = defaultLevel;
            this.curHealth = this.curMaxHealth;
            this.isDead = false;
        }
        #endregion

        #region Public Attributes
        public int EnemyID
        {
            get { return enemyID; }
        }
        public string EnemyName
        {
            get { return enemyName; }
        }
        public int Score
        {
            get { return score; }
        }
        public int MaxLevel
        {
            get { return maxLevel; }
        }
        public float CurMaxHealth
        {
            get { return curMaxHealth; }
            set
            {
                curMaxHealth = value;
                OnCurMaxHealthChange?.Invoke(curMaxHealth);
            }
        }
        public float CurHealth
        {
            get { return curHealth; }
            set
            {
                curHealth = value;
                OnCurHealthChange?.Invoke(curHealth, curMaxHealth);
            }
        }
        public float CurMoveSpeed
        {
            get { return curMoveSpeed; }
        }
        public float CurDmg
        {
            get { return curDmg; }
            set
            {
                curDmg = value;
            }
        }
        public float CurAttackRate
        {
            get { return curAttackRate; }
        }
        public int CurLevel
        {
            get { return curLevel; }
            set
            {
                curLevel = value;
                CurMaxHealth = curLevel * baseHealthMulti + baseHealth;
                CurDmg = curLevel * baseDmgMulti + baseDmg;
                OnCurLevelChange?.Invoke(curLevel);
            }
        }
        public float AttackRateDefault
        {
            get { return attackRateDefault; }
        }
        public float AlertDistance
        {
            get { return alertDistance; }
        }
        public float AttackRange
        {
            get { return attackRange; }
        }
        public float StopDistance
        {
            get { return stopDistance; }
        }
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                if(isDead)
                {
                    OnDead?.Invoke(score);
                }
            }
        }
        #endregion

        #region Events(Observers)
        public delegate void FloatValueChange(float newValue);
        public delegate void DoubleFloatValueChange(float newValue1, float newValue2);
        public delegate void IntValueChange(int newValue);

        public event FloatValueChange OnCurMaxHealthChange;
        public event DoubleFloatValueChange OnCurHealthChange;

        public event IntValueChange OnCurLevelChange;
        public event IntValueChange OnDead;

        #endregion
    }
}
