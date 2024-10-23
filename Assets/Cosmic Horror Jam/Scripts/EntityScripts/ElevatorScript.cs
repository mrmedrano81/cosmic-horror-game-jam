using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public float y_travelDistance = 5.25f;
    public float speed;

    public GameObject door;
    public float y_doorTravelDistance = -4.86f;
    public float doorSpeed;

    public float elevatorCloseDelay;

    private Vector3 topDestination;
    private Vector3 doorOpenDestination;

    private Vector3 originalPosition;
    private Vector3 originalDoorPosition;

    private Vector3 doorOffset;

    [HideInInspector] public bool _startClosingSequence;

    private ElevatorCloseScript _closeScript;
    private bool _doorClosed;

    private float _currentTime;
    private bool _isWaitingToClose;


    private void Awake()
    {
        _closeScript = GetComponentInChildren<ElevatorCloseScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _doorClosed = false;
        _startClosingSequence = false;
        topDestination = transform.position + new Vector3(0, y_travelDistance, 0);
        doorOffset = new Vector3(0, y_doorTravelDistance, 0);

        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_closeScript._playerInElevator && !_startClosingSequence)
        {
            _startClosingSequence = true;
        }

        if (!_startClosingSequence)
        {
            AscendingElevator();
        }
        else
        {
            ClosingElevator();
        }
    }

    public void AscendingElevator()
    {
        if (Vector3.Distance(topDestination, transform.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, topDestination, Time.deltaTime * speed);
            originalDoorPosition = door.transform.position;
        }
        else if (Vector3.Distance(door.transform.position, originalDoorPosition + doorOffset) > 0.1f)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, originalDoorPosition + doorOffset, Time.deltaTime * doorSpeed);
        }
    }

    public void ClosingElevator()
    {
        if (Vector3.Distance(door.transform.position, originalDoorPosition) > 0.1f && !_doorClosed)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, originalDoorPosition, Time.deltaTime * doorSpeed);
        }
        else
        {
            _doorClosed = true;
        }
    }

    public void DescendingElevator()
    {
        if (Vector3.Distance(originalPosition, transform.position) > 0.1f && _doorClosed)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * speed);
        }
    }


}
