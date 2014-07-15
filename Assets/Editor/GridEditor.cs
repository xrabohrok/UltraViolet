using UnityEngine;
using UnityEditor;
using ultraviolet.builder;


namespace ultraviolet.editors{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {
        SerializedProperty widthScale;
        SerializedProperty widthCount;
        SerializedProperty lengthCount;

        bool dirty = true;
	
        public override void OnInspectorGUI()
        {
            if(dirty)
            {
                widthCount = serializedObject.FindProperty("widthCount");
                lengthCount = serializedObject.FindProperty("lengthCount");
                widthScale = serializedObject.FindProperty("widthScale");
                
                dirty = false;
            }
            serializedObject.Update();

            EditorGUILayout.IntSlider(lengthCount, 1, 100, new GUIContent("Cells wide"));
            EditorGUILayout.IntSlider(widthCount,1,100, new GUIContent("Cells long"));
            EditorGUILayout.PropertyField(widthScale, new GUIContent("Cell Width"));


            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();

	    }
    }
}
