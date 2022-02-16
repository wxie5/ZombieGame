using UnityEngine;

namespace View.PlayerView
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private PlayerID playerNum = PlayerID.PlayerA;
        [SerializeField] private float playerRotSmoothTime = 0.02f;
        [Range(9.81f, 20f)]
        [SerializeField] private float posGravity = 9.81f;

        [Header("Combat Properties")]
        [SerializeField] private Transform shotTrans;
        [SerializeField] private Transform gunSlotTrans;

        [Header("Scene Interaction")]
        [SerializeField] private float pickUpRange = 1.5f;

        [Header("Target Lock Properties")]
        [SerializeField] private float lockRange = 10f;
        [SerializeField] private float autoLoseTargetDelay = 1f;

        [Header("Layer Mask: Select All That Apply")]
        [SerializeField] private LayerMask lockableLayer;
        [SerializeField] private LayerMask bulletHitableLayer;
        [SerializeField] private LayerMask weaponLayer;
    }
}
