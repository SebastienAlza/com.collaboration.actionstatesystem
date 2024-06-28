using UnityEditor;
using UnityEngine;

namespace ActionStateSystem.Runtime
{
	[ExecuteInEditMode]
	public abstract class BaseAction : MonoBehaviour, IAction
	{
		[HideInInspector]
		public string actionName;

		public ActionManager actionManager;

		[SerializeField]
		private ConditionType conditionType;

		[SerializeField, HideInInspector]
		private BaseCondition condition;

		[SerializeField, HideInInspector]
		private int conditionUniqueID;

		private const string ConditionHolderName = "ConditionHolder";
		private GameObject conditionHolder;

		[HideInInspector]
		public bool IsSubAction = false;

		protected virtual void Awake()
		{
			// Charger la condition uniquement en mode édition
			if (!Application.isPlaying)
			{
				LoadCondition();
			}
		}

		private void OnValidate()
		{
			// Gérer les conditions uniquement en mode édition
			if (!Application.isPlaying)
			{
				EditorApplication.delayCall += ManageConditionComponentInEditor;
			}
		}

		public void SetActionManager(ActionManager manager)
		{
			actionManager = manager;
		}

		private void ManageConditionComponentInEditor()
		{
			if (this == null) return;

			EnsureConditionHolderExists();

			if (conditionType == ConditionType.None && condition != null)
			{
				var objectToDestroy = condition;
				condition = null;
				conditionUniqueID = 0; // Réinitialiser l'ID unique
				EditorApplication.delayCall += () =>
				{
					if (objectToDestroy != null)
					{
						DestroyImmediate(objectToDestroy);
					}
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
								if (objectToDestroy != null)
								{
									DestroyImmediate(objectToDestroy);
								}
							};
						}
						EditorApplication.delayCall += () =>
						{
							condition = conditionHolder.AddComponent(type) as BaseCondition;
							if (condition != null)
							{
								AssignUniqueID(condition); // Assigner un ID unique
								conditionUniqueID = condition.uniqueID; // Enregistrer l'ID unique
							}
						};
					}
					else
					{
						conditionUniqueID = condition.uniqueID; // Enregistrer l'ID unique
					}
				}
			}
		}

		private void LoadCondition()
		{
			if (conditionUniqueID != 0)
			{
				BaseCondition[] conditions = GetComponentsInChildren<BaseCondition>(true);
				foreach (var cond in conditions)
				{
					if (cond.uniqueID == conditionUniqueID)
					{
						condition = cond;
						break;
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

				case ConditionType.VisionConeCondition:
					return typeof(VisionConeCondition);

				case ConditionType.ValueCondition:
					return typeof(ValueCondition);

				case ConditionType.AlwaysTrueCondition:
					return typeof(AlwaysTrueCondition);

				case ConditionType.None:
				default:
					return null;
			}
		}

		private void AssignUniqueID(BaseCondition newCondition)
		{
			BaseCondition[] existingConditions = GetComponentsInChildren<BaseCondition>(true);
			int newID = Random.Range(1, int.MaxValue);
			bool isUnique = false;

			while (!isUnique)
			{
				isUnique = true;
				foreach (var condition in existingConditions)
				{
					if (condition.uniqueID == newID)
					{
						isUnique = false;
						newID = Random.Range(1, int.MaxValue);
						break;
					}
				}
			}

			newCondition.uniqueID = newID;
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
			// Nettoyer les conditions uniquement en mode édition
			if (!Application.isPlaying && !gameObject.activeSelf)
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
				conditionUniqueID = 0; // Réinitialiser l'ID unique
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
		VisionConeCondition,
		ValueCondition,
		AlwaysTrueCondition
		// Ajoutez d'autres types de conditions ici
	}
}
