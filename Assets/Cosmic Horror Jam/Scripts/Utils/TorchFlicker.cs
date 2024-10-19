using UnityEngine;

public class MultiLayerTorchFlicker : MonoBehaviour
{
    public Light innerLight; // Closest layer of light
    public Light secondLayerLight; // Middle layer of light
    public Light thirdLayerLight; // Outer layer of light

    public float innerMinIntensity = 1.0f;
    public float innerMaxIntensity = 1.4f;
    public float secondMinIntensity = 0.7f;
    public float secondMaxIntensity = 1.0f;
    public float thirdMinIntensity = 0.4f;
    public float thirdMaxIntensity = 0.8f;

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
        float innerNoise = Mathf.PerlinNoise(randomizer1, Time.time * flickerSpeed);
        innerLight.intensity = Mathf.Lerp(innerMinIntensity, innerMaxIntensity, innerNoise);
        innerLight.range = Mathf.Lerp(3f, 5f, innerNoise); // Adjust range as needed

        float secondNoise = Mathf.PerlinNoise(randomizer2, Time.time * (flickerSpeed * 0.9f));
        secondLayerLight.intensity = Mathf.Lerp(secondMinIntensity, secondMaxIntensity, secondNoise);
        secondLayerLight.range = Mathf.Lerp(5f, 7f, secondNoise); // Adjust range for second layer

        float thirdNoise = Mathf.PerlinNoise(randomizer3, Time.time * (flickerSpeed * 0.8f));
        thirdLayerLight.intensity = Mathf.Lerp(thirdMinIntensity, thirdMaxIntensity, thirdNoise);
        thirdLayerLight.range = Mathf.Lerp(7f, 10f, thirdNoise); // Adjust range for third layer
    }

}
