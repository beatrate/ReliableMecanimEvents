using UnityEditor;
using UnityEngine;

namespace Beatrate.ReliableMecanimEvents
{
	[CustomPropertyDrawer(typeof(MecanimAnimatorAction))]
	public class MecanimAnimatorActionDrawer : PropertyDrawer
	{
		private const float HorizontalSpacing = 4.0f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using(var propertyScope = new EditorGUI.PropertyScope(position, label, property))
			{
				Rect currentPosition = position;
				currentPosition.width = 150.0f;
				SerializedProperty variableName = property.FindPropertyRelative("variableName");
				EditorGUI.PropertyField(currentPosition, variableName, GUIContent.none);
				currentPosition.x += currentPosition.width + HorizontalSpacing;

				currentPosition.width = 75.0f;
				SerializedProperty variableType = property.FindPropertyRelative("variableType");
				EditorGUI.PropertyField(currentPosition, variableType, GUIContent.none);
				currentPosition.x += currentPosition.width + HorizontalSpacing;

				currentPosition.width = position.x + position.width - currentPosition.x;
				if(currentPosition.width > 0.0f)
				{
					switch((MecanimAnimatorParameterType)variableType.intValue)
					{
						case MecanimAnimatorParameterType.Float:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("floatValue"), GUIContent.none);
							break;
						case MecanimAnimatorParameterType.Int:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("intValue"), GUIContent.none);
							break;
						case MecanimAnimatorParameterType.Bool:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("boolValue"), GUIContent.none);
							break;
						case MecanimAnimatorParameterType.Trigger:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("triggerValue"), GUIContent.none);
							break;
					}
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}
