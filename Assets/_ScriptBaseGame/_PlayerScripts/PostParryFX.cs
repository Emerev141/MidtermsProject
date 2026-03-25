using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ParryFlashFX : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float flashDuration = 0.2f;

    private ColorAdjustments colorAdjust;

    void Awake()
    {
        if (volume != null)
        {
            volume.profile.TryGet(out colorAdjust);
        }
    }

    public void TriggerFlash()
    {
        if (colorAdjust != null)
            StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / flashDuration;

            // Fade from white to normal
            colorAdjust.colorFilter.value = Color.Lerp(Color.white, Color.clear, p);

            yield return null;
        }

        // Reset
        colorAdjust.colorFilter.value = Color.clear;
    }
}
