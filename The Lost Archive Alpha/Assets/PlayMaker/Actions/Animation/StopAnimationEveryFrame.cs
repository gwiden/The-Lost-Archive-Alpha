// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Stops all playing Animations on a Game Object. Optionally, specify a single Animation to Stop.")]
	public class StopAnimationEveryFrame : BaseAnimationAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;
		[Tooltip("Leave empty to stop all playing animations.")]
		[UIHint(UIHint.Animation)]
		public FsmString animName;

        [Tooltip("Repeat every frame. Typically this would be set to True.")]
        public bool everyFrame;

        public override void Reset()
		{
			gameObject = null;
			animName = null;
            everyFrame = false;
        }

		public override void OnEnter()
		{
			DoStopAnimation();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoStopAnimation();
        }

        private void DoStopAnimation()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (!UpdateCache(go))
		    {
		        return;
		    }

            if (FsmString.IsNullOrEmpty(animName))
            {
                animation.Stop();
            }
            else
            {
                animation.Stop(animName.Value);
            }
		}

        /*
			public override string ErrorCheck()
			{
				return ErrorCheckHelpers.CheckAnimationSetup(gameObject.value);
			}*/
	}
}