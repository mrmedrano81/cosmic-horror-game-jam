using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        [Header("[REFERENCES]")]
        public Animator animator;

        [Header("Locomotion Animation Hashes")]
        public readonly int JUMPING = Animator.StringToHash("Jumping");
        public readonly int DASHING = Animator.StringToHash("Dashing");
        public readonly int SOFT_LANDING = Animator.StringToHash("Soft Landing");
        public readonly int STUMBLE = Animator.StringToHash("Stumble");
        public readonly int ROLLING = Animator.StringToHash("Rolling");
        public readonly int HARD_LANDING = Animator.StringToHash("Hard Landing");
        public readonly int RUNNING_JUMP = Animator.StringToHash("Running Jump");

        [Header("Combat Offense Animation Hashes")]
        readonly int SPEAR_THROW = Animator.StringToHash("Spear Throw");

        [Header("Containers")]
        private Dictionary<EPlayerLocomotion, int> AnimHashDict = new Dictionary<EPlayerLocomotion, int>();
        public EPlayerLocomotion currentCharacterAction;
        public int currentCharacterAnimState;

        [Header("Animation Variables")]
        public float MoveAxisX;
        public float MoveAxisY;

        [Header("Animation Timer Settings")]
        public bool lockAnimation;
        private float currentAnimationDuration;
        private float startAnimationTime;

        // Start is called before the first frame update
        void Awake()
        {
            SetupAnimationDict();
        }

        private void Start()
        {

        }

        private void Update()
        {
            animator.SetFloat("MoveX", MoveAxisX);
            animator.SetFloat("MoveY", MoveAxisY);
        }

        #region --- Dictionary Setup ---
        private void SetupAnimationDict()
        {
            // Basic Locomotion
            AnimHashDict.Add(EPlayerLocomotion.Jumping, JUMPING);
            AnimHashDict.Add(EPlayerLocomotion.SoftLanding, SOFT_LANDING);
            AnimHashDict.Add(EPlayerLocomotion.Rolling, ROLLING);
            AnimHashDict.Add(EPlayerLocomotion.HardLanding, HARD_LANDING);
            AnimHashDict.Add(EPlayerLocomotion.RunningJump, RUNNING_JUMP);
        }
        #endregion


        public void PlayAnimation(EPlayerLocomotion newCharacterAction, bool useCrossFade = true, float crossfadeDuration = 0.15f)
        {
            if (currentCharacterAnimState != AnimHashDict[newCharacterAction])
            {
                currentCharacterAnimState = AnimHashDict[newCharacterAction];

                if (useCrossFade)
                {
                    animator.CrossFadeInFixedTime(AnimHashDict[newCharacterAction], crossfadeDuration);
                }

                else
                {
                    animator.Play(AnimHashDict[newCharacterAction]);
                }
            }
        }

        public void PlayAnimation(int newCharacterAnimState, bool useCrossFade = true, float crossfadeDuration = 0.2f)
        {
            if (currentCharacterAnimState != newCharacterAnimState)
            {
                currentCharacterAnimState = newCharacterAnimState;

                if (useCrossFade)
                {
                    animator.CrossFadeInFixedTime(currentCharacterAnimState, crossfadeDuration);
                }

                else
                {
                    animator.Play(currentCharacterAnimState);
                }
            }
        }

        public void RunningEvent(int arg)
        {
            //Debug.Log("Running: " + arg);
        }
    }
}

