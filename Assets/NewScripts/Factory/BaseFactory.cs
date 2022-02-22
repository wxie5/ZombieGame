using UnityEngine;

namespace Factory
{
    public class BaseFactory : MonoBehaviour, IFactory
    {
        protected string filePath;

        protected virtual void Start() { }
    }
}
