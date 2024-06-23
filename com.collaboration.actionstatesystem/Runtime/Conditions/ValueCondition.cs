using UnityEngine;

public class ValueCondition : BaseCondition
{
	[SerializeField] private Component targetComponent; // Composant cible
	[SerializeField] private string fieldName; // Nom de la propriété ou du champ à vérifier
	[SerializeField] private float targetValueFloat; // Valeur cible pour les floats
	[SerializeField] private bool targetValueBool; // Valeur cible pour les booléens
	[SerializeField] private ComparisonType comparisonType = ComparisonType.GreaterThanOrEqual;

	public override bool IsMet()
	{
		if (targetComponent == null)
		{
			Debug.LogWarning("ValueCondition: Target component not set.");
			return false;
		}

		var targetType = targetComponent.GetType();
		var targetField = targetType.GetField(fieldName);
		var targetProperty = targetType.GetProperty(fieldName);

		if (targetField == null && targetProperty == null)
		{
			Debug.LogWarning($"ValueCondition: Field or property '{fieldName}' not found on component '{targetType.Name}'.");
			return false;
		}

		if (targetField != null)
		{
			if (targetField.FieldType == typeof(float))
			{
				float currentValue = (float)targetField.GetValue(targetComponent);
				return CompareValues(currentValue);
			}
			else if (targetField.FieldType == typeof(bool))
			{
				bool currentValue = (bool)targetField.GetValue(targetComponent);
				return CompareValues(currentValue);
			}
			else
			{
				Debug.LogWarning($"ValueCondition: Field '{fieldName}' is not of type float or bool.");
				return false;
			}
		}
		else if (targetProperty != null)
		{
			if (targetProperty.PropertyType == typeof(float))
			{
				float currentValue = (float)targetProperty.GetValue(targetComponent);
				return CompareValues(currentValue);
			}
			else if (targetProperty.PropertyType == typeof(bool))
			{
				bool currentValue = (bool)targetProperty.GetValue(targetComponent);
				return CompareValues(currentValue);
			}
			else
			{
				Debug.LogWarning($"ValueCondition: Property '{fieldName}' is not of type float or bool.");
				return false;
			}
		}

		return false;
	}

	private bool CompareValues(float currentValue)
	{
		switch (comparisonType)
		{
			case ComparisonType.GreaterThanOrEqual:
				return currentValue >= targetValueFloat;
			case ComparisonType.LessThanOrEqual:
				return currentValue <= targetValueFloat;
			case ComparisonType.Equal:
				return Mathf.Approximately(currentValue, targetValueFloat);
			default:
				return false;
		}
	}

	private bool CompareValues(bool currentValue)
	{
		return currentValue == targetValueBool;
	}
}

public enum ComparisonType
{
	GreaterThanOrEqual,
	LessThanOrEqual,
	Equal
}
