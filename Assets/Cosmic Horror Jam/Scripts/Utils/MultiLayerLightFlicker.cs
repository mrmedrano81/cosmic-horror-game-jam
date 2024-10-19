using UnityEngine;

public class MultiLayerLightFlicker : MonoBehaviour
{
    public Light innerLight; // Closest layer of light
    public Light secondLayerLight; // Middle layer of light
    public Light thirdLayerLight; // Outer layer of light

    // Intensity settings
    public float innerMinIntensity = 1.0f;
    public float innerMaxIntensity = 1.4f;
    public float secondMinIntensity = 0.7f;
    public float secondMaxIntensity = 1.0f;
    public float thirdMinIntensity = 0.4f;
    public float thirdMaxIntensity = 0.8f;

    // Range settings
    public float innerMinRange = 3.0f;
    public float innerMaxRange = 5.0f;
    public float secondMinRange = 5.0f;
    public float secondMaxRange = 7.0f;
    public float thirdMinRange = 7.0f;
    public float thirdMaxRange = 10.0f;

    public float flickerSpeed = 0.1f; // Speed of flicker
    private float randomizer1;
    private float randomizer2;
    private float randomizer3;

    void Start()
    {
        if (innerLight == null || secondLayerLight == null || thirdLayerLight == null)
        {
            Debug.LogError("All three lights must be assigned.");
        }

        // Assign random starting points for each light layer to vary flicker patterns
        randomizer1 = Random.Range(0f, 65535f);
        randomizer2 = Random.Range(0f, 65535f);
        randomizer3 = Random.Range(0f, 65535f);
    }

    void Update()
    {
        FlickerLightLayers();
    }

    void FlickerLightLayers()
    {
        // Inner light flicker
        float innerNoise = Mathf.PerlinNoise(randomizer1, Time.time * flickerSpeed);
        innerLight.intensity = Mathf.Lerp(innerMinIntensity, innerMaxIntensity, innerNoise);
        innerLight.range = Mathf.Lerp(innerMinRange, innerMaxRange, innerNoise); // Variable range

        // Second layer light flicker
        float secondNoise = Mathf.PerlinNoise(randomizer2, Time.time * (flickerSpeed * 0.9f));
        secondLayerLight.intensity = Mathf.Lerp(secondMinIntensity, secondMaxIntensity, secondNoise);
        secondLayerLight.range = Mathf.Lerp(secondMinRange, secondMaxRange, secondNoise); // Variable range

        // Third layer light flicker
        float thirdNoise = Mathf.PerlinNoise(randomizer3, Time.time * (flickerSpeed * 0.8f));
        thirdLayerLight.intensity = Mathf.Lerp(thirdMinIntensity, thirdMaxIntensity, thirdNoise);
        thirdLayerLight.range = Mathf.Lerp(thirdMinRange, thirdMaxRange, thirdNoise); // Variable range
    }
}
