using UnityEngine;

//This script is created and wrote by Wei Xie
public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 45f;

    private void Update()
    {
        transform.Rotate(new Vector3(0f, rotateSpeed, 0f) * Time.deltaTime);
    }
}
