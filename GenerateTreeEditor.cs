using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateTree))]
public class GenerateTreeEditor : Editor
{
    private GenerateTree treeGenerator;
    private SerializedProperty autogenerateProperty;

    private void OnEnable()
    {
        treeGenerator = (GenerateTree)target;
        autogenerateProperty = serializedObject.FindProperty("autogenerate");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();
        EditorGUILayout.PropertyField(autogenerateProperty, new GUIContent("Autogenerate"));

        if (GUILayout.Button("Generate Tree") || autogenerateProperty.boolValue)
        {
            treeGenerator.GenerateTrunk();
        }

        serializedObject.ApplyModifiedProperties();
    }
}