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

		// Afficher les autres propri�t�s du script sauf conditionType
		DrawPropertiesExcluding(serializedObject, "conditionType", "condition");

		// Afficher conditionType s�par�ment
		EditorGUILayout.PropertyField(conditionTypeProp);

		BaseAction baseAction = (BaseAction)target;

		// Afficher les propri�t�s de la condition dans un repli
		if (baseAction.GetCondition() != null)
		{
			if (EditorGUILayout.Foldout(true, "Condition Settings"))
			{
				SerializedObject conditionSerializedObject = new SerializedObject(baseAction.GetCondition());
				SerializedProperty conditionProp = conditionSerializedObject.GetIterator();

				conditionProp.NextVisible(true); // Aller au premier enfant visible
				while (conditionProp.NextVisible(false)) // It�rer � travers les propri�t�s
				{
					EditorGUILayout.PropertyField(conditionProp, true);
				}

				conditionSerializedObject.ApplyModifiedProperties();
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}
