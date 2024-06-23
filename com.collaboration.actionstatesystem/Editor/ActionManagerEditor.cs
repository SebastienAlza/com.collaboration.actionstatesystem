using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ActionManager))]
public class ActionManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		ActionManager actionManager = (ActionManager)target;

		serializedObject.Update();

		SerializedProperty actionsProperty = serializedObject.FindProperty("actions");

		EditorGUILayout.PropertyField(actionsProperty, new GUIContent("Actions"), true);

		if (actionsProperty.isExpanded)
		{
			EditorGUI.indentLevel++;

			for (int i = 0; i < actionsProperty.arraySize; i++)
			{
				SerializedProperty actionProperty = actionsProperty.GetArrayElementAtIndex(i);
				BaseAction action = (BaseAction)actionProperty.objectReferenceValue;
				if (action != null)
				{
					EditorGUILayout.LabelField($"Action {i + 1}: {action.actionName}");
				}
				else
				{
					EditorGUILayout.LabelField($"Action {i + 1}: (null)");
				}
			}

			EditorGUI.indentLevel--;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
