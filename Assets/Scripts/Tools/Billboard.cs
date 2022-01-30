using UnityEngine;

/// <summary>
/// attach this script to any world space UI element to let it look at the main camera
/// </summary>
public class Billboard : MonoBehaviour
{
    private Transform camTrans;

    private void Start()
    {
        camTrans = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + camTrans.forward);
    }
}
