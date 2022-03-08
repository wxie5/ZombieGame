using Utils.MathTool;
using UnityEngine;


public class Props_BloodPack : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        other.GetComponent<PlayerStats>().Recover(amount);
    }
}
