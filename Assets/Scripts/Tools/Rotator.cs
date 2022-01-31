using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 45f;

    private void Update()
    {
        transform.Rotate(new Vector3(0f, rotateSpeed, 0f) * Time.deltaTime);
    }
}
