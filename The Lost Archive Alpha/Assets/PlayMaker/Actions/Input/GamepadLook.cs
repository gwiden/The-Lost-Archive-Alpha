// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved. <- Lie

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    /// <summary>
    /// Action version of Unity's builtin GamepadLook behaviour.
    /// TODO: Expose invert Y option.
    /// </summary>
    [ActionCategory(ActionCategory.Input)]
    [Tooltip("Rotates a GameObject based on Right Stick movement. Minimum and Maximum values can be used to constrain the rotation.")]
    public class GamepadLook : FsmStateAction
    {
        public enum RotationAxes { RightStickXAndY = 0, RightStickX = 1, RightStickY = 2 }

        [RequiredField]
        [Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;

        [Tooltip("The axes to rotate around.")]
        public RotationAxes axes = RotationAxes.RightStickXAndY;

        [RequiredField]
        [Tooltip("Sensitivity of movement in X direction.")]
        public FsmFloat sensitivityX;

        [RequiredField]
        [Tooltip("Sensitivity of movement in Y direction.")]
        public FsmFloat sensitivityY;

        [HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
        public FsmFloat minimumX;

        [HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
        public FsmFloat maximumX;

        [HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat minimumY;

        [HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat maximumY;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        float rotationX;
        float rotationY;

        public override void Reset()
        {
            gameObject = null;
            axes = RotationAxes.RightStickXAndY;
            sensitivityX = 15f;
            sensitivityY = 15f;
            minimumX = new FsmFloat { UseVariable = true };
            maximumX = new FsmFloat { UseVariable = true };
            minimumY = -60f;
            maximumY = 60f;
            everyFrame = true;
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                Finish();
                return;
            }

            // Make the rigid body not change rotation
            // TODO: Original Unity script had this. Expose as option?
            var rigidbody = go.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.freezeRotation = true;
            }

            // initialize rotation

            rotationX = go.transform.localRotation.eulerAngles.y;
            rotationY = go.transform.localRotation.eulerAngles.x;

            DoGamepadLook();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoGamepadLook();
        }

        void DoGamepadLook()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            var transform = go.transform;

            switch (axes)
            {
                case RotationAxes.RightStickXAndY:

                    transform.localEulerAngles = new Vector3(GetYRotationGP(), GetXRotationGP(), 0);
                    break;

                case RotationAxes.RightStickX:

                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, GetXRotationGP(), 0);
                    break;

                case RotationAxes.RightStickY:

                    transform.localEulerAngles = new Vector3(-GetYRotationGP(), transform.localEulerAngles.y, 0);
                    break;

            }
        }


        float GetXRotationGP()
        {
            rotationX += Input.GetAxis("Right Stick Horizontal") * sensitivityX.Value;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            return rotationX;
        }

        float GetYRotationGP()
        {
            rotationY += Input.GetAxis("Right Stick Vertical") * sensitivityY.Value;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            return rotationY;
        }

        // Clamp function that respects IsNone
        static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
        {
            if (!min.IsNone && angle < min.Value)
            {
                angle = min.Value;
            }

            if (!max.IsNone && angle > max.Value)
            {
                angle = max.Value;
            }

            return angle;
        }
    }
}