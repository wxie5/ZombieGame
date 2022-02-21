using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Test Script, wrote by Jiacheng Sun
public class StageChange : MonoBehaviour
{
    [SerializeField] private int stage;
    private void OnTriggerEnter(Collider other)
    {
        StoryMode2Manager.Instance.changeStage(stage);
    }
}
