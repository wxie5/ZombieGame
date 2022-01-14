using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public GameObject[] m_treasurePrefab;
    private Transform m_treasureTransform;
    private bool hasDead = false; //make sure the props only appear once
    // Update is called once per frame
    void Update()
    {
        if(!hasDead)
        {
            if (gameObject.GetComponent<SimpleAI>().IsDead)
            {
                hasDead = true;
                m_treasureTransform = gameObject.transform;
                Instantiate(m_treasurePrefab[Random.Range(0, m_treasurePrefab.Length)], m_treasureTransform);
            }
        }
    }
}
