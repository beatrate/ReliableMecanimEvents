using UnityEditor;
using UnityEngine;

namespace Beatrate.ReliableMecanimEvents
{
	[CustomPropertyDrawer(typeof(MecanimEventInstance))]
	public class MecanimEventInstanceDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using(var propertyScope = new EditorGUI.PropertyScope(position, label, property))
			{
				Rect currentPosition = position;
				float lineStep = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				currentPosition.height = EditorGUIUtility.singleLineHeight;

				property.isExpanded = EditorGUI.Foldout(currentPosition, property.isExpanded, property.displayName);
				currentPosition.y += lineStep;

				if(property.isExpanded)
				{
					++EditorGUI.indentLevel;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("dispatchMode"));
					currentPosition.y += lineStep;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("methodName"));
					currentPosition.y += lineStep;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("normalizedTime"));
					currentPosition.y += lineStep;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("alwaysTrigger"));
					currentPosition.y += lineStep;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("repeatOnLoop"));
					currentPosition.y += lineStep;

					var parameter = property.FindPropertyRelative("parameter");
					EditorGUI.PropertyField(currentPosition, parameter);
					currentPosition.y += lineStep;

					switch((MecanimEventParameter)parameter.intValue)
					{
						case MecanimEventParameter.Int:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("intParameter"));
							currentPosition.y += lineStep;
							break;
						case MecanimEventParameter.Float:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("floatParameter"));
							currentPosition.y += lineStep;
							break;
						case MecanimEventParameter.String:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("stringParameter"));
							currentPosition.y += lineStep;
							break;
						case MecanimEventParameter.Object:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("objectParameter"));
							currentPosition.y += lineStep;
							break;
						case MecanimEventParameter.Event:
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("intParameter"));
							currentPosition.y += lineStep;
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("floatParameter"));
							currentPosition.y += lineStep;

							currentPosition.y += lineStep;
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("stringParameter"));
							currentPosition.y += lineStep;
							EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("objectParameter"));
							currentPosition.y += lineStep;

							break;
						default:
							break;
					}

					var animatorActions = property.FindPropertyRelative("animatorActions");
					EditorGUI.PropertyField(currentPosition, animatorActions, includeChildren: true);
					currentPosition.y += EditorGUI.GetPropertyHeight(animatorActions, includeChildren: true);

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("debugBreak"));
					currentPosition.y += lineStep;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("debugLog"));
					currentPosition.y += lineStep;

					EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("debugString"));
					currentPosition.y += lineStep;

					--EditorGUI.indentLevel;
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if(property.isExpanded)
			{
				int propertyCount = 6;
				MecanimEventParameter parameter = (MecanimEventParameter)property.FindPropertyRelative("parameter").intValue;
				if(parameter == MecanimEventParameter.Event)
				{
					propertyCount += 4;
				}
				else if(parameter != MecanimEventParameter.None)
				{
					++propertyCount;
				}

				// Debug Break, Debug Log, Debug String.
				propertyCount += 3;

				int lineCount = propertyCount + 1; // Count foldout as well.
				return EditorGUIUtility.singleLineHeight * lineCount + EditorGUIUtility.standardVerticalSpacing * (lineCount - 1) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("animatorActions"), includeChildren: true);
			}

			return EditorGUIUtility.singleLineHeight;
		}
	}
}
