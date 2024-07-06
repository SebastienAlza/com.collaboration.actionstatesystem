using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataProperties))]
public class DataPropertiesEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DataProperties dataProperties = (DataProperties)target;

		// Encadré pour le titre et le nom du ScriptableObject
		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.LabelField("Dynamic Datas ", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Name of ScriptableObject : " + dataProperties.name);
		EditorGUILayout.EndVertical();

		if (GUILayout.Button("Add Attribute"))
		{
			dataProperties.attributes.Add(new EntityAttribute());
		}

		for (int i = 0; i < dataProperties.attributes.Count; i++)
		{
			EditorGUILayout.BeginVertical("box");
			dataProperties.attributes[i].name = EditorGUILayout.TextField("Name", dataProperties.attributes[i].name);
			dataProperties.attributes[i].type = (EntityAttribute.AttributeType)EditorGUILayout.EnumPopup("Type", dataProperties.attributes[i].type);

			switch (dataProperties.attributes[i].type)
			{
				case EntityAttribute.AttributeType.Float:
					dataProperties.attributes[i].floatValue = EditorGUILayout.FloatField("Value", dataProperties.attributes[i].floatValue);
					break;
				case EntityAttribute.AttributeType.Vector2:
					dataProperties.attributes[i].vector2Value = EditorGUILayout.Vector2Field("Value", dataProperties.attributes[i].vector2Value);
					break;
				case EntityAttribute.AttributeType.RandomFloat:
					dataProperties.attributes[i].randomFloatValue = EditorGUILayout.Vector2Field("Value", dataProperties.attributes[i].randomFloatValue);
					break;
			}

			if (GUILayout.Button("Remove Attribute"))
			{
				dataProperties.attributes.RemoveAt(i);
			}

			EditorGUILayout.EndVertical();
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty(dataProperties);
		}
	}
}
