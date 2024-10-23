using UnityEngine;

public class KeyItemBob : MonoBehaviour
{
    public float bobbingSpeed = 2f;        // Speed of the bobbing motion
    public float bobbingHeight = 0.25f;    // Maximum height for bobbing
    public float rotationSpeed = 50f;      // Speed of the left-right rotation
    public float maxRotationAngle = 15f;   // Maximum angle for left-right rotation

    private Vector3 startPosition;
    private float currentRotationAngle = 0f;
    private bool rotatingRight = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Bobbing up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate left and right (limited to maxRotationAngle)
        float rotationChange = rotationSpeed * Time.deltaTime;

        if (rotatingRight)
        {
            currentRotationAngle += rotationChange;
            if (currentRotationAngle >= maxRotationAngle)
            {
                rotatingRight = false;
            }
        }
        else
        {
            currentRotationAngle -= rotationChange;
            if (currentRotationAngle <= -maxRotationAngle)
            {
                rotatingRight = true;
            }
        }

        // Apply the left-right rotation around the Y-axis
        transform.localRotation = Quaternion.Euler(0, currentRotationAngle, 0);
    }
}
