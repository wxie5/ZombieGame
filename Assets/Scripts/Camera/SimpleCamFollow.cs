using UnityEngine;
//This Script is create and wrote by Wei Xie
public class SimpleCamFollow : MonoBehaviour
{
    private Transform playerTrans;

    public Transform PlayerTrans
    {
        set { playerTrans = value; }
    }

    private void LateUpdate()
    {
        transform.position = playerTrans.position;
    }
}