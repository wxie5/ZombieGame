using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIManager : MonoBehaviour
{
    void Start()
    {
        GameSetting.Instance.Load();
    }
}
