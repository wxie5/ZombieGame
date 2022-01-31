using UnityEngine;

// The script is create and write by Jiacheng Sun
// modified by Wei Xie
[RequireComponent(typeof(EnemyStats))]
public class Treasure : MonoBehaviour
{
    [SerializeField] private GameObject[] treaturePrefabs;
    [SerializeField] private Transform treasureParentTrans;

    private EnemyStats enemyStats;
    private bool has_dead;

    private void Start()
    {
        has_dead = false;
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Update() // if the zombie with props dead, instantiate the props, and only instantiate once
    {
        if(!has_dead)
        {
            if (enemyStats.IsDead)
            {
                InstantiateTreasure();
                has_dead = true;
            }
        }
    }
    public void InstantiateTreasure() // instantiate the props
    {
        int randomInstID = Random.Range(0, treaturePrefabs.Length);
        Vector3 instantiatePosition = transform.position;
        instantiatePosition.y += 0.5f;
        Instantiate(treaturePrefabs[randomInstID], instantiatePosition, transform.rotation, treasureParentTrans);
    }
}
