using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EntityAttribute
{
	public string name;
	public float currentValue;
}

[CreateAssetMenu(fileName = "GenericAttributes", menuName = "ScriptableObjects/GenericAttributes", order = 1)]
public class DataProperties : ScriptableObject
{
	public List<EntityAttribute> attributes = new List<EntityAttribute>();

	public float GetCurrentValue(string attributeName)
	{
		foreach (EntityAttribute entityAttribute in attributes)
		{
			if (entityAttribute.name == attributeName)
			{
				return entityAttribute.currentValue;
			}
		}
		return 0;
	}
}
