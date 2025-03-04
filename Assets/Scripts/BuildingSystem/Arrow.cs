using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public Image ArrowImage; // Reference to the Image component of the arrow
    private float currentFillAmount = 0f; // Track current fill amount
    private float targetFillAmount = 0f; // Target fill amount

    private void Start()
    {
        // Ensure the fill amount starts at 0
        ArrowImage.fillAmount = 0f;
    }

    public void StopAnimation()
    {
        ArrowImage.fillAmount = 0f; // Reset the fill amount to 0
        targetFillAmount = 0f;
        DestroyImmediate(gameObject); // Destroy the arrow immediately
    }

    // Lerp the fill amount based on the target
    private void Update()
    {
        if (ArrowImage.fillAmount != targetFillAmount)
        {
            ArrowImage.fillAmount = targetFillAmount;
        }
    }

    // Set the target fill amount (to 1f for full)
    public void UpdateFillAmount(float target, float duration)
    {
        targetFillAmount = target;
    }
}
