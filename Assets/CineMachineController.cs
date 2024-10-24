using UnityEngine;
using Cinemachine;

public class CutsceneController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera; // Assign the default Unity Main Camera
    public CinemachineVirtualCamera cutsceneCamera; // Assign your cutscene Cinemachine camera

    [Header("Move Down Settings")]
    public float moveDownSpeed = 5f;
    public float moveDownDuration = 3f;

    [Header("Zoom Out Settings")]
    public float zoomOutSpeed = 10f;
    public float zoomOutDuration = 2f;

    [Header("Pause Settings")]
    public float pauseDuration = 2f;

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
        initialPosition = cutsceneCamera.transform.position;
        initialZoom = cutsceneCamera.m_Lens.FieldOfView;

        // Ensure the main camera starts active and cutscene camera is inactive
        mainCamera.gameObject.SetActive(true);
        cutsceneCamera.gameObject.SetActive(false);
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
            cutsceneCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
            moveTimer += Time.deltaTime;
        }
        else
        {
            isMovingDown = false;
            isZoomingOut = true;
        }
    }

    private void ZoomOut()
    {
        if (zoomTimer < zoomOutDuration)
        {
            float progress = zoomTimer / zoomOutDuration;
            float targetZoom = initialZoom + zoomOutSpeed;
            cutsceneCamera.m_Lens.FieldOfView = Mathf.Lerp(initialZoom, targetZoom, progress);
            zoomTimer += Time.deltaTime;
        }
        else
        {
            isZoomingOut = false;
            isPausing = true;
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
            EndCutscene();
        }
    }

    public void StartCutscene()
    {
        // Switch to the cutscene camera
        mainCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
    }

    private void EndCutscene()
    {
        // Switch back to the main camera after the cutscene
        cutsceneCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Optionally, disable this script after the cutscene ends
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Hit Player");
        if (other.CompareTag("Player"))
        {
            StartCutscene(); // Start the cutscene when the player enters the trigger
        }
    }
}
