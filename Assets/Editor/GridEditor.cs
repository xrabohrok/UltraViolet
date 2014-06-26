using UnityEngine;
using UnityEditor;
using ultraviolet.builder;


namespace ultraviolet.editors{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {
        SerializedProperty width;
        SerializedProperty widthCount;
        SerializedProperty lengthCount;

        bool dirty = true;
	
        public override void OnInspectorGUI()
        {
            if(dirty)
            {
                widthCount = serializedObject.FindProperty("widthCount");
                lengthCount = serializedObject.FindProperty("lengthCount");
                dirty = false;
            }
            serializedObject.Update();

            EditorGUILayout.PropertyField(widthCount, new GUIContent("Cells long"));

            serializedObject.ApplyModifiedProperties();

	    }
    }
}
