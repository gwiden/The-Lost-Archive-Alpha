// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Clamp the Position of a Game Object")]
	public class ClampPosition : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

        [RequiredField]
        [Tooltip("The minimum value X.")]
        public FsmFloat xMin;

        [RequiredField]
        [Tooltip("The maximum value X.")]
        public FsmFloat xMax;

        [RequiredField]
        [Tooltip("The minimum value Y.")]
        public FsmFloat yMin;

        [RequiredField]
        [Tooltip("The maximum value Y.")]
        public FsmFloat yMax;

        [RequiredField]
        [Tooltip("The minimum value Z.")]
        public FsmFloat zMin;

        [RequiredField]
        [Tooltip("The maximum value Z.")]
        public FsmFloat zMax;
		
		public Space space;
		
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
            xMin = float.NegativeInfinity;
            xMax = float.PositiveInfinity;
            yMin = float.NegativeInfinity;
            yMax = float.PositiveInfinity;
            zMin = float.NegativeInfinity;
            zMax = float.PositiveInfinity;
            space = Space.World;
			everyFrame = false;
		}

		public override void OnEnter()
		{
            DoClampTransform();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnUpdate()
		{
            DoClampTransform();
		}

        void DoClampTransform()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            var position = space == Space.World ? go.transform.position : go.transform.localPosition;

            position.x = Mathf.Clamp(position.x, xMin.Value, xMax.Value); // clamp position x
            position.y = Mathf.Clamp(position.y, yMin.Value, yMax.Value); // clamp position y
            position.z = Mathf.Clamp(position.z, zMin.Value, zMax.Value); // clamp position z

            if (space == Space.World)
            {
                go.transform.position = position;
            }
            else
            {
                go.transform.localPosition = position;
            }
        }


    }
}