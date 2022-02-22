using System;

namespace Model.PlayerModel
{
    [Serializable]
    public class PlayerModel
    {
        #region Player Stats
        // identity stats (primary key)
        private int playerID;

        // base stats
        private float baseHealth;
        private float baseMoveSpeed;
        private float baseShotRate;
        private float baseDmg;
        private float baseShotOffset;

        // current stats
        private float curHealth;
        private float curMoveSpeed;
        private float curShotRate;
        private float curDmg;
        private float curShotOffset;

        // multiplier
        private float moveSpeedMulti = 1f;
        private float shotRateMulti = 1f;
        private float dmgMulti = 1f;
        private float shotOffsetMulti = 1f;
        #endregion

        #region Constructor
        public PlayerModel(int playerID, float baseHealth, float baseMoveSpeed)
        {
            this.playerID = playerID;

            BaseHealth = baseHealth;
            BaseMoveSpeed = baseMoveSpeed;

            CurHealth = BaseHealth;
            CurMoveSpeed = BaseMoveSpeed;

            MoveSpeedMulti = 1f;
            ShotRateMulti = 1f;
            DmgMulti = 1f;
            ShotOffsetMulti = 1f;
        }
        #endregion

        #region Public Attributes
        public float PlayerID
        {
            get { return playerID; }
        }
        public float BaseHealth
        {
            get { return baseHealth; }
            set
            {
                baseHealth = value;
                OnBaseHealthChange?.Invoke(baseHealth);
            }
        }
        public float BaseMoveSpeed
        {
            get { return baseMoveSpeed; }
            set
            {
                baseMoveSpeed = value;
                OnBaseMoveSpeedChange?.Invoke(baseMoveSpeed);
            }
        }
        public float BaseShotRate
        {
            get { return baseShotRate; }
            set
            {
                baseShotRate = value;
                OnBaseShotRateChange?.Invoke(baseShotRate);
            }
        }
        public float BaseDmg
        {
            get { return baseDmg; }
            set
            {
                baseDmg = value;
                OnBaseDmgChange?.Invoke(baseDmg);
            }
        }
        public float BaseShotOffset
        {
            get { return baseHealth; }
            set
            {
                baseShotOffset = value;
                OnBaseShotOffsetChange?.Invoke(baseShotOffset);
            }
        }
        public float CurHealth
        {
            get { return curHealth; }
            set
            {
                curHealth = value;
                OnCurHealthChange?.Invoke(curHealth);
            }
        }
        public float CurMoveSpeed
        {
            get { return curMoveSpeed; }
            set
            {
                curMoveSpeed = value;
                OnCurMoveSpeedChange?.Invoke(curMoveSpeed);
            }
        }
        public float CurShotRate
        {
            get { return curShotRate; }
            set
            {
                curShotRate = value;
                OnCurShotRateChange?.Invoke(curShotRate);
            }
        }
        public float CurDmg
        {
            get { return curDmg; }
            set
            {
                curDmg = value;
                OnCurDmgChange?.Invoke(curDmg);
            }
        }
        public float CurShotOffset
        {
            get { return curShotOffset; }
            set
            {
                curShotOffset = value;
                OnCurShotOffsetChange?.Invoke(curShotOffset);
            }
        }
        public float MoveSpeedMulti
        {
            get { return moveSpeedMulti; }
            set
            {
                moveSpeedMulti = value;
                OnMoveSpeedMultiChange?.Invoke(moveSpeedMulti);
                CurMoveSpeed = moveSpeedMulti * baseMoveSpeed;
            }
        }
        public float ShotRateMulti
        {
            get { return shotRateMulti; }
            set
            {
                shotRateMulti = value;
                curShotRate = shotRateMulti * baseShotRate;
                OnShotRateMultiChange?.Invoke(shotRateMulti);
                OnCurShotRateChange?.Invoke(curShotRate);
            }
        }
        public float DmgMulti
        {
            get { return dmgMulti; }
            set
            {
                dmgMulti = value;
                curDmg = dmgMulti * baseDmg;
                OnDmgMultiChange?.Invoke(dmgMulti);
                OnCurDmgChange?.Invoke(curDmg);
            }
        }
        public float ShotOffsetMulti
        {
            get { return shotOffsetMulti; }
            set
            {
                shotOffsetMulti = value;
                curShotOffset = shotOffsetMulti * shotOffsetMulti;
                OnShotOffsetMultiChange?.Invoke(shotOffsetMulti);
                OnCurShotOffsetChange?.Invoke(curShotOffset);
            }
        }
        #endregion

        #region Events(Observers)
        public delegate void FloatValueChange(float newValue);

        public event FloatValueChange OnBaseHealthChange;
        public event FloatValueChange OnBaseMoveSpeedChange;
        public event FloatValueChange OnBaseShotRateChange;
        public event FloatValueChange OnBaseDmgChange;
        public event FloatValueChange OnBaseShotOffsetChange;

        public event FloatValueChange OnCurHealthChange;
        public event FloatValueChange OnCurMoveSpeedChange;
        public event FloatValueChange OnCurShotRateChange;
        public event FloatValueChange OnCurDmgChange;
        public event FloatValueChange OnCurShotOffsetChange;

        public event FloatValueChange OnMoveSpeedMultiChange;
        public event FloatValueChange OnShotRateMultiChange;
        public event FloatValueChange OnDmgMultiChange;
        public event FloatValueChange OnShotOffsetMultiChange;
        #endregion
    }
}
