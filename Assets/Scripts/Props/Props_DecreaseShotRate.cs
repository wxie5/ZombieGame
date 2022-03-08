using Utils.MathTool;
using UnityEngine;

public class Props_DecreaseShotRate : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        if (other.GetComponent<PlayerStats>().CurrentPropsNumber_DecreaseShotRate >= other.GetComponent<PlayerStats>().MaximumPropsNumber)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>().AddScore(500);
            }
        }
        else
        {
            other.GetComponent<PlayerStats>().ShotRateMulti = MathTool.NonNegativeSub(other.GetComponent<PlayerStats>().ShotRateMulti, amount);
            other.GetComponent<PlayerStats>().CurrentShotRate = other.GetComponent<PlayerStats>().ShotRateMulti * other.GetComponent<PlayerStats>().GunInfos[other.GetComponent<PlayerStats>().CurrentGunIndex].shotRate;
            other.GetComponent<PlayerStats>().CurrentPropsNumber_DecreaseShotRate++;
        }
    }
}
