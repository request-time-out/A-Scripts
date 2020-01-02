// Decompiled with JetBrains decompiler
// Type: ReMotion.TweenSettingsExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ReMotion
{
  public static class TweenSettingsExtensions
  {
    public static Tween<TObject, float> UseFloatTween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, float> getter,
      TweenSetter<TObject, float> setter,
      EasingFunction easingFunction,
      float duration,
      float to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, float>) new TweenSettingsExtensions.FloatTween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, Vector3> UseVector3Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, Vector3> getter,
      TweenSetter<TObject, Vector3> setter,
      EasingFunction easingFunction,
      float duration,
      Vector3 to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, Vector3>) new TweenSettingsExtensions.Vector3Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, Vector2> UseVector2Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, Vector2> getter,
      TweenSetter<TObject, Vector2> setter,
      EasingFunction easingFunction,
      float duration,
      Vector2 to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, Vector2>) new TweenSettingsExtensions.Vector2Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, Vector4> UseVector4Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, Vector4> getter,
      TweenSetter<TObject, Vector4> setter,
      EasingFunction easingFunction,
      float duration,
      Vector4 to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, Vector4>) new TweenSettingsExtensions.Vector4Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, double> UseDoubleTween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, double> getter,
      TweenSetter<TObject, double> setter,
      EasingFunction easingFunction,
      float duration,
      double to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, double>) new TweenSettingsExtensions.DoubleTween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, int> UseInt32Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, int> getter,
      TweenSetter<TObject, int> setter,
      EasingFunction easingFunction,
      float duration,
      int to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, int>) new TweenSettingsExtensions.Int32Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, long> UseInt64Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, long> getter,
      TweenSetter<TObject, long> setter,
      EasingFunction easingFunction,
      float duration,
      long to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, long>) new TweenSettingsExtensions.Int64Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, uint> UseUInt32Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, uint> getter,
      TweenSetter<TObject, uint> setter,
      EasingFunction easingFunction,
      float duration,
      uint to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, uint>) new TweenSettingsExtensions.UInt32Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, ulong> UseUInt64Tween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, ulong> getter,
      TweenSetter<TObject, ulong> setter,
      EasingFunction easingFunction,
      float duration,
      ulong to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, ulong>) new TweenSettingsExtensions.UInt64Tween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    public static Tween<TObject, Color> UseColorTween<TObject>(
      this TweenSettings settings,
      TObject target,
      TweenGetter<TObject, Color> getter,
      TweenSetter<TObject, Color> setter,
      EasingFunction easingFunction,
      float duration,
      Color to,
      bool isRelative)
      where TObject : class
    {
      return (Tween<TObject, Color>) new TweenSettingsExtensions.ColorTween<TObject>(settings, target, getter, setter, easingFunction, duration, to, isRelative);
    }

    private class FloatTween<TObject> : Tween<TObject, float> where TObject : class
    {
      public FloatTween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, float> getter,
        TweenSetter<TObject, float> setter,
        EasingFunction easingFunction,
        float duration,
        float to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override float AddOperator(float left, float right)
      {
        return left + right;
      }

      protected override float GetDifference(float from, float to)
      {
        return to - from;
      }

      protected override void CreateValue(
        ref float from,
        ref float difference,
        ref float ratio,
        out float value)
      {
        value = from + difference * ratio;
      }
    }

    private class Vector3Tween<TObject> : Tween<TObject, Vector3> where TObject : class
    {
      public Vector3Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, Vector3> getter,
        TweenSetter<TObject, Vector3> setter,
        EasingFunction easingFunction,
        float duration,
        Vector3 to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override Vector3 AddOperator(Vector3 left, Vector3 right)
      {
        return Vector3.op_Addition(left, right);
      }

      protected override Vector3 GetDifference(Vector3 from, Vector3 to)
      {
        return new Vector3((float) (to.x - from.x), (float) (to.y - from.y), (float) (to.z - from.z));
      }

      protected override void CreateValue(
        ref Vector3 from,
        ref Vector3 difference,
        ref float ratio,
        out Vector3 value)
      {
        ((Vector3) ref value).\u002Ector((float) (from.x + difference.x * (double) ratio), (float) (from.y + difference.y * (double) ratio), (float) (from.z + difference.z * (double) ratio));
      }
    }

    private class Vector2Tween<TObject> : Tween<TObject, Vector2> where TObject : class
    {
      public Vector2Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, Vector2> getter,
        TweenSetter<TObject, Vector2> setter,
        EasingFunction easingFunction,
        float duration,
        Vector2 to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override Vector2 AddOperator(Vector2 left, Vector2 right)
      {
        return Vector2.op_Addition(left, right);
      }

      protected override Vector2 GetDifference(Vector2 from, Vector2 to)
      {
        return new Vector2((float) (to.x - from.x), (float) (to.y - from.y));
      }

      protected override void CreateValue(
        ref Vector2 from,
        ref Vector2 difference,
        ref float ratio,
        out Vector2 value)
      {
        ((Vector2) ref value).\u002Ector((float) (from.x + difference.x * (double) ratio), (float) (from.y + difference.y * (double) ratio));
      }
    }

    private class Vector4Tween<TObject> : Tween<TObject, Vector4> where TObject : class
    {
      public Vector4Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, Vector4> getter,
        TweenSetter<TObject, Vector4> setter,
        EasingFunction easingFunction,
        float duration,
        Vector4 to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override Vector4 AddOperator(Vector4 left, Vector4 right)
      {
        return Vector4.op_Addition(left, right);
      }

      protected override Vector4 GetDifference(Vector4 from, Vector4 to)
      {
        return new Vector4((float) (to.x - from.x), (float) (to.y - from.y), (float) (to.z - from.z), (float) (double) (to.w = (__Null) (float) from.w));
      }

      protected override void CreateValue(
        ref Vector4 from,
        ref Vector4 difference,
        ref float ratio,
        out Vector4 value)
      {
        ((Vector4) ref value).\u002Ector((float) (from.x + difference.x * (double) ratio), (float) (from.y + difference.y * (double) ratio), (float) (from.z + difference.z * (double) ratio), (float) (from.w + difference.w * (double) ratio));
      }
    }

    private class DoubleTween<TObject> : Tween<TObject, double> where TObject : class
    {
      public DoubleTween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, double> getter,
        TweenSetter<TObject, double> setter,
        EasingFunction easingFunction,
        float duration,
        double to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override double AddOperator(double left, double right)
      {
        return left + right;
      }

      protected override double GetDifference(double from, double to)
      {
        return to - from;
      }

      protected override void CreateValue(
        ref double from,
        ref double difference,
        ref float ratio,
        out double value)
      {
        value = from + difference * (double) ratio;
      }
    }

    private class Int32Tween<TObject> : Tween<TObject, int> where TObject : class
    {
      public Int32Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, int> getter,
        TweenSetter<TObject, int> setter,
        EasingFunction easingFunction,
        float duration,
        int to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override int AddOperator(int left, int right)
      {
        return left + right;
      }

      protected override int GetDifference(int from, int to)
      {
        return to - from;
      }

      protected override void CreateValue(
        ref int from,
        ref int difference,
        ref float ratio,
        out int value)
      {
        value = (int) ((double) from + (double) difference * (double) ratio);
      }
    }

    private class Int64Tween<TObject> : Tween<TObject, long> where TObject : class
    {
      public Int64Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, long> getter,
        TweenSetter<TObject, long> setter,
        EasingFunction easingFunction,
        float duration,
        long to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override long AddOperator(long left, long right)
      {
        return left + right;
      }

      protected override long GetDifference(long from, long to)
      {
        return to - from;
      }

      protected override void CreateValue(
        ref long from,
        ref long difference,
        ref float ratio,
        out long value)
      {
        value = (long) ((double) from + (double) difference * (double) ratio);
      }
    }

    private class UInt32Tween<TObject> : Tween<TObject, uint> where TObject : class
    {
      public UInt32Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, uint> getter,
        TweenSetter<TObject, uint> setter,
        EasingFunction easingFunction,
        float duration,
        uint to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override uint AddOperator(uint left, uint right)
      {
        return left + right;
      }

      protected override uint GetDifference(uint from, uint to)
      {
        return to - from;
      }

      protected override void CreateValue(
        ref uint from,
        ref uint difference,
        ref float ratio,
        out uint value)
      {
        value = (uint) ((double) from + (double) difference * (double) ratio);
      }
    }

    private class UInt64Tween<TObject> : Tween<TObject, ulong> where TObject : class
    {
      public UInt64Tween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, ulong> getter,
        TweenSetter<TObject, ulong> setter,
        EasingFunction easingFunction,
        float duration,
        ulong to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override ulong AddOperator(ulong left, ulong right)
      {
        return left + right;
      }

      protected override ulong GetDifference(ulong from, ulong to)
      {
        return to - from;
      }

      protected override void CreateValue(
        ref ulong from,
        ref ulong difference,
        ref float ratio,
        out ulong value)
      {
        value = (ulong) ((double) from + (double) difference * (double) ratio);
      }
    }

    private class ColorTween<TObject> : Tween<TObject, Color> where TObject : class
    {
      public ColorTween(
        TweenSettings settings,
        TObject target,
        TweenGetter<TObject, Color> getter,
        TweenSetter<TObject, Color> setter,
        EasingFunction easingFunction,
        float duration,
        Color to,
        bool isRelative)
        : base(settings, target, getter, setter, easingFunction, duration, to, isRelative)
      {
      }

      protected override Color AddOperator(Color left, Color right)
      {
        return Color.op_Addition(left, right);
      }

      protected override Color GetDifference(Color from, Color to)
      {
        return new Color((float) (to.a - from.a), (float) (to.r - from.r), (float) (to.g - from.g), (float) (to.b - from.b));
      }

      protected override void CreateValue(
        ref Color from,
        ref Color difference,
        ref float ratio,
        out Color value)
      {
        ((Color) ref value).\u002Ector((float) (from.a + difference.a * (double) ratio), (float) (from.r + difference.r * (double) ratio), (float) (from.g + difference.g * (double) ratio), (float) (from.b + difference.b * (double) ratio));
      }
    }
  }
}
