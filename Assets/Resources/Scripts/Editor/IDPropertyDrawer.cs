using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ComponentUtils;

namespace Tcp4
{

    [CustomPropertyDrawer(typeof(ID))]
    public class IDPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float spacing = 5f;
            float idWidth = position.width * 0.4f;
            float groupWidth = position.width * 0.4f - spacing;
            float resetWidth = position.width * 0.2f - spacing;

            Rect idRect = new Rect(position.x, position.y, idWidth, position.height);
            Rect groupRect = new Rect(position.x + idWidth + spacing, position.y, groupWidth, position.height);
            Rect resetRect = new Rect(position.x + idWidth + groupWidth + spacing * 2, position.y, resetWidth, position.height);

            SerializedProperty idProperty = property.FindPropertyRelative("id");
            SerializedProperty groupProperty = property.FindPropertyRelative("group");

            IdGroup oldGroup = (IdGroup)groupProperty.enumValueIndex;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(idRect, idProperty, new GUIContent("ID"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.PropertyField(groupRect, groupProperty, GUIContent.none);

            if (GUI.Button(resetRect, "Reset"))
            {
                ResetToOriginal(property);
            }

            if (oldGroup != (IdGroup)groupProperty.enumValueIndex)
            {
                HandleGroupChange(property, oldGroup);
            }

            EditorGUI.EndProperty();
        }

        private void HandleGroupChange(SerializedProperty property, IdGroup oldGroup)
        {
            SerializedProperty groupProperty = property.FindPropertyRelative("group");
            var newGroup = (IdGroup)groupProperty.enumValueIndex;

            var target = property.serializedObject.targetObject;
            var idField = target.GetType().GetField(property.name);
            if (idField != null)
            {
                var idInstance = idField.GetValue(target) as ID;
                if (idInstance != null)
                {
                    idInstance.ChangeGroup(newGroup);
                    property.serializedObject.Update();
                }
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        private void ResetToOriginal(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
            var idField = target.GetType().GetField(property.name);
            if (idField != null)
            {
                var idInstance = idField.GetValue(target) as ID;
                if (idInstance != null)
                {
                    idInstance.ResetToOriginal();
                    property.serializedObject.Update();
                }
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
