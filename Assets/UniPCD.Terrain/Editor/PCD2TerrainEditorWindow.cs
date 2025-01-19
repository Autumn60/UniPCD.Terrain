using UnityEditor;
using UnityEngine;

namespace UniPCD.Terrain.Editor
{
  public class PCD2TerrainEditorWindow : EditorWindow
  {
    private PCDObject pcdObject;
    private int heightmapResolution = 2049;
    [SerializeField]
    private float[] querySizeMagnifications = { 1.0f };

    [MenuItem("UniPCD/Terrain/Generate Terrain from PCD")]
    public static void ShowWindow()
    {
      GetWindow<PCD2TerrainEditorWindow>("PCD2Terrain");
    }

    private void OnGUI()
    {
      GUILayout.Label("Generate Terrain from PCD", EditorStyles.boldLabel);
      pcdObject = (PCDObject)EditorGUILayout.ObjectField("PCD Object", pcdObject, typeof(PCDObject), true);
      heightmapResolution = EditorGUILayout.IntField("Heightmap Resolution", heightmapResolution);
      heightmapResolution = Mathf.Clamp(heightmapResolution, 33, 4097);

      SerializedObject so = new SerializedObject(this);
      EditorGUILayout.PropertyField(so.FindProperty("querySizeMagnifications"), true);
      so.ApplyModifiedProperties();

      if (pcdObject)
      {
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Terrain"))
        {
          PCD2Terrain.GenerateTerrain(pcdObject.pcd, heightmapResolution, querySizeMagnifications, $"{pcdObject.name}_terrain");
        }
      }
    }
  }
}