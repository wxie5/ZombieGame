using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //Some buff multipliers that the player comes with, the initial value is 1, which will be increased by the effect of props. The value in the game is the multiplier * actual value.
    [SerializeField] private Image mask; //mask for HealthBar UI
    [SerializeField] private float m_MaxHP = 100;
    private float m_CurrentHP;
    private float m_ShotRate = 1; //The shoot rate multipliers, which is faster and faster from 1->0.
    private float m_BasicDamage = 1;
    private float m_MoveSpeed = 1;
    private int m_BulletNumber = 1;
    private float m_Offset = 1; //The offset when shooting, which is more and more accurate from 1->0.
    private bool m_IsDead = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentHP = m_MaxHP;
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHP -= amount;
    }

    public void Recover(float amount)
    {
        m_CurrentHP += amount;
        if(m_CurrentHP >= m_MaxHP)
        {
            m_CurrentHP = m_MaxHP;
        }
    }


    //Here are some functions that interact with the props class that can affect the multipliers value.
    public void ChangeShotRate(float amount)
    {
        m_ShotRate *= (1 + amount);
    }

    public void ChangeBasicDamage(float amount)
    {
        m_BasicDamage *= (1 + amount);
    }

    public void ChangeMoveSpeed(float amount)
    {
        m_MoveSpeed *= (1 - amount);
    }

    public void ChangeBulletNumber(int amount)
    {
        m_BulletNumber += amount;
    }

    public void ChangeOffset(float amount)
    {
        m_Offset *= (1 - amount);
    }


    //Here are some methods for returning member variable values to other classes.
    public float MaxHP()
    {
        return m_MaxHP;
    }    
    public float CurrentHP()
    {
        return m_CurrentHP;
    }

    public float ShotRate()
    {
        return m_ShotRate;
    }
    
    public float BasicDamage()
    {
        return m_BasicDamage;
    }

    public float MoveSpeed()
    {
        return m_MoveSpeed;
    }

    public int BulletNumber()
    {
        return m_BulletNumber;
    }

    public float Offset()
    {
        return m_Offset;
    }    

    public bool IsDead()
    {
        return m_IsDead;
    }
}
