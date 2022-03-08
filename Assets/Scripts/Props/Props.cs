using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is create and wrote by Jiacheng Sun

public class Props : MonoBehaviour
{
    [SerializeField] private AudioClip getPropsSE;
    [SerializeField] private float amount;

    protected void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(getPropsSE, gameObject.transform.position);

        TakeEffects(amount, other);
        Destroy(gameObject);
    }

    protected virtual void TakeEffects(float amount, Collider other)
    { }

}
