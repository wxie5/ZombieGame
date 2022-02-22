using UnityEngine;

namespace Factory
{
    public class VFXFactory : BaseFactory
    {
        protected override void Start()
        {
            filePath = "Prefabs/VFX/";
        }

        public void InstBloodExplosion(Vector3 pos)
        {
            InstVFX(pos, "BloodExplosion");
        }

        private void InstVFX(Vector3 pos, string fileName)
        {
            GameObject vfxPrefab = Resources.Load<GameObject>(filePath + fileName);
            Instantiate(vfxPrefab, pos, Quaternion.identity);
        }
    }
}
