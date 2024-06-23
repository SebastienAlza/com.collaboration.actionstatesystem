using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType == SerializedPropertyType.String)
		{
			EditorGUI.BeginProperty(position, label, property);

			// Get the list of tags
			string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

			// Find the index of the current tag
			int index = System.Array.IndexOf(tags, property.stringValue);
			if (index == -1) index = 0;

			// Show the popup
			index = EditorGUI.Popup(position, label.text, index, tags);

			// Update the property value
			property.stringValue = tags[index];

			EditorGUI.EndProperty();
		}
		else
		{
			EditorGUI.PropertyField(position, property, label);
		}
	}
}