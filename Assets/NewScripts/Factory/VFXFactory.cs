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
            InstVFXNotLoop(pos, "BloodExplosion");
        }

        private void InstVFXNotLoop(Vector3 pos, string fileName)
        {
            GameObject vfxPrefab = Resources.Load<GameObject>(filePath + fileName);
            Instantiate(vfxPrefab, pos, Quaternion.identity);
        }
    }
}
