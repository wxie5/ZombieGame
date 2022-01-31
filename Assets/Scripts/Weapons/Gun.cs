//This script is wrote by Wei Xie

using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Gun")]
public class Gun : ScriptableObject
{
    public GunType gunType;
    public float shotRate;
    public float shotRange;
    public float damage;
    public int cartridgeCapacity;
    public int ammoCapacity;
    public int weaponID;
    public Vector2 offset;
    public GameObject gunPrefab;

    // these two vectors stores the best position and rotation for player to hold the current gun
    // because these data are got by testing, don't change them once they are assigned
    public Vector3 handPosition;
    public Vector3 handRotation;
}

public enum GunType
{
    EmptyGun = 0,
    AssaultRifle = 1,
    Handgun = 2
}
