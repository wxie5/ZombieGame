using Utils.MathTool;
using UnityEngine;

public class Props_Ammo : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        if (other.GetComponent<PlayerStats>().IsSingleWeapon())
        {
            other.GetComponent<PlayerStats>().AmmoCaps[0] += (other.GetComponent<PlayerStats>().GunInfos[0].cartridgeCapacity + other.GetComponent<PlayerStats>().AmmoCapInc) * (int)amount;
        }
        else
        {
            for (int i = 0; i < other.GetComponent<PlayerStats>().GunInfos.Length; i++)
            {
                other.GetComponent<PlayerStats>().AmmoCaps[i] += (other.GetComponent<PlayerStats>().GunInfos[i].cartridgeCapacity + other.GetComponent<PlayerStats>().AmmoCapInc) * (int)amount;
            }
        }
    }
}
