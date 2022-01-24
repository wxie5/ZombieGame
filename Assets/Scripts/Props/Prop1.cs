using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Props get!");
        Destroy(gameObject);
    }
}
