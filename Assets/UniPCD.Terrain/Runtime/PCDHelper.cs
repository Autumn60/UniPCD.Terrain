using UnityEngine;

namespace UniPCD.Terrain
{
  public static class PCDHelper
  {
    public static Bounds GetBounds(PointCloud pointCloud)
    {
      Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

      foreach (Point point in pointCloud.points)
      {
        min = Vector3.Min(min, point.position);
        max = Vector3.Max(max, point.position);
      }

      return new Bounds((min + max) / 2, max - min);
    }
  }
}
