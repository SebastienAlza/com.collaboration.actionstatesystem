using UnityEngine;

public class ValueCondition : BaseCondition
{
	[SerializeField] private Component targetComponent; // Target component
	[SerializeField] private string fieldName; // Name of the field or property to check
	[SerializeField] private float targetValueFloat; // Target value for floats
	[SerializeField] private bool targetValueBool; // Target value for booleans
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
			return CheckField(targetField);
		}
		else if (targetProperty != null)
		{
			return CheckProperty(targetProperty);
		}

		return false;
	}

	private bool CheckField(System.Reflection.FieldInfo field)
	{
		if (field.FieldType == typeof(float))
		{
			float currentValue = (float)field.GetValue(targetComponent);
			return CompareValues(currentValue);
		}
		else if (field.FieldType == typeof(bool))
		{
			bool currentValue = (bool)field.GetValue(targetComponent);
			return CompareValues(currentValue);
		}
		else
		{
			Debug.LogWarning($"ValueCondition: Field '{field.Name}' is not of type float or bool.");
			return false;
		}
	}

	private bool CheckProperty(System.Reflection.PropertyInfo property)
	{
		if (property.PropertyType == typeof(float))
		{
			float currentValue = (float)property.GetValue(targetComponent);
			return CompareValues(currentValue);
		}
		else if (property.PropertyType == typeof(bool))
		{
			bool currentValue = (bool)property.GetValue(targetComponent);
			return CompareValues(currentValue);
		}
		else
		{
			Debug.LogWarning($"ValueCondition: Property '{property.Name}' is not of type float or bool.");
			return false;
		}
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
