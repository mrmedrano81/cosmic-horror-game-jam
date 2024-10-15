using KinematicCharacterController;
using UnityEngine;

public class PlayerVaultingState : PlayerBaseState
{

    // Detection Phase
    public bool _isMediumVaulting;
    public bool _isLowVaulting;

    public bool _obstacleIsClose;

    public bool _motionCompleted;

    public float _landingPositionDetectDistance;
    public float _landingPositionDetectDownwardDistance;
    public Vector3 _landingPositionRaycastOrigin;

    public Vector3 _landingPosition;
    public float _landingPositionHeightFromPlayerPos;

    // Mantling Phase
    public Vector3 _mantleStartPosition;
    public Vector3 _mantleEndPosition;

    public Vector3 _currentVelocity;

    public float _sampleTime;
    public float _obstacleSnapOffset = 1f;

    public bool _isMantling;

    public PlayerVaultingState(PlayerKCC currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void SetInput(ref PlayerCharacterInputs inputs)
    {
    }

    public override void EnterState()
    {
        _ctx._animator.CrossFadeInFixedTime(_ctx.JUMPING, 0.1f);

        //_ctx.Motor.SetCapsuleDimensions(_ctx._originalCapsuleRadius, _ctx._originalCapsuleHeight/2f, 0f);
        _ctx.Motor.SetCapsuleCollisionsActivation(false);

        _currentVelocity = _ctx.Motor.Velocity;

        _motionCompleted = false;
        _isMediumVaulting = false;
        _isLowVaulting = false;

        if (_ctx.LowObstacleInFront())
        {
            // lowVault
            Debug.Log("[OBSTACLE SYSTEM] Low Vault");

            Vector3 lowObstacleHitPoint = Vector3.zero;

            if (Physics.Raycast(_ctx._lowObstacleRaycastOrigin, _ctx.Motor.CharacterForward, out RaycastHit lowObstacleFrontHit, _ctx._obstacleCheckLength, _ctx._wallLayers))
            {
                lowObstacleHitPoint = lowObstacleFrontHit.point;
            }

            _landingPositionDetectDistance = Vector3.Distance(lowObstacleHitPoint, _ctx._lowObstacleRaycastOrigin) + _ctx.Motor.Capsule.radius;

            _isMediumVaulting = false;
            _isLowVaulting = true;

            _landingPositionRaycastOrigin = _ctx._midObstacleRaycastOrigin + _ctx.Motor.CharacterForward * _landingPositionDetectDistance;
            _landingPositionDetectDownwardDistance = _ctx._obstacleMidCheckHeight - _ctx._obstacleLowCheckHeight;

            if (Physics.Raycast(_landingPositionRaycastOrigin, -Vector3.up, out RaycastHit lowLandingHit, _landingPositionDetectDownwardDistance, _ctx._wallLayers))
            {
                _isMantling = true;

                _landingPosition = lowLandingHit.point;

                Debug.DrawRay(_landingPositionRaycastOrigin, -Vector3.up, Color.white, 3f);

                _landingPositionHeightFromPlayerPos = _ctx._obstacleMidCheckHeight - Vector3.Distance(_landingPositionRaycastOrigin, lowLandingHit.point);

                //_ctx.Motor.SetPosition(_landingPosition + Vector3.up*_ctx.Motor.Capsule.height/2f);
                //Debug.Break();

                Debug.Log(Vector3.Distance(_ctx.Motor.TransientPosition, lowObstacleFrontHit.point));

                if (Vector3.Distance(_ctx.Motor.TransientPosition, lowObstacleFrontHit.point) > (_ctx._closeObstacleCheckLength + _ctx._originalCapsuleRadius / 3f))
                {
                    _obstacleIsClose = false;
                    SetCurve(1f, 2f);

                    Debug.Log("far");
                    //Debug.Break();
                }
                else
                {
                    _obstacleIsClose = true;

                    _ctx.Motor.SetPosition(lowObstacleFrontHit.point + Vector3.up*(_ctx.Motor.TransientPosition.y - lowObstacleFrontHit.point.y)*1.1f + lowObstacleFrontHit.normal*(_obstacleSnapOffset + _ctx._originalCapsuleRadius/3f));

                    SetCurve(1f, 2f);

                    Debug.Log("close");
                }
            }


            else
            {
                _isMantling = false;

                //Debug.Log("low vault");

                //_landingPosition = _ctx._lowObstacleRaycastOrigin + _ctx.Motor.CharacterForward * _ctx._obstacleCheckLength;
                //SetCurve(0f, 2f);
            }
        }

        else if (_ctx.MidObstacleInFront())
        {
            // mediumVault
            Debug.Log("[OBSTACLE SYSTEM] Medium Vault");

            Vector3 midObstacleHitPoint = Vector3.zero;

            if (Physics.Raycast(_ctx._lowObstacleRaycastOrigin, _ctx.Motor.CharacterForward, out RaycastHit midObstacleHit, _ctx._obstacleCheckLength, _ctx._wallLayers))
            {
                midObstacleHitPoint = midObstacleHit.point;
            }

            _landingPositionDetectDistance = Vector3.Distance(midObstacleHitPoint, _ctx._midObstacleRaycastOrigin) + _ctx.Motor.Capsule.radius;

            _isMediumVaulting = true;
            _isLowVaulting = false;

            _landingPositionRaycastOrigin = _ctx._highObstacleRaycastOrigin + _ctx.Motor.CharacterForward * _landingPositionDetectDistance;
            _landingPositionDetectDownwardDistance = _ctx._obstacleHighCheckHeight;


            if (Physics.Raycast(_landingPositionRaycastOrigin, -_ctx.Motor.CharacterUp, out RaycastHit midLandingPositionHit, _landingPositionDetectDownwardDistance, _ctx._wallLayers))
            {
                _isMantling = true;

                _landingPosition = midLandingPositionHit.point;
                Debug.DrawRay(_landingPositionRaycastOrigin, -Vector3.up, Color.white, 3f);

                _landingPositionHeightFromPlayerPos = _ctx._obstacleHighCheckHeight - Vector3.Distance(_landingPositionRaycastOrigin, midLandingPositionHit.point);

                //_ctx.Motor.SetPosition(_landingPosition + Vector3.up * _ctx.Motor.Capsule.height / 2f);
                //Debug.Break();
                Debug.Log(Vector3.Distance(_ctx.Motor.TransientPosition, midObstacleHit.point));

                if (Vector3.Distance(_ctx.Motor.TransientPosition, midObstacleHit.point) > (_ctx._closeObstacleCheckLength + _ctx._originalCapsuleRadius / 3f))
                {
                    _obstacleIsClose = false;
                    SetCurve(1f, 2f);

                    Debug.Log("far");
                    //Debug.Break();
                }
                else
                {
                    _obstacleIsClose = true;

                    _ctx.Motor.SetPosition(midObstacleHit.point - Vector3.up * (midObstacleHit.point.y - _ctx.Motor.TransientPosition.y*1.1f) 
                        + midObstacleHit.normal * (_obstacleSnapOffset + _ctx._originalCapsuleRadius / 3f));

                    //Debug.Break();
                    SetCurve(2f, 3f);

                    Debug.Log("close");
                }
            }
            else
            {
                _isMantling = false;
                //Debug.Log("medium vault");

                //_landingPosition = _ctx._highObstacleRaycastOrigin + _ctx.Motor.CharacterForward * 2f;
                //SetCurve(0f, 3f);
            }
        }

        else
        {
            Debug.Log("[OBSTACLE SYSTEM] Error, no vault requirements met in vaulting state");
            Debug.Break();
        }


        void SetCurve(float heightOffset, float backwardOffset)
        {
            _mantleStartPosition = _ctx.Motor.TransientPosition;
            _mantleEndPosition = _landingPosition + Vector3.up * _ctx._originalCapsuleHeight / 2.1f;

            _ctx._curve.A.position = _mantleStartPosition;
            _ctx._curve.B.position = _mantleEndPosition;
            _ctx._curve.Control.position = _mantleEndPosition + Vector3.up * heightOffset - _ctx.Motor.CharacterForward * backwardOffset;

            _sampleTime = 0;
        }
    }

    public override void ExitState()
    {
        //_ctx.Motor.SetCapsuleDimensions(_ctx._originalCapsuleRadius, _ctx._originalCapsuleHeight, 0f);
        _ctx.Motor.SetCapsuleCollisionsActivation(true);
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (_motionCompleted)
        {
            if (_ctx._moveInputVector.magnitude != 0 && _isMantling)
            {
                Vector3 inputRight = Vector3.Cross(_ctx._moveInputVector, _ctx.Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(_ctx.Motor.GroundingStatus.GroundNormal, inputRight).normalized * _ctx._moveInputVector.magnitude;

                if (_ctx._isMaintainingMomentum)
                {
                    currentVelocity = reorientedInput * _ctx._runningSpeed;
                }
                else
                {
                    currentVelocity = reorientedInput * _ctx._walkingSpeed;
                }
                
            }

            currentVelocity += _ctx._gravity * deltaTime * 10f;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }

    public override void CheckSwitchState()
    {
        if (_ctx._jumpRequested || _ctx._movementAbilityRequested)
        {
            _motionCompleted = true;
            SwitchState(_factory.Airborne());
        }
        else if (_motionCompleted)
        {
            if (_ctx.IsGrounded)
            {
                _ctx._finalFallVelocityY = -5.1f;
                SwitchState(_factory.Grounded());
            }
            else if (_ctx.Motor.Velocity.y < -5.2f)
            {
                SwitchState(_factory.Airborne());
            }
        }
    }

    public override void InitializeSubStates()
    {
    }

    public override void UpdateState()
    {
        if (!_motionCompleted)
        {
            if (_isMantling)
            {
                _sampleTime += Time.deltaTime * _ctx._mantleSpeed / (1f + _sampleTime * 1.1f);
            }
            else
            {
                _sampleTime += Time.deltaTime * 2f * _ctx._mantleSpeed / (1f + _sampleTime * 2f);
            }

            _ctx.Motor.SetPosition(_ctx._curve.evaluate(_sampleTime));

            if (_sampleTime >= 1)
            {
                //_ctx.Motor.SetCapsuleDimensions(_ctx._originalCapsuleRadius, _ctx._originalCapsuleHeight, 0f);
                _ctx.Motor.SetPosition(_mantleEndPosition);
                _motionCompleted = true;

                if (!_isMantling)
                {
                    _ctx.AddVelocity(_ctx.Motor.CharacterForward * _ctx._runningSpeed);
                }
            }
        }
    }



    #region Mantling Motion

    #endregion
}
