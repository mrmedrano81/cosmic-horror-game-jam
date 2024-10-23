using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityPostProcessing : MonoBehaviour
{
    public Volume postProcessingVolume;    // The volume with post-processing effects
    [Range(0f, 1f)] public float sanity = 1f;  // Sanity parameter (0 = worst sanity, 1 = full sanity)

    // Lens Distortion settings
    public float lensDistortionMin = 0.5f;
    public float lensDistortionMax = -0.1f;

    public float pulseStrengthMin = 0.05f;  // Minimum pulse strength at full sanity
    public float pulseStrengthMax = 0.3f;   // Maximum pulse strength at low sanity
    public float pulseSpeedMin = 0.5f;      // Minimum pulse speed at full sanity
    public float pulseSpeedMax = 5f;        // Maximum pulse speed at low sanity

    // Vignette settings
    public float vignetteIntensityMin = 0.5f;
    public float vignetteIntensityMax = 0.1f;
    public float vignetteSmoothnessMin = 1f;
    public float vignetteSmoothnessMax = 0.3f;

    // Chromatic Aberration settings
    public float chromaticAberrationMin = 1f;
    public float chromaticAberrationMax = 0f;

    public float saturationMin = -70f;
    public float saturationMax = 0f;

    // Color Adjustments settings (Color Filter)
    public Color colorFilterMin = Color.red;  // Color filter at worst sanity
    public Color colorFilterMax = Color.white; // Color filter at full sanity

    private LensDistortion lensDistortion;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    private SanityMeter sanityMeter;

    void Start()
    {
        sanityMeter = FindObjectOfType<SanityMeter>();

        // Ensure the Volume has the necessary overrides
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
        sanity = sanityMeter._currentSanity / sanityMeter._maxSanity;
        // Clamp sanity between 0 and 1
        sanity = Mathf.Clamp01(sanity);

        // Calculate pulse strength and speed based on sanity (lower sanity = stronger and faster pulse)
        float pulseStrength = Mathf.Lerp(pulseStrengthMax, pulseStrengthMin, sanity);
        float pulseSpeed = Mathf.Lerp(pulseSpeedMax, pulseSpeedMin, sanity);

        // Modify Lens Distortion based on sanity with pulse
        if (lensDistortion != null)
        {
            // Basic distortion based on sanity
            float distortion = Mathf.Lerp(lensDistortionMin, lensDistortionMax, sanity);

            // Add pulse effect on top of the basic distortion
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseStrength;
            lensDistortion.intensity.value = distortion + pulse;
        }

        // Modify Vignette based on sanity
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(vignetteIntensityMin, vignetteIntensityMax, sanity);
            vignette.smoothness.value = Mathf.Lerp(vignetteSmoothnessMin, vignetteSmoothnessMax, sanity);
        }

        // Modify Chromatic Aberration based on sanity
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberrationMin, chromaticAberrationMax, sanity);
        }

        // Modify Color Adjustments (Color Filter) based on sanity
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(saturationMin, saturationMax, sanity);
        }
    }
}
