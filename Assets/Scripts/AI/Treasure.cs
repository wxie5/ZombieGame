using UnityEngine;

[RequireComponent(typeof(SimpleAI))]
public class Treasure : MonoBehaviour
{
    [SerializeField] private GameObject[] treaturePrefabs;
    [SerializeField] private Transform treasureParentTrans;

    private SimpleAI simpleAI;

    private void Start()
    {
        simpleAI = GetComponent<SimpleAI>();
        simpleAI.onDead += InstantiateTreature;
    }

    public void InstantiateTreature()
    {
        int randomInstID = Random.Range(0, treaturePrefabs.Length);
        Vector3 instantiatePosition = transform.position;
        instantiatePosition.y += 0.5f;
        Instantiate(treaturePrefabs[randomInstID], instantiatePosition, transform.rotation, treasureParentTrans);
        simpleAI.onDead -= InstantiateTreature;
    }
}
