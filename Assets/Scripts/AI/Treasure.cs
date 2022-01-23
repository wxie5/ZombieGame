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
        Instantiate(treaturePrefabs[randomInstID], transform.position, transform.rotation, treasureParentTrans);
        simpleAI.onDead -= InstantiateTreature;
    }
}
