using UnityEngine;
namespace ActionStateSystem.Runtime
{
	public class PauseAction : BaseAction
	{
		protected override void Awake()
		{
			actionName = "\"## > Action Pause";
		}

		public override void StartAction()
		{
			base.StartAction();
		}

		public override void StopAction()
		{
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (ShouldTransition())
			{
				StopAction();
			}
		}
	}
}