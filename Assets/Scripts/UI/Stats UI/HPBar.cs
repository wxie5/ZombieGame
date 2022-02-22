using UnityEngine;

// This script is worte by Wei Xie
public class HPBar : MonoBehaviour
{
    [SerializeField] private GameObject hpBarGO;
    [SerializeField] private RectTransform fill;
    [SerializeField] private RectTransform inFill;
    [SerializeField] private float smoothTime = 0.2f;

    private float smoothVelocityRef;

    public void SetFill(float currentHP, float maxHP)
    {
        // use clamp01 to avoid some error case (ex: currentHP is greater than maxHP)
        float normalizedX = Mathf.Clamp01(currentHP / maxHP);

        fill.localScale = new Vector3(normalizedX, fill.localScale.y, fill.localScale.z);
    }

    public void DisableHPBar()
    {
        hpBarGO.SetActive(false);
    }

    private void Update()
    {
        if (!hpBarGO.activeSelf) { return; }

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
