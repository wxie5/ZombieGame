using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    public enum PropsType
    {
        BloodRecover,
        DamageIncrease,
        MoveSpeedIncrease,
        BulletNumberIncrease,
        OffSetDecrease,
        ShotRateDecrease,
    };

    [SerializeField] private float props_exists_time = 10;
    private float timer = 0;

    [SerializeField] private PropsType propsType;
    [SerializeField] private float recoverAmount = 30;
    [SerializeField] private float damageIncreasePercentage = 0.1f;
    [SerializeField] private float moveSpeedIncreasePercentage = 0.1f;
    [SerializeField] private int bulletNumberIncreaseNumber = 1;
    [SerializeField] private float offSetDecreasePercentage = 0.1f;
    [SerializeField] private float ShotRateDecreasePercentage = 0.1f;
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer > props_exists_time)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (propsType == PropsType.BloodRecover)
        {
            other.GetComponent<PlayerStats>().Recover(recoverAmount);
        }
        if (propsType == PropsType.DamageIncrease)
        {
            other.GetComponent<PlayerStats>().ChangeDamage(damageIncreasePercentage);
        }
        if (propsType == PropsType.MoveSpeedIncrease)
        {
            other.GetComponent<PlayerStats>().ChangeMoveSpeed(moveSpeedIncreasePercentage);
        }
        if (propsType == PropsType.BulletNumberIncrease)
        {
            other.GetComponent<PlayerStats>().ChangeBulletNumber(bulletNumberIncreaseNumber);
        }
        if (propsType == PropsType.OffSetDecrease)
        {
            other.GetComponent<PlayerStats>().ChangeOffset(offSetDecreasePercentage);
        }
        if (propsType == PropsType.ShotRateDecrease)
        {
            other.GetComponent<PlayerStats>().ChangeShotRate(ShotRateDecreasePercentage);
        }
        Destroy(gameObject);
    }
}
