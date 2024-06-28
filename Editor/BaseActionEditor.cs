using UnityEditor;
using UnityEngine;
using ActionStateSystem.Runtime;

namespace ActionStateSystem.CustomEditors
{
	[CustomEditor(typeof(BaseAction), true)]
	public class BaseActionEditor : Editor
	{
		SerializedProperty conditionTypeProp;
		SerializedProperty actionColorProp;

		private void OnEnable()
		{
			conditionTypeProp = serializedObject.FindProperty("conditionType");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Display the custom header
			BaseAction baseAction = (BaseAction)target;
			EditorGUILayout.LabelField($"Action Name: {baseAction.actionName}");

			// Display the other properties
			DrawPropertiesExcluding(serializedObject, "conditionType", "condition", "actionColor");

			// Display conditionType separately
			EditorGUILayout.PropertyField(conditionTypeProp);

			// Display the properties of the condition in a foldout
			if (baseAction.GetCondition() != null)
			{
				if (EditorGUILayout.Foldout(true, "Condition Settings"))
				{
					SerializedObject conditionSerializedObject = new SerializedObject(baseAction.GetCondition());
					SerializedProperty conditionProp = conditionSerializedObject.GetIterator();

					conditionProp.NextVisible(true); // Go to the first visible child
					while (conditionProp.NextVisible(false)) // Iterate through the properties
					{
						EditorGUILayout.PropertyField(conditionProp, true);
					}

					conditionSerializedObject.ApplyModifiedProperties();
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

	}
}
