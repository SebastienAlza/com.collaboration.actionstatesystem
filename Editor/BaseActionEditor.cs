using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseAction), true)]
public class BaseActionEditor : Editor
{
	SerializedProperty conditionTypeProp;

	private void OnEnable()
	{
		conditionTypeProp = serializedObject.FindProperty("conditionType");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		// Afficher les autres propriétés du script sauf conditionType
		DrawPropertiesExcluding(serializedObject, "conditionType", "condition");

		// Afficher conditionType séparément
		EditorGUILayout.PropertyField(conditionTypeProp);

		BaseAction baseAction = (BaseAction)target;

		// Afficher les propriétés de la condition dans un repli
		if (baseAction.GetCondition() != null)
		{
			if (EditorGUILayout.Foldout(true, "Condition Settings"))
			{
				SerializedObject conditionSerializedObject = new SerializedObject(baseAction.GetCondition());
				SerializedProperty conditionProp = conditionSerializedObject.GetIterator();

				conditionProp.NextVisible(true); // Aller au premier enfant visible
				while (conditionProp.NextVisible(false)) // Itérer à travers les propriétés
				{
					EditorGUILayout.PropertyField(conditionProp, true);
				}

				conditionSerializedObject.ApplyModifiedProperties();
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}
