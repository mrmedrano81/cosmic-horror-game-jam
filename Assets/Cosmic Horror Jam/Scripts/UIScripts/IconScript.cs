using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconScript : MonoBehaviour
{
    public GameObject keyIcon_1;
    public GameObject keyIcon_2;
    public GameObject keyIcon_3;

    private void Awake()
    {
        keyIcon_1.SetActive(false);
        keyIcon_2.SetActive(false);
        keyIcon_3.SetActive(false);
    }
}
