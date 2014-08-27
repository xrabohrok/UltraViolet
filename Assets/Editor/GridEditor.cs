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
            var GridParent = (Grid)this.target;

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
            GridParent.basePrefab = (GameObject)EditorGUILayout.ObjectField(GridParent.basePrefab, typeof(GameObject), false);
            
            if(GUILayout.Button("Refresh"))
            {
                GridParent.refreshEditorView();
            }


            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();

	    }
    }

    [CustomEditor(typeof(Cell))]
    public class CellEditor : Editor
    {
        [DrawGizmo(GizmoType.Selected| GizmoType.NotSelected)]
        static void DrawGameObjectName(Transform transform, GizmoType gizmoType)
        {
            if(transform.GetComponents<Cell>().Length >= 1)
            Handles.Label(transform.position, transform.gameObject.name);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
