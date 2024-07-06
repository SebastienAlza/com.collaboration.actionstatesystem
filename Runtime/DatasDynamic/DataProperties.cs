using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

[System.Serializable]
public class EntityAttribute
{
	public string name;

	public enum AttributeType
	{
		Float,
		Vector2,
		RandomFloat
	}

	public AttributeType type;

	public float floatValue;
	public Vector2 vector2Value;
	public Vector2 randomFloatValue;

	public object GetValue()
	{
		switch (type)
		{
			case AttributeType.Float:
				return floatValue;
			case AttributeType.Vector2:
				return vector2Value;
			case AttributeType.RandomFloat:
				return randomFloatValue;
			default:
				return null;
		}
	}
}

[CreateAssetMenu(fileName = "GenericAttributes", menuName = "ScriptableObjects/GenericAttributes", order = 1)]
public class DataProperties : ScriptableObject
{
	public List<EntityAttribute> attributes = new List<EntityAttribute>();

	public float GetCurrentFloatValue(string attributeName)
	{
		foreach (EntityAttribute entityAttribute in attributes)
		{
			if (entityAttribute.name == attributeName && entityAttribute.type == EntityAttribute.AttributeType.Float)
			{
				return entityAttribute.floatValue;
			}
		}
		return 0f;
	}

	public float GetCurrentRandomFloatValue(string attributeName)
	{
		foreach (EntityAttribute entityAttribute in attributes)
		{
			if (entityAttribute.name == attributeName && entityAttribute.type == EntityAttribute.AttributeType.RandomFloat)
			{
				// Debug log to check the values
				Debug.Log($"RandomFloat range for {attributeName}: {entityAttribute.randomFloatValue.x} to {entityAttribute.randomFloatValue.y}");
				return Random.Range(entityAttribute.randomFloatValue.x, entityAttribute.randomFloatValue.y);
			}
		}
		return 0f;
	}

	public Vector2 GetCurrentVector2Value(string attributeName)
	{
		foreach (EntityAttribute entityAttribute in attributes)
		{
			if (entityAttribute.name == attributeName && entityAttribute.type == EntityAttribute.AttributeType.Vector2)
			{
				return entityAttribute.vector2Value;
			}
		}
		return Vector2.zero;
	}
}
