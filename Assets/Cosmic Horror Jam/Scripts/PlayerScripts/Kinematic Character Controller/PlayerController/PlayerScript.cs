using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.InputSystem;

namespace KinematicCharacterController
{
    public class PlayerScript : MonoBehaviour
    {
        [Header("References")]
        public PlayerKCC Character;
        public PlayerInteraction PlayerInteraction;
        public CameraManager CharacterCamera;

        [Header("Input Actions")]
        PlayerInput playerInput;

        InputAction _moveAction;
        InputAction _jumpAction;
        InputAction _run;
        InputAction _interact;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private bool _openMenu;

        private Vector3 defaultPlanerDirection;

        private void Awake()
        {
            
        }

        private void Start()
        {
            // New Input System
            playerInput = GetComponent<PlayerInput>();

            playerInput.SwitchCurrentActionMap("General");

            _moveAction = playerInput.actions.FindAction("Move");
            _jumpAction = playerInput.actions.FindAction("Jump");
            _run = playerInput.actions.FindAction("Run");
            _interact = playerInput.actions.FindAction("Interact");

            // --------------------------------------

            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character._cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());

            _openMenu = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _openMenu = !_openMenu;
            }

            if (!_openMenu)
            {
                HandleCharacterInput();
            }
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                if (Character.CurrentCharacterState == ECharacterState.Default)
                {
                    CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                    CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
                    defaultPlanerDirection = CharacterCamera.PlanarDirection;
                }
            }

            if (!_openMenu)
            {
                HandleCameraInput();
            }
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);

            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = _moveAction.ReadValue<Vector2>().y;
            characterInputs.MoveAxisRight = _moveAction.ReadValue<Vector2>().x;

            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.SpaceBar = _jumpAction.ReadValue<float>() == 1;
            characterInputs.LeftShiftHold = _run.ReadValue<float>() == 1;

            PlayerInteraction._interactInput = _interact.ReadValue<float>() == 1;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}