//This script is wrote by Wei Xie

using System;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    [SerializeField] private Gun gunItemInfo;

    public Gun GunItemInfo
    {
        get { return gunItemInfo; }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            // show the weapon info UI
        }
        else
        {
            // turn off weapon info UI
        }
    }
}
