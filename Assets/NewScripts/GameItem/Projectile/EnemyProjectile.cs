using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerManager>().GetHit(damage);
            Destroy(this.gameObject);
        }
        else if(coll.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
