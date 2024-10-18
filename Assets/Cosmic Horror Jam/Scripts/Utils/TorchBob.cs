using UnityEngine;

public class TorchBob : MonoBehaviour
{
    public float bobSpeed = 5f;    // Speed of the bobbing motion
    public float bobAmountY = 0.05f; // Vertical bobbing amount
    public float bobAmountX = 0.03f; // Horizontal bobbing amount
    public float noiseAmount = 0.02f; // Amount of randomness in bobbing

    private float defaultYPos;
    private float defaultXPos;
    private float timer = 0;

    void Start()
    {
        // Store the default position of the torch
        defaultYPos = transform.localPosition.y;
        defaultXPos = transform.localPosition.x;
    }

    void Update()
    {
        // Check if the player is moving
        if (IsPlayerWalking())
        {
            // Increment timer based on bobSpeed and deltaTime
            timer += Time.deltaTime * bobSpeed;

            // Calculate the new Y and X positions based on sine/cosine waves
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmountY;
            float newX = defaultXPos + Mathf.Cos(timer) * bobAmountX;

            // Add Perlin noise for a more natural, noisy effect
            float noiseY = Mathf.PerlinNoise(timer, 0) * noiseAmount;
            float noiseX = Mathf.PerlinNoise(0, timer) * noiseAmount;

            // Apply the new noisy position to the torch
            transform.localPosition = new Vector3(newX + noiseX, newY + noiseY, transform.localPosition.z);
        }
        else
        {
            // Reset the timer when the player stops moving to prevent jitter
            timer = 0;
        }
    }

    bool IsPlayerWalking()
    {
        // Check if the player is moving (you can change this to your movement logic)
        //return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        return true;
    }
}
