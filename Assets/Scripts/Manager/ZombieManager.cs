using System;
using UnityEngine;

[Serializable]
public class ZombieManager
{
    public GameObject m_ZombieType;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_ZombieNumber;
    [HideInInspector] public GameObject m_Instance;
}
