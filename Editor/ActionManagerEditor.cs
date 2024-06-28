using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using ActionStateSystem.Runtime;

namespace ActionStateSystem.CustomEditors
{
	[CustomEditor(typeof(ActionManager))]
	public class ActionManagerEditor : UnityEditor.Editor
	{
		private SerializedProperty actionsProperty;

		private void OnEnable()
		{
			actionsProperty = serializedObject.FindProperty("actions");
		}

		public override void OnInspectorGUI()
		{
			ActionManager actionManager = (ActionManager)target;

			serializedObject.Update();

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
						// Concaténer les noms des subActions s'il s'agit d'un SubActionManager
						string actionNames = action.actionName;
						if (action is SubActionManager subActionManager)
						{
							List<string> subActionNames = subActionManager.GetSubActionNames();
							if (subActionNames.Count > 0)
							{
								actionNames += "SubActions: " + string.Join(" & ", subActionNames) + "";
							}
						}

						EditorGUILayout.LabelField($"Action {i + 1}: {actionNames}");

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
