// Decompiled with JetBrains decompiler
// Type: PicoGames.Utilities.Shape
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace PicoGames.Utilities
{
  public class Shape
  {
    private const float SQR_TWO = 1.414214f;

    public static Vector3[] GetSquare(float _centerScale = 2f)
    {
      return Shape.GetPolygon(4, _centerScale);
    }

    public static Vector3[] GetPentagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(5, _centerScale);
    }

    public static Vector3[] GetHexagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(6, _centerScale);
    }

    public static Vector3[] GetHeptagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(7, _centerScale);
    }

    public static Vector3[] GetOctagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(8, _centerScale);
    }

    public static Vector3[] GetNonagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(9, _centerScale);
    }

    public static Vector3[] GetDecagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(10, _centerScale);
    }

    public static Vector3[] GetDodecagon(float _centerScale = 2f)
    {
      return Shape.GetPolygon(12, _centerScale);
    }

    public static Vector3[] GetPolygon(int _sides, float _centerScale = 2f)
    {
      return Shape.GetRoseCurve(_sides, 1, _centerScale, true);
    }

    public static Vector3[] GetStar(float _centerScale = 2f)
    {
      return Shape.GetRoseCurve(5, 2, _centerScale, true);
    }

    public static Vector3[] GetRoseCurve(
      int _points,
      int _detail,
      float _centerScale,
      bool _unitize)
    {
      _points = Mathf.Max(3, _points);
      _detail = Mathf.Max(1, _detail);
      Vector3[] _points1 = new Vector3[_points * _detail];
      int num1 = _points;
      Vector3 _min = Vector3.op_Multiply(Vector3.get_one(), float.MaxValue);
      Vector3 _max = Vector3.op_Multiply(Vector3.get_one(), float.MinValue);
      for (int index = 0; index < _points1.Length; ++index)
      {
        float num2 = (float) index * (6.283185f / (float) _points1.Length);
        float num3 = Mathf.Cos(num2 * (float) num1) + _centerScale;
        _points1[index] = new Vector3(num3 * Mathf.Cos(num2), num3 * Mathf.Sin(num2), 0.0f);
        _min = Vector3.Min(_min, _points1[index]);
        _max = Vector3.Max(_max, _points1[index]);
      }
      if (_unitize)
        Shape.Unitize(ref _points1, _min, _max);
      return _points1;
    }

    public static void Unitize(ref Vector3[] _points, Vector3 _min, Vector3 _max)
    {
      float num = Vector3.Distance(_min, _max) / 1.414214f;
      for (int index = 0; index < _points.Length; ++index)
      {
        ref Vector3 local = ref _points[index];
        local = Vector3.op_Division(local, num);
      }
    }
  }
}
