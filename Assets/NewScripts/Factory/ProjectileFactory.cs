using UnityEngine;

namespace Factory
{
    public class ProjectileFactory : BaseFactory
    {
        protected override void Start()
        {
            filePath = "Prefabs/Projectile/";
        }

        public void InstPosionProj(Vector3 pos, Quaternion dir, float dmg)
        {
            InstProj(pos, dir, dmg, "PosionProjectile");
        }

        private void InstProj(Vector3 pos, Quaternion dir, float dmg, string fileName)
        {
            GameObject projPrefab = Resources.Load<GameObject>(filePath + fileName);
            GameObject projGO = Instantiate(projPrefab, pos, dir);
            projGO.GetComponent<Projectile>().Damage = dmg;
        }
    }
}
