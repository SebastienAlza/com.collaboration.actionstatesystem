using System.Collections.Generic;
using UnityEngine;
namespace ActionStateSystem.Runtime
{
	public class SubActionManager : BaseAction
	{
		public List<BaseAction> subActions = new List<BaseAction>();
		private int currentSubActionIndex = 0;
		private bool isActionRunning;

		protected override void Awake()
		{
			base.Awake();
			foreach (var subAction in subActions)
			{
				subAction.actionManager = this.actionManager; // Assigner le ActionManager parent
				subAction.IsSubAction = true;
			}
		}
		public override void StartAction()
		{
			Debug.Log("SubActionManager " + this.actionName + " démarrée");
			isActionRunning = true;
			currentSubActionIndex = 0;

			if (subActions.Count > 0)
			{
				subActions[currentSubActionIndex].ResetCondition();
				subActions[currentSubActionIndex].StartAction();
			}

			foreach (BaseAction action in subActions)
			{
				action.StartAction();
			}

			base.StartAction();
		}

		public override void StopAction()
		{
			Debug.Log("SubActionManager " + this.actionName + " arrêtée");
			isActionRunning = false;

			foreach (BaseAction action in subActions)
			{
				action.StopAction();
			}
			base.StopAction();
		}

		public override void UpdateAction()
		{
			if (!isActionRunning)
			{
				return;
			}

			foreach (BaseAction action in subActions)
			{
				action.UpdateAction();
			}

			// Vérifier la condition de SubActionManager
			if (base.ShouldTransition())
			{
				Debug.Log("SubActionManager condition met, stopping all sub-actions.");
				StopAction();
				isActionRunning = false;
				return;
			}
		}

		public override bool ShouldTransition()
		{
			bool result = base.ShouldTransition();
			Debug.Log("SubActionManager " + this.actionName + " ShouldTransition: " + result);
			return result;
		}

		public List<string> GetSubActionNames()
		{
			List<string> names = new List<string>();
			foreach (var subAction in subActions)
			{
				if (subAction != null)
				{
					names.Add(subAction.actionName);
				}
			}
			return names;
		}
	}

}