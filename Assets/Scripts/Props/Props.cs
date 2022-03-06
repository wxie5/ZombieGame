using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is create and wrote by Jiacheng Sun

//Basiclly just add effects to props to change the player's basic attributes.
public class Props : MonoBehaviour
{
    public enum PropsType
    {
        BloodRecover,
        DamageIncrease,
        MoveSpeedIncrease,
        AmmoCapacityIncrease,
        OffSetDecrease,
        ShotRateDecrease,
        Ammo
    };

    //[SerializeField] private float props_exists_time = 15;
    //private float timer = 0;
    [SerializeField] private AudioClip getPropsSE;
    [SerializeField] private PropsType propsType;
    [SerializeField] private float recoverAmount = 30;
    [SerializeField] private float damageIncreasePercentage = 0.1f;
    [SerializeField] private float moveSpeedIncreasePercentage = 0.1f;
    [SerializeField] private int ammoCapacityIncreaseNumber = 2;
    [SerializeField] private float offSetDecreasePercentage = 0.1f;
    [SerializeField] private float ShotRateDecreasePercentage = 0.1f;
    [SerializeField] private int AmmoNumberIncreaseAmount = 3;
    private void FixedUpdate()
    {
        //timer += Time.fixedDeltaTime;
        //if(timer > props_exists_time)
        //{
            //Destroy(gameObject);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(getPropsSE, gameObject.transform.position);
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
        if (propsType == PropsType.AmmoCapacityIncrease)
        {
            other.GetComponent<PlayerStats>().ChangeAmmoCapcity(ammoCapacityIncreaseNumber);
        }
        if (propsType == PropsType.OffSetDecrease)
        {
            other.GetComponent<PlayerStats>().ChangeOffset(offSetDecreasePercentage);
        }
        if (propsType == PropsType.ShotRateDecrease)
        {
            other.GetComponent<PlayerStats>().ChangeShotRate(ShotRateDecreasePercentage);
        }
        if (propsType == PropsType.Ammo)
        {
            other.GetComponent<PlayerStats>().ChangeAmmoAmount(AmmoNumberIncreaseAmount);
        }
        Destroy(gameObject);
    }
}
