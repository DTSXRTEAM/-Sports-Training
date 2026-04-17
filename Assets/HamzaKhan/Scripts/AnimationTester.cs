using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace CricketBowlingAnimations
{
    public class AnimationTester : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Animator animator;

        [SerializeField] private string animatorBowlingTriggerName = "Bowl";
        [SerializeField] private string animatorBowlingTypeName = "BowlingType";

        [SerializeField] private BowlingAnimation bowlingAnimation;

        [SerializeField] private bool useRootMotion = true;

        [Header("XR Input")]
        public XRNode controllerNode = XRNode.RightHand; // change to LeftHand if needed
        public float triggerThreshold = 0.8f;

        private InputDevice device;
        private bool isPressed = false;

        public enum BowlingAnimation
        {
            LeftArmFastBowler,
            LeftArmMediumFastBowler,
            LeftArmOrthodoxSpinner,
            LeftArmWristSpinner,
            RightArmFastBowler,
            RightArmMediumFastBowler,
            RightArmLegSpinner,
            RightArmOffSpinner,
        }

        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.position;
        }

        void Start()
        {
            device = InputDevices.GetDeviceAtXRNode(controllerNode);
        }

        void Update()
        {
            // Root motion toggle
            animator.applyRootMotion = useRootMotion;

            // Set animation type
            animator.SetFloat(animatorBowlingTypeName, (int)bowlingAnimation + 1);

            // Reconnect device if needed
            if (!device.isValid)
                device = InputDevices.GetDeviceAtXRNode(controllerNode);

            float triggerValue;

            if (device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
            {
                // PRESS
                if (triggerValue > triggerThreshold && !isPressed)
                {
                    isPressed = true;

                    PlayAnimation();
                }

                // RELEASE
                if (triggerValue < 0.2f)
                {
                    isPressed = false;
                }
            }
        }

        void PlayAnimation()
        {
            // Prevent replay while already playing
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Bowling"))
            {
                transform.position = originalPosition;
                animator.SetTrigger(animatorBowlingTriggerName);
            }
        }
    }
}