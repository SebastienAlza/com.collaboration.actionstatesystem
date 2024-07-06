using System.Collections.Generic;
using UnityEngine;

namespace ActionStateSystem.Runtime
{
	public class ActionManager : MonoBehaviour
	{
		[SerializeField] private List<BaseAction> actions = new List<BaseAction>();
		private List<BaseAction> actionsWithConditions = new List<BaseAction>();
		private List<BaseAction> actionsWithoutConditions = new List<BaseAction>();
		private int currentActionIndexWithCondition = 0;

		private void Start()
		{
			foreach (var action in actions)
			{
				if (action.conditionType == ConditionType.None)
				{
					actionsWithoutConditions.Add(action);
					action.StartAction();  // D�marrer les actions sans conditions d�s le d�but
				}
				else
				{
					actionsWithConditions.Add(action);
				}
			}

			StartCurrentActionWithCondition();
		}

		private void Update()
		{
			UpdateCurrentActionWithCondition();
			UpdateCurrentActionsWithoutConditions();
		}

		private void StartCurrentActionWithCondition()
		{
			if (currentActionIndexWithCondition < actionsWithConditions.Count)
			{
				var currentAction = actionsWithConditions[currentActionIndexWithCondition];
				currentAction.actionManager = this;
				//currentAction.ResetCondition();
				currentAction.StartAction();
			}
		}

		private void UpdateCurrentActionWithCondition()
		{
			if (currentActionIndexWithCondition < actionsWithConditions.Count)
			{
				var currentAction = actionsWithConditions[currentActionIndexWithCondition];
				currentAction.UpdateAction();

				if (currentAction.ShouldTransition())
				{
					Debug.Log("Action " + currentAction.actionName + " termin�e. Passer � la suivante.");
					currentAction.StopAction();
					currentActionIndexWithCondition++;
					StartCurrentActionWithCondition();
				}
			}
			else
			{
				// Toutes les actions avec condition sont termin�es, on recommence du d�but
				ResetActionsWithCondition();
				StartCurrentActionWithCondition();
			}
		}

		private void UpdateCurrentActionsWithoutConditions()
		{
			foreach (BaseAction currentAction in actionsWithoutConditions)
			{
				currentAction.UpdateAction();

				// Restart the action immediately after stopping it
				if (currentAction.ShouldTransition())
				{
					currentAction.StopAction();
					currentAction.StartAction();
				}
			}
		}

		private void ResetActionsWithCondition()
		{
			currentActionIndexWithCondition = 0;
			foreach (var action in actionsWithConditions)
			{
				action.ResetCondition();
			}
		}

		public void ActionWithConditionCompleted()
		{
			if (currentActionIndexWithCondition < actionsWithConditions.Count)
			{
				currentActionIndexWithCondition++;
				StartCurrentActionWithCondition();
			}
		}
	}
}
