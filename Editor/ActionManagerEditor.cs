using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using ActionStateSystem.Runtime;

namespace ActionStateSystem.editor
{
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

			if (GUILayout.Button("Configure Actions"))
			{
				ConfigureActions(actionManager);
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void ConfigureActions(ActionManager actionManager)
		{
			foreach (var action in actionManager.GetComponentsInChildren<BaseAction>())
			{
				action.SetActionManager(actionManager);
				EditorUtility.SetDirty(action);
			}
			EditorUtility.SetDirty(actionManager);
		}
	}
}