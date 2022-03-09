using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Test Script, wrote by Jiacheng Sun
public class ArriveDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<StoryMode2Manager>().ArriveDoor = true;
    }
}
