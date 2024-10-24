using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string returnSceneName = "MainScene"; // Scene to return to after cutscene

    [Header("Cutscene Cameras")]
    public CinemachineVirtualCamera elevatorCamera; // Cinemachine camera in the elevator

    [Header("Stage Durations")]
    public float shakeDuration = 3f;  // How long the camera shakes in the elevator
    public float moveDownDuration = 4f;  // Duration for moving down
    public float zoomOutDuration = 3f;  // Duration for zoom out
    public float pauseDuration = 2f;  // Pause time at the end

    [Header("Movement Settings")]
    public float moveDownDistance = 10f;  // Distance the camera moves down
    public float zoomOutDistance = 5f;  // Distance the camera moves back during zoom out
    public float moveDownSpeed = 2f;  // Speed of moving down
    public float zoomOutSpeed = 2f;  // Speed of zooming out

    [Header("Shaky Camera Settings")]
    public float shakeIntensity = 0.2f;  // Intensity of the camera shake

    [Header("Fade to Black Panel Settigns")]
    public GameObject fadePanel;
    public TextMeshProUGUI fadeText;
    public float fadeDuration = 2f;
    public Button returnButton;

    private Vector3 initialPosition;  // Initial camera position
    private float timer = 0f;  // Time tracker
    private bool isShaking = true;
    private bool isMovingDown = false;
    private bool isZoomingOut = false;
    private bool isPaused = false;
    private bool isFadingIn = false;
    private bool nextscene = false;
    void Start()
    {
        // Store the initial position of the camera
        initialPosition = elevatorCamera.transform.localPosition;

        // Start with the shaky camera effect
        isShaking = true;


        if (fadePanel)
        {
            fadePanel.SetActive(false);
        }

        else
        {
            Debug.LogError("Fade Panel Not Assigned");
        }
    }

    void Update()
    {
        if (isShaking)
        {
            ShakeCamera();
        }
        else if (isMovingDown)
        {
            MoveDown();
        }
        else if (isZoomingOut)
        {
            ZoomOut();
        }
        else if (isPaused)
        {
            PauseAtEnd();
        }

        if (isFadingIn)
        {
            FadeIn();
        }
    }

    private void ShakeCamera()
    {
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / shakeDuration);

        // Apply random shaking to the camera's position
        Vector3 shakeOffset = new Vector3(
            Random.Range(-shakeIntensity, shakeIntensity),
            Random.Range(-shakeIntensity, shakeIntensity),
            Random.Range(-shakeIntensity, shakeIntensity)
        );
        elevatorCamera.transform.localPosition = initialPosition + shakeOffset;

        if (progress >= 1f)
        {
            // Stop shaking and start moving down
            timer = 0f;
            elevatorCamera.transform.localPosition = initialPosition;  // Reset position
            isShaking = false;
            isMovingDown = true;
        }
    }

    private void MoveDown()
    {
        timer += Time.deltaTime * moveDownSpeed; // Scale timer with speed
        float progress = Mathf.Clamp01(timer / moveDownDuration);

        // Smoothly move the camera down
        Vector3 targetPosition = initialPosition + Vector3.down * moveDownDistance;
        elevatorCamera.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, progress);

        if (progress >= 1f)
        {
            // Stop moving down and start zooming out
            timer = 0f;
            isMovingDown = false;
            isZoomingOut = true;
        }
    }

    private void ZoomOut()
    {
        timer += Time.deltaTime * zoomOutSpeed; // Scale timer with speed
        float progress = Mathf.Clamp01(timer / zoomOutDuration);

        // Smoothly pull back the camera by adjusting its position
        Vector3 targetPosition = elevatorCamera.transform.localPosition - Vector3.back * zoomOutDistance;
        elevatorCamera.transform.localPosition = Vector3.Lerp(initialPosition + Vector3.down * moveDownDistance, targetPosition, progress);

        if (progress >= 1f)
        {
            // Stop zooming out and start the pause
            timer = 0f;
            isZoomingOut = false;
            isPaused = true;
        }
    }

    private void PauseAtEnd()
    {
        timer += Time.deltaTime;

        if (timer >= pauseDuration)
        {
            StartFadeIn();
            timer = 0f;
            timer += Time.deltaTime;
            if(timer >= pauseDuration)
            {
                Debug.Log("Switch Scenes");
            }
        }
    }

    private void StartFadeIn()
    {
        fadePanel.SetActive(true);
        isFadingIn = true;
    }

    private void FadeIn()
    {
        //Debug.Log("FadeIn plays");
        Image panelImage = fadePanel.GetComponent<Image>();
        if (panelImage)
        {
            Color color = panelImage.color;
            color.a += Time.deltaTime / fadeDuration;
            //Debug.Log("Panel Alpha: " + color.a);
            panelImage.color = color;

            if (color.a >= 1f)
            {
                //Debug.Log("Full Stop");
                color.a = 1f;
                panelImage.color = color;
                isFadingIn = false;

                returnButton.gameObject.SetActive(true);
                fadeText.text = "Thank You For Playing";
                 
            }
        }
        if (panelImage == null)
        {
            Debug.Log("Not Referenced");
        }
    }

    public void loadscene()
    {
        SceneManager.LoadScene(0);
    }
}
