using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] m_zombiePrefab;
    public Transform[] m_SpawnPoint;
    public bool m_RandomSpawn;
    public ZombieManager[] m_Zombies;


    void Start()
    {
        
        SpawnZombies() ;

        //StartCoroutine(GameLoop());
    }

    private void SpawnZombies()
    {
        if (m_RandomSpawn)
        {
            int zombie_number;
            int spawn_point_number;
            for (int i = 0; i < m_Zombies.Length; i++)
            {
                zombie_number = Random.Range(0, m_zombiePrefab.Length);
                spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
                m_Zombies[i].m_Instance = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint[spawn_point_number]) as GameObject;
            }
        }
        else
        {
            for (int i = 0; i < m_Zombies.Length; i++)
            {
                m_Zombies[i].m_Instance = Instantiate(m_Zombies[i].m_ZombieType, m_Zombies[i].m_SpawnPoint) as GameObject;
            }
        }
    }

    
}
