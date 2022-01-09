using UnityEngine;

namespace Prototype.Camera
{
    public class SimpleCamFollow : MonoBehaviour
    {
        [SerializeField] private Transform playerTrans;

        private void LateUpdate()
        {
            transform.position = playerTrans.position;
        }
    }
}
