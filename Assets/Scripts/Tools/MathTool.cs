using UnityEngine;

namespace Prototype.Tool
{
    public class MathTool : MonoBehaviour
    {
        public static float NormalizedFloat(float inputFloat)
        {
            if (inputFloat == 0f) { return 0f; }
            return inputFloat > 0f ? 1f : -1f;
        }
    }
}
