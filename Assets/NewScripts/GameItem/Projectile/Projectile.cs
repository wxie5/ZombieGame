using UnityEngine;

// later we will change this into object pool for optimization
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    protected float damage = 10f;

    public float Damage
    {
        set { damage = value; }
    }

    protected void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
