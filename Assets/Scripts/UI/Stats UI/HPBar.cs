using UnityEngine;
using UnityEngine.UI;

// This script is worte by Wei Xie
/// <summary>
/// this class can be inherited to decide many other factors (whose hp bar, when to modify hp, etc.)
/// </summary>
public class HPBar : MonoBehaviour
{
    [SerializeField] private RectTransform fill;
    [SerializeField] private RectTransform inFill;
    [SerializeField] private float smoothTime = 0.2f;

    private float smoothVelocityRef;

    protected void SetFill(float currentHP, float maxHP)
    {
        // use clamp01 to avoid some error case (ex: currentHP is greater than maxHP)
        float normalizedX = Mathf.Clamp01(currentHP / maxHP);

        fill.localScale = new Vector3(normalizedX, fill.localScale.y, fill.localScale.z);
    }

    private void Update()
    {
        SmoothUpdateInFill();
    }

    private void SmoothUpdateInFill()
    {
        float currentX = inFill.localScale.x;
        float targetX = fill.localScale.x;
        float smoothX = Mathf.SmoothDamp(currentX, targetX, ref smoothVelocityRef, smoothTime);
        inFill.localScale = new Vector3(smoothX, inFill.localScale.y, inFill.localScale.z);
    }
}
