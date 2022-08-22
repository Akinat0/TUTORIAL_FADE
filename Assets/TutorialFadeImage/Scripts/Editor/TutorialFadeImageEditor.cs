namespace Abu
{
    using UnityEditor;
    using UnityEditor.UI;

    /// <summary>
    /// Custom editor for TutorialFadeImage.
    /// </summary>
    [CustomEditor(typeof(TutorialFadeImage))]
    public class TutorialFadeImageEditor : ImageEditor
    {
        SerializedProperty holeSizeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            holeSizeProperty = serializedObject.FindProperty("smoothness");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SpriteGUI();
            AppearanceControlsGUI();
            EditorGUILayout.PropertyField(holeSizeProperty);
            EditorGUILayout.PropertyField(m_RaycastTarget);

            serializedObject.ApplyModifiedProperties();
        }
    }

}