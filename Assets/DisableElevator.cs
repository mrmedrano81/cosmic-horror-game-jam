using System.Collections;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [Header("Disable Settings")]
    public float disableDuration = 5f; // Time in seconds before disabling the GameObject

    private void Start()
    {
        // Start the coroutine to disable the GameObject after the specified duration
        StartCoroutine(DisableCoroutine());
    }

    private IEnumerator DisableCoroutine()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(disableDuration);

        // Disable the GameObject
        gameObject.SetActive(false);
    }
}
