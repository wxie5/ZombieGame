using UnityEngine;

namespace Utils.MathTool
{
    /// <summary>
    /// some math tools that is helpful for this prototype project
    /// </summary>
    public class MathTool : MonoBehaviour
    {
        /// <summary>
        /// get a float, return either -1, 0, or 1
        /// </summary>
        /// <param name="inputFloat"></param>
        /// <returns></returns>
        public static float NormalizedFloat(float inputFloat)
        {
            if (inputFloat == 0f) { return 0f; }
            return inputFloat > 0f ? 1f : -1f;
        }

        /// <summary>
        /// Add a deltaTime to the timer, and ensure there is no overflow
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float TimerAddition(float timer, float t)
        {
            return Mathf.Min(timer + Time.deltaTime, t);
        }

        /// <summary>
        /// return the subtracted value, never go below 0
        /// </summary>
        /// <param name="current"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static float NonNegativeSub(float current, float amount)
        {
            return Mathf.Max(0f, current - amount);
        }

        public static int NonNegativeSub(int current, int amount)
        {
            return Mathf.Max(0, current - amount);
        }

        /// <summary>
        /// return the added value, never go above floor
        /// </summary>
        /// <param name="current"></param>
        /// <param name="amount"></param>
        /// <param name="floorValue"></param>
        /// <returns></returns>
        public static float NonOverflowAdd(float current, float amount, float floorValue)
        {
            return Mathf.Min(current + amount, floorValue);
        }
    }
}
