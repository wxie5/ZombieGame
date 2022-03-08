using Utils.MathTool;
using UnityEngine;

public class Props_IncreaseDamage : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        if (other.GetComponent<PlayerStats>().CurrentPropsNumber_IncreaseDamage >= other.GetComponent<PlayerStats>().MaximumPropsNumber)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>().AddScore(500);
            }
        }
        else
        {
            other.GetComponent<PlayerStats>().DamageRateMulti = MathTool.NonNegativeSub(other.GetComponent<PlayerStats>().DamageRateMulti, -amount);
            other.GetComponent<PlayerStats>().CurrentDamage = other.GetComponent<PlayerStats>().DamageRateMulti * other.GetComponent<PlayerStats>().GunInfos[other.GetComponent<PlayerStats>().CurrentGunIndex].damage;
            other.GetComponent<PlayerStats>().CurrentPropsNumber_IncreaseDamage++;

        }
    }
}
