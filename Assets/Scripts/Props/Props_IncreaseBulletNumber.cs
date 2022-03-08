using Utils.MathTool;
using UnityEngine;


public class Props_IncreaseBulletNumber : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        if (other.GetComponent<PlayerStats>().CurrentPropsNumber_IncreaseAmmoCapacity >= other.GetComponent<PlayerStats>().MaximumPropsNumber)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>().AddScore(500);
            }
        }
        else
        {
            other.GetComponent<PlayerStats>().AmmoCapInc += (int)amount;
            other.GetComponent<PlayerStats>().CurrentPropsNumber_IncreaseAmmoCapacity++;
        }
    }
}
