using Utils.MathTool;
using UnityEngine;
public class Props_DecreaseOffset : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        if (other.GetComponent<PlayerStats>().CurrentPropsNumber_DecreaseOffset >= other.GetComponent<PlayerStats>().MaximumPropsNumber)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>().AddScore(500);
            }
        }
        else
        {
            other.GetComponent<PlayerStats>().ShotOffsetMulti = MathTool.NonNegativeSub(other.GetComponent<PlayerStats>().ShotOffsetMulti, amount);
            other.GetComponent<PlayerStats>().CurrentShotOffset = other.GetComponent<PlayerStats>().ShotOffsetMulti * other.GetComponent<PlayerStats>().GunInfos[other.GetComponent<PlayerStats>().CurrentGunIndex].offset;
            other.GetComponent<PlayerStats>().CurrentPropsNumber_DecreaseOffset++;
        }
    }
}
