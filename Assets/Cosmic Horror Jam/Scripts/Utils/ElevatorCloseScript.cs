using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCloseScript : MonoBehaviour
{
    public bool _playerInElevator;

    private void Start()
    {
        _playerInElevator = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInElevator = true;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);

    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        _playerInElevator = true;
    //    }
    //}
}
