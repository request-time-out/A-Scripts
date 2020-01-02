// Decompiled with JetBrains decompiler
// Type: MathfEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public static class MathfEx
{
  public static float LerpAccel(float from, float to, float t)
  {
    return Mathf.Lerp(from, to, Mathf.Sqrt(t));
  }

  public static bool IsRange<T>(T min, T n, T max, bool isEqual) where T : IComparable
  {
    return isEqual ? MathfEx.RangeEqualOn<T>(min, n, max) : MathfEx.RangeEqualOff<T>(min, n, max);
  }

  public static bool RangeEqualOn<T>(T min, T n, T max) where T : IComparable
  {
    return n.CompareTo((object) max) <= 0 && n.CompareTo((object) min) >= 0;
  }

  public static bool RangeEqualOff<T>(T min, T n, T max) where T : IComparable
  {
    return n.CompareTo((object) max) < 0 && n.CompareTo((object) min) > 0;
  }

  public static float LerpBrake(float from, float to, float t)
  {
    return Mathf.Lerp(from, to, t * (2f - t));
  }

  public static int LoopValue(ref int value, int start, int end)
  {
    if (value > end)
      value = start;
    else if (value < start)
      value = end;
    return value;
  }

  public static int LoopValue(int value, int start, int end)
  {
    return MathfEx.LoopValue(ref value, start, end);
  }

  public static Rect AspectRect(float baseH = 1280f, float rate = 720f)
  {
    return new Rect(0.0f, (float) (((double) Screen.get_height() - (double) Screen.get_width() / (double) baseH * (double) rate) * 0.5) / (float) Screen.get_height(), 1f, rate * (float) Screen.get_width() / baseH / (float) Screen.get_height());
  }

  public static long Min(long _a, long _b)
  {
    return _a > _b ? _b : _a;
  }

  public static long Max(long _a, long _b)
  {
    return _a > _b ? _a : _b;
  }

  public static long Clamp(long _value, long _min, long _max)
  {
    return MathfEx.Min(MathfEx.Max(_value, _min), _max);
  }

  public static float ToRadian(float degree)
  {
    return degree * ((float) Math.PI / 180f);
  }

  public static float ToDegree(float radian)
  {
    return radian * 57.29578f;
  }

  public static Vector3 GetShapeLerpPositionValue(float shape, Vector3 min, Vector3 max)
  {
    return (double) shape >= 0.5 ? Vector3.Lerp(Vector3.get_zero(), max, Mathf.InverseLerp(0.5f, 1f, shape)) : Vector3.Lerp(min, Vector3.get_zero(), Mathf.InverseLerp(0.0f, 0.5f, shape));
  }

  public static Vector3 GetShapeLerpAngleValue(float shape, Vector3 min, Vector3 max)
  {
    Vector3 zero = Vector3.get_zero();
    if ((double) shape >= 0.5)
    {
      float num = Mathf.InverseLerp(0.5f, 1f, shape);
      for (int index = 0; index < 3; ++index)
        ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(0.0f, ((Vector3) ref max).get_Item(index), num));
    }
    else
    {
      float num = Mathf.InverseLerp(0.0f, 0.5f, shape);
      for (int index = 0; index < 3; ++index)
        ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(((Vector3) ref min).get_Item(index), 0.0f, num));
    }
    return zero;
  }
}
