using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchStatusScript : MonoBehaviour
{
    public GameObject torchHigh;
    public GameObject torchMedium;
    public GameObject torchLow;

    public float maxValue;
    public float decayPerTick;
    public float tickRate;

    public float MediumThreshold;

    [Header("DEBUG")]
    public float currentValue;
    

    // Start is called before the first frame update
    void Start()
    {
        torchHigh.SetActive(true);
        torchMedium.SetActive(false);
        torchLow.SetActive(false);

        currentValue = maxValue;

        //torchHigh.SetActive(true);
        //torchMedium.SetActive(false);
        //torchLow.SetActive(false);

        //currentValue = 0.0f;

        StartCoroutine("TorchDecay", tickRate);
    }

    IEnumerator TorchDecay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (currentValue > 0f)
            {
                currentValue -= decayPerTick;
            }

            currentValue = Mathf.Clamp(currentValue, 0, maxValue);

            if (currentValue == 0)
            {
                if (!torchLow.activeSelf)
                {
                    torchHigh.SetActive(false);
                    torchMedium.SetActive(false);
                    torchLow.SetActive(true);
                }
            }
            else if (currentValue > 0 && currentValue < MediumThreshold)
            {
                if (!torchMedium.activeSelf)
                {
                    torchHigh.SetActive(false);
                    torchMedium.SetActive(true);
                    torchLow.SetActive(false);
                }
            }
            else if (currentValue > MediumThreshold)
            {
                if (!torchHigh.activeSelf)
                {
                    torchHigh.SetActive(true);
                    torchMedium.SetActive(false);
                    torchLow.SetActive(false);
                }
            }
        }
    }
}
