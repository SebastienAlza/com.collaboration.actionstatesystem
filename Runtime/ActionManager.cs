using UnityEngine;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour
{
	public bool IsEditMode = false;
	[SerializeField] private List<BaseAction> actions = new List<BaseAction>();
	private int currentActionIndex = 0;

	private void Start()
	{
		StartCurrentAction();
	}

	private void Update()
	{
		if (currentActionIndex >= actions.Count)
		{
			ResetActions();
			StartCurrentAction();
			return;
		}

		UpdateCurrentAction();
	}

	private void StartCurrentAction()
	{
		if (currentActionIndex < actions.Count)
		{
			actions[currentActionIndex].actionManager = this;
			actions[currentActionIndex].ResetCondition();
			actions[currentActionIndex].StartAction();
		}
	}

	private void UpdateCurrentAction()
	{
		if (currentActionIndex < actions.Count)
		{
			var currentAction = actions[currentActionIndex];
			currentAction.UpdateAction();

			if (currentAction.ShouldTransition())
			{
				Debug.Log("Action " + currentAction.actionName + " completed. Moving to next.");
				currentAction.StopAction();
				currentActionIndex++;
				StartCurrentAction();
			}
		}
	}

	private void ResetActions()
	{
		currentActionIndex = 0;
		foreach (var action in actions)
		{
			action.ResetCondition();
		}
	}

	public void ActionCompleted()
	{
		if (currentActionIndex < actions.Count)
		{
			currentActionIndex++;
			StartCurrentAction();
		}
	}
}
