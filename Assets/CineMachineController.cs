using UnityEngine;
using Cinemachine;

public class CutsceneController : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera virtualCamera; // Assign in the Inspector

    [Header("Move Down Settings")]
    public float moveDownSpeed = 5f; // Speed of downward movement
    public float moveDownDuration = 3f; // Duration of the downward movement

    [Header("Zoom Out Settings")]
    public float zoomOutSpeed = 10f; // Speed of zoom out
    public float zoomOutDuration = 2f; // Duration of zoom-out effect

    [Header("Pause Settings")]
    public float pauseDuration = 2f; // How long the camera stays still after the zoom out

    private float moveTimer = 0f;
    private float zoomTimer = 0f;
    private float pauseTimer = 0f;

    private bool isMovingDown = true;
    private bool isZoomingOut = false;
    private bool isPausing = false;

    private Vector3 initialPosition;
    private float initialZoom;

    void Start()
    {
        // Store the initial position and field of view (FOV)
        initialPosition = virtualCamera.transform.position;
        initialZoom = virtualCamera.m_Lens.FieldOfView;
    }

    void Update()
    {
        if (isMovingDown)
        {
            MoveDown();
        }
        else if (isZoomingOut)
        {
            ZoomOut();
        }
        else if (isPausing)
        {
            PauseInPlace();
        }
    }

    private void MoveDown()
    {
        if (moveTimer < moveDownDuration)
        {
            float progress = moveTimer / moveDownDuration;
            Vector3 targetPosition = initialPosition + Vector3.down * moveDownSpeed;
            virtualCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
            moveTimer += Time.deltaTime;
        }
        else
        {
            // Transition to zoom out phase
            isMovingDown = false;
            isZoomingOut = true;
            moveTimer = 0f; // Reset timer for next phase
        }
    }

    private void ZoomOut()
    {
        if (zoomTimer < zoomOutDuration)
        {
            float progress = zoomTimer / zoomOutDuration;
            float targetZoom = initialZoom + zoomOutSpeed;
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(initialZoom, targetZoom, progress);
            zoomTimer += Time.deltaTime;
        }
        else
        {
            // Transition to pause phase
            isZoomingOut = false;
            isPausing = true;
            zoomTimer = 0f; // Reset timer for the next phase
        }
    }

    private void PauseInPlace()
    {
        if (pauseTimer < pauseDuration)
        {
            pauseTimer += Time.deltaTime;
        }
        else
        {
            // End of cutscene - Optionally trigger other events here
            Debug.Log("Cutscene Complete");
            enabled = false; // Disable the script to end the cutscene
        }
    }
}
