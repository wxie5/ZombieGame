using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Gun")]
public class Gun : ScriptableObject
{
    public GunType gunType;
    public float shotRate;
    public float shotRange;
    public float damage;
    public Vector2 offset;
    public GameObject gunPrefab;
    public Vector3 handPosition;
    public Vector3 handRotation;
}

public enum GunType
{
    AssaultRifle = 0,
    Handgun = 1
}
