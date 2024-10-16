using UnityEngine;
using UnityEditor;

namespace ComponentUtils
{
    [CustomPropertyDrawer(typeof(AutoIDAttribute))]
    public class AutoIDDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var idProperty = property.FindPropertyRelative("id");
            var groupProperty = property.FindPropertyRelative("group");

            if (idProperty != null && groupProperty != null)
            {
                var autoIDAttribute = attribute as AutoIDAttribute;

                if (idProperty.intValue == 0)
                {
                    autoIDAttribute.GenerateIdIfNeeded(property.serializedObject.targetObject);
                    property.serializedObject.ApplyModifiedProperties();
                }

                // Layout do editor
                float labelWidth = EditorGUIUtility.labelWidth;
                float fieldWidth = (position.width - labelWidth) / 2;

                // Label
                Rect labelRect = new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelRect, label);

                // Campo Group
                Rect groupRect = new Rect(position.x + labelWidth, position.y, fieldWidth - 5, EditorGUIUtility.singleLineHeight);
                groupProperty.enumValueIndex = (int)(IdGroup)EditorGUI.EnumPopup(groupRect, (IdGroup)groupProperty.enumValueIndex);

                // Campo ID
                Rect idRect = new Rect(position.x + labelWidth + fieldWidth, position.y, fieldWidth, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(idRect, idProperty, GUIContent.none);

                // Verifica se o ID já está em uso
                bool isIdInUse = ID.IsIdInUse(idProperty.intValue, (IdGroup)groupProperty.enumValueIndex);
                int instanceCount = ID.CountInstances(idProperty.intValue);

                // Se o ID estiver em uso e houver mais de uma instância, exibe mensagem de erro
                if (isIdInUse && instanceCount > 1)
                {
                    Rect errorRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.LabelField(errorRect, $"ID {idProperty.intValue} já está em uso por {instanceCount} instâncias!", EditorStyles.boldLabel);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isIdInUse = ID.IsIdInUse(property.FindPropertyRelative("id").intValue, (IdGroup)property.FindPropertyRelative("group").enumValueIndex);
            int instanceCount = ID.CountInstances(property.FindPropertyRelative("id").intValue);

            if (isIdInUse && instanceCount > 1)
            {
                return EditorGUIUtility.singleLineHeight * 2; 
            }

            return EditorGUIUtility.singleLineHeight;
        }
    }
}
