using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityPostProcessing : MonoBehaviour
{
    public Volume postProcessingVolume;
    [Range(0f, 1f)] public float sanity = 1f;

    public float lensDistortionMin = 0.5f;
    public float lensDistortionMax = -0.1f;

    public float pulseStrengthMin = 0.05f;
    public float pulseStrengthMax = 0.3f;
    public float pulseSpeedMin = 0.5f;
    public float pulseSpeedMax = 5f;

    public float vignetteIntensityMin = 0.5f;
    public float vignetteIntensityMax = 0.1f;
    public float vignetteSmoothnessMin = 1f;
    public float vignetteSmoothnessMax = 0.3f;

    public float chromaticAberrationMin = 1f;
    public float chromaticAberrationMax = 0f;

    public float saturationMin = -70f;
    public float saturationMax = 0f;

    public Color colorFilterMin = Color.red;
    public Color colorFilterMax = Color.white;

    private LensDistortion lensDistortion;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    private SanityMeter sanityMeter;

    private bool isFadingToBlack = false;
    public float fadeDuration = 3f;  // Time in seconds for fade to black

    void Start()
    {
        sanityMeter = FindObjectOfType<SanityMeter>();

        if (postProcessingVolume.profile.TryGet<LensDistortion>(out lensDistortion) &&
            postProcessingVolume.profile.TryGet<Vignette>(out vignette) &&
            postProcessingVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration) &&
            postProcessingVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            // Everything is set up correctly
        }
        else
        {
            Debug.LogError("Post Processing effects are not set up correctly in the Volume.");
        }
    }

    void Update()
    {
        if (isFadingToBlack)
        {
            FadeToBlack();
            return; // Exit the update loop if we're fading to black
        }

        sanity = sanityMeter._currentSanity / sanityMeter._maxSanity;
        sanity = Mathf.Clamp01(sanity);

        float pulseStrength = Mathf.Lerp(pulseStrengthMax, pulseStrengthMin, sanity);
        float pulseSpeed = Mathf.Lerp(pulseSpeedMax, pulseSpeedMin, sanity);

        if (lensDistortion != null)
        {
            float distortion = Mathf.Lerp(lensDistortionMin, lensDistortionMax, sanity);
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseStrength;
            lensDistortion.intensity.value = distortion + pulse;
        }

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(vignetteIntensityMin, vignetteIntensityMax, sanity);
            vignette.smoothness.value = Mathf.Lerp(vignetteSmoothnessMin, vignetteSmoothnessMax, sanity);
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberrationMin, chromaticAberrationMax, sanity);
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(saturationMin, saturationMax, sanity);

            // Start fading to black if sanity reaches 0
            if (sanity <= 0)
            {
                StartFadeToBlack();
            }
            else
            {
                colorAdjustments.postExposure.value = 0f;
            }
        }
    }

    void StartFadeToBlack()
    {
        isFadingToBlack = true;
    }

    void FadeToBlack()
    {
        // Gradually reduce the saturation and exposure over the fade duration
        float fadeProgress = Mathf.Clamp01(Time.deltaTime / fadeDuration);

        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, -10f, fadeProgress); // Lower exposure for black screen effect
        }

        //Debug.Log("Fade Progress: " + colorAdjustments.postExposure.value);
        // If the fade is complete
        if (colorAdjustments.postExposure.value <= -7f && sanityMeter._triggerBlackoutFromSanity)
        {
            isFadingToBlack = false;
            sanityMeter._respawnFromInsanity = true; // Trigger respawn or any other event
            sanityMeter._triggerBlackoutFromSanity = false;
        }
    }
}
