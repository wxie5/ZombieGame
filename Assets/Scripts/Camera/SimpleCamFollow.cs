using UnityEngine;

public class SimpleCamFollow : MonoBehaviour
{
    public Transform playerTrans;

    private void LateUpdate()
    {
        transform.position = playerTrans.position;
    }
}