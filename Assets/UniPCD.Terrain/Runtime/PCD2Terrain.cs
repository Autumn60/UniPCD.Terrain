using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UniPCD.Terrain
{
  using Octree = UniPCD.Octree.Octree;

  public static class PCD2Terrain
  {
    public static bool GenerateTerrain(PCD pcd, int heightmapResolution, string terrainName = "New Terrain")
    {
      Octree octree;
      Vector3 mapOrigin;
      Vector3 mapSize;
      {
        Bounds mapBounds = PCDHelper.GetBounds(pcd.pointCloud);
        octree = new Octree(mapBounds, 8, 128);
        mapOrigin = mapBounds.min;
        mapSize = mapBounds.size;
      }
      octree.Build(pcd.pointCloud.points);

      TerrainData terrainData = new TerrainData()
      {
        heightmapResolution = heightmapResolution,
        size = new Vector3(mapSize.y, mapSize.z, mapSize.x)
      };

      float[,] heights = new float[heightmapResolution, heightmapResolution];
      Vector2 cellSize = mapSize / heightmapResolution;
      Bounds cellBounds = new Bounds
      {
        center = Vector3.zero,
        size = new Vector3(cellSize.x, cellSize.y, float.MaxValue)
      };
      Vector3 cellOffset = new Vector3
      {
        x = cellSize.x / 2 + mapOrigin.x,
        y = -cellSize.y / 2 + mapOrigin.y + mapSize.y,
        z = 0.0f
      };

      for (int z = 0; z < heightmapResolution; z++)
      {
        for (int x = 0; x < heightmapResolution; x++)
        {
          cellBounds.center = new Vector3(z * cellSize.x, -x * cellSize.y, 0) + cellOffset;
          List<int> indices = octree.Query(cellBounds);
          float minHeight = float.MaxValue;
          foreach (int index in indices)
          {
            float height = pcd.pointCloud.points[index].position.z - mapOrigin.z;
            if (height < minHeight) minHeight = height;
          }
          if (minHeight == float.MaxValue) minHeight = 0.0f;
          heights[z, x] = minHeight / mapSize.z;
        }
      }
      terrainData.SetHeights(0, 0, heights);

      string terrainDataPath = $"Assets/{terrainName}.asset";
      AssetDatabase.CreateAsset(terrainData, terrainDataPath);
      AssetDatabase.SaveAssets();
      return false;
    }
  }
}