using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public abstract class BaseAction : MonoBehaviour, IAction
{
	public string actionName;
	public ActionManager actionManager;

	[SerializeField]
	private ConditionType conditionType;

	[SerializeField, HideInInspector]
	private BaseCondition condition;

	private const string ConditionHolderName = "ConditionHolder";
	private GameObject conditionHolder;

	[HideInInspector]
	public bool IsSubAction = false;

	protected virtual void Awake()
	{
	}

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			EditorApplication.delayCall += ManageConditionComponentInEditor;
		}
	}

	private void ManageConditionComponentInEditor()
	{
		if (this == null) return;

		EnsureConditionHolderExists();

		if (conditionType == ConditionType.None && condition != null)
		{
			var objectToDestroy = condition;
			condition = null;
			EditorApplication.delayCall += () =>
			{
				DestroyImmediate(objectToDestroy);
				CleanupConditionHolder();
			};
		}
		else if (conditionType != ConditionType.None)
		{
			System.Type type = GetConditionType(conditionType);
			if (type != null)
			{
				if (condition == null || condition.GetType() != type)
				{
					var objectToDestroy = condition;
					if (objectToDestroy != null)
					{
						EditorApplication.delayCall += () =>
						{
							DestroyImmediate(objectToDestroy);
						};
					}
					EditorApplication.delayCall += () =>
					{
						condition = conditionHolder.AddComponent(type) as BaseCondition;
					};
				}
			}
		}
	}

	private void EnsureConditionHolderExists()
	{
		if (conditionHolder == null)
		{
			conditionHolder = transform.Find(ConditionHolderName)?.gameObject;

			if (conditionHolder == null)
			{
				conditionHolder = new GameObject(ConditionHolderName);
				conditionHolder.transform.SetParent(transform);
				conditionHolder.transform.localPosition = Vector3.zero;
			}
		}
	}

	private void CleanupConditionHolder()
	{
		if (conditionHolder != null)
		{
			var components = conditionHolder.GetComponents<Component>();
			if (components.Length == 1)
			{
				DestroyImmediate(conditionHolder);
				conditionHolder = null;
			}
		}
	}

	private System.Type GetConditionType(ConditionType conditionType)
	{
		switch (conditionType)
		{
			case ConditionType.TriggerCondition:
				return typeof(TriggerCondition);

			case ConditionType.TimerCondition:
				return typeof(TimeCondition);

			case ConditionType.ValueCondition:
				return typeof(ValueCondition);

			case ConditionType.None:
			default:
				return null;
		}
	}

	public BaseCondition GetCondition()
	{
		return condition;
	}

	public void ResetCondition()
	{
		if (condition != null && IsSubAction != true)
		{
			condition.Deactivate();
			condition.Activate();
		}
	}

	public abstract void Execute();

	public virtual bool ShouldTransition()
	{
		if (conditionType == ConditionType.None) return false;
		bool result = condition != null && condition.IsActive && condition.IsMet();
		return result;
	}

	public virtual void StartAction()
	{
		if (condition != null && IsSubAction != true)
		{
			condition.Activate();
		}
	}

	public virtual void StopAction()
	{
		if (condition != null && IsSubAction != true)
		{
			condition.Deactivate();
		}
		if (actionManager != null && IsSubAction != true)
		{
			actionManager.ActionCompleted();
		}
	}

	private void OnDisable()
	{
		if (!Application.isPlaying)
		{
			CleanupCondition();
		}
	}

	private void CleanupCondition()
	{
		if (condition != null)
		{
			DestroyImmediate(condition);
			condition = null;
		}

		if (conditionHolder != null)
		{
			var components = conditionHolder.GetComponents<Component>();
			if (components.Length <= 1) // Seul le Transform reste
			{
				DestroyImmediate(conditionHolder);
				conditionHolder = null; 
			}
		}
	}

	public abstract void UpdateAction();
}

public interface IAction
{
	void Execute();

	bool ShouldTransition();

	void StartAction();

	void StopAction();

	void UpdateAction();
}

public enum ConditionType
{
	None,
	TriggerCondition,
	TimerCondition,
	ValueCondition
	// Ajoutez d'autres types de conditions ici
}