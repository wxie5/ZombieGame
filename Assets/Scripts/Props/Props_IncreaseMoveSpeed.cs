using Utils.MathTool;
using UnityEngine;

public class Props_IncreaseMoveSpeed : Props
{
    protected override void TakeEffects(float amount, Collider other)
    {
        if (other.GetComponent<PlayerStats>().CurrentPropsNumber_IncreaseMoveSpeed >= other.GetComponent<PlayerStats>().MaximumPropsNumber)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>().AddScore(500);
            }
        }
        else
        {
            other.GetComponent<PlayerStats>().MoveSpeedMulti = MathTool.NonNegativeSub(other.GetComponent<PlayerStats>().MoveSpeedMulti, -amount);
            other.GetComponent<PlayerStats>().CurrentMoveSpeed = other.GetComponent<PlayerStats>().MoveSpeedMulti * other.GetComponent<PlayerStats>().MaxMoveSpeed;
            other.GetComponent<PlayerStats>().CurrentPropsNumber_IncreaseMoveSpeed++;
        }
    }
}
