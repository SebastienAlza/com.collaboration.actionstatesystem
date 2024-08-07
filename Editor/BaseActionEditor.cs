using UnityEditor;
using UnityEngine;
using ActionStateSystem.Runtime;

namespace ActionStateSystem.CustomEditors
{
	[CustomEditor(typeof(BaseAction), true)]
	public class BaseActionEditor : Editor
	{
		SerializedProperty conditionTypeProp;
		SerializedProperty useDynamicDataProp;
		SerializedProperty dataPropertiesProp;
		SerializedProperty attributeNameProp;
		bool conditionSettingsFoldout = true;
		bool dynamicDataFoldout = true;

		private void OnEnable()
		{
			conditionTypeProp = serializedObject.FindProperty("conditionType");
			useDynamicDataProp = serializedObject.FindProperty("useDynamicData");
			dataPropertiesProp = serializedObject.FindProperty("dataProperties");
			attributeNameProp = serializedObject.FindProperty("attributeName");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Display the custom header
			BaseAction baseAction = (BaseAction)target;
			EditorGUILayout.LabelField($"Action Name: {baseAction.actionName}", EditorStyles.boldLabel);

			// Display the properties excluding the ones we handle separately
			SerializedProperty property = serializedObject.GetIterator();
			property.NextVisible(true); // Move to the first visible property
			do
			{
				if (property.name != "conditionType" && property.name != "useDynamicData" && property.name != "dataProperties" && property.name != "attributeName")
				{
					EditorGUILayout.PropertyField(property, true);
				}
			}
			while (property.NextVisible(false));

			// Foldout for Dynamic Data Properties
			dynamicDataFoldout = EditorGUILayout.Foldout(dynamicDataFoldout, "Dynamic Data Settings");
			if (dynamicDataFoldout)
			{
				EditorGUILayout.PropertyField(useDynamicDataProp);

				if (useDynamicDataProp.boolValue)
				{
					EditorGUILayout.PropertyField(dataPropertiesProp);
					EditorGUILayout.PropertyField(attributeNameProp);
				}
			}

			// Display conditionType separately
			EditorGUILayout.PropertyField(conditionTypeProp);

			// Display the properties of the condition in a foldout
			if (baseAction.GetCondition() != null)
			{
				conditionSettingsFoldout = EditorGUILayout.Foldout(conditionSettingsFoldout, "Condition Settings");
				if (conditionSettingsFoldout)
				{
					SerializedObject conditionSerializedObject = new SerializedObject(baseAction.GetCondition());
					conditionSerializedObject.Update();
					SerializedProperty conditionProp = conditionSerializedObject.GetIterator();

					conditionProp.NextVisible(true); // Go to the first visible child
					while (conditionProp.NextVisible(false)) // Iterate through the properties
					{
						// Ensure we skip "m_Script" which is a default property in Unity
						if (conditionProp.name != "m_Script" && conditionProp.name != "conditionType" && conditionProp.name != "useDynamicData" && conditionProp.name != "dataProperties" && conditionProp.name != "attributeName")
						{
							EditorGUILayout.PropertyField(conditionProp, true);
						}
					}

					conditionSerializedObject.ApplyModifiedProperties();
				}
			}

			// Apply modified properties
			serializedObject.ApplyModifiedProperties();
		}
	}
}
