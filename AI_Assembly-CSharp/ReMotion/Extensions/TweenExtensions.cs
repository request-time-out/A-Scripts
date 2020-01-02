// Decompiled with JetBrains decompiler
// Type: ReMotion.Extensions.TweenExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ReMotion.Extensions
{
  public static class TweenExtensions
  {
    public static Tween<Transform, Vector3> TweenPosition(
      this Transform transform,
      Vector3 to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Transform, Vector3> tween = settings.UseVector3Tween<Transform>(transform, (TweenGetter<Transform, Vector3>) (x => x.get_position()), (TweenSetter<Transform, Vector3>) ((Transform target, ref Vector3 value) => target.set_position(value)), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenPositionAsync(
      this Transform transform,
      Vector3 to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return transform.TweenPosition(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }

    public static Tween<Transform, Vector2> TweenPositionXY(
      this Transform transform,
      Vector2 to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Transform, Vector2> tween = settings.UseVector2Tween<Transform>(transform, (TweenGetter<Transform, Vector2>) (x => Vector2.op_Implicit(x.get_position())), (TweenSetter<Transform, Vector2>) ((Transform target, ref Vector2 value) =>
      {
        Vector3 position = target.get_position();
        Transform transform1 = target;
        Vector3 vector3_1 = (Vector3) null;
        vector3_1.x = value.x;
        vector3_1.y = value.y;
        vector3_1.z = position.z;
        Vector3 vector3_2 = vector3_1;
        transform1.set_position(vector3_2);
      }), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenPositionXYAsync(
      this Transform transform,
      Vector2 to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return transform.TweenPositionXY(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }

    public static Tween<Transform, float> TweenPositionX(
      this Transform transform,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Transform, float> tween = settings.UseFloatTween<Transform>(transform, (TweenGetter<Transform, float>) (x => (float) x.get_position().x), (TweenSetter<Transform, float>) ((Transform target, ref float value) =>
      {
        Vector3 position = target.get_position();
        Transform transform1 = target;
        Vector3 vector3_1 = (Vector3) null;
        vector3_1.x = (__Null) (double) value;
        vector3_1.y = position.y;
        vector3_1.z = position.z;
        Vector3 vector3_2 = vector3_1;
        transform1.set_position(vector3_2);
      }), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenPositionXAsync(
      this Transform transform,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return transform.TweenPositionX(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }

    public static Tween<Transform, float> TweenPositionY(
      this Transform transform,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Transform, float> tween = settings.UseFloatTween<Transform>(transform, (TweenGetter<Transform, float>) (x => (float) x.get_position().y), (TweenSetter<Transform, float>) ((Transform target, ref float value) =>
      {
        Vector3 position = target.get_position();
        Transform transform1 = target;
        Vector3 vector3_1 = (Vector3) null;
        vector3_1.x = position.x;
        vector3_1.y = (__Null) (double) value;
        vector3_1.z = position.z;
        Vector3 vector3_2 = vector3_1;
        transform1.set_position(vector3_2);
      }), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenPositionYAsync(
      this Transform transform,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return transform.TweenPositionY(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }

    public static Tween<Transform, float> TweenPositionZ(
      this Transform transform,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Transform, float> tween = settings.UseFloatTween<Transform>(transform, (TweenGetter<Transform, float>) (x => (float) x.get_position().z), (TweenSetter<Transform, float>) ((Transform target, ref float value) =>
      {
        Vector3 position = target.get_position();
        Transform transform1 = target;
        Vector3 vector3_1 = (Vector3) null;
        vector3_1.x = position.x;
        vector3_1.y = position.y;
        vector3_1.z = (__Null) (double) value;
        Vector3 vector3_2 = vector3_1;
        transform1.set_position(vector3_2);
      }), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenPositionZAsync(
      this Transform transform,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return transform.TweenPositionZ(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }

    public static Tween<Graphic, float> TweenAlpha(
      this Graphic graphic,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Graphic, float> tween = settings.UseFloatTween<Graphic>(graphic, (TweenGetter<Graphic, float>) (x => (float) x.get_color().a), (TweenSetter<Graphic, float>) ((Graphic target, ref float value) =>
      {
        Color color1 = target.get_color();
        Graphic graphic1 = target;
        Color color2 = (Color) null;
        color2.r = color1.r;
        color2.g = color1.g;
        color2.b = color1.b;
        color2.a = (__Null) (double) value;
        Color color3 = color2;
        graphic1.set_color(color3);
      }), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenAlphaAsync(
      this Graphic graphic,
      float to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return graphic.TweenAlpha(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }

    public static Tween<Graphic, Color> TweenColor(
      this Graphic graphic,
      Color to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false,
      bool autoStart = true)
    {
      settings = settings ?? TweenSettings.Default;
      easing = easing ?? settings.DefaultEasing;
      Tween<Graphic, Color> tween = settings.UseColorTween<Graphic>(graphic, (TweenGetter<Graphic, Color>) (x => x.get_color()), (TweenSetter<Graphic, Color>) ((Graphic target, ref Color value) => target.set_color(value)), easing, duration, to, isRelativeTo);
      if (autoStart)
        tween.Start();
      return tween;
    }

    public static IObservable<Unit> TweenColorAsync(
      this Graphic graphic,
      Color to,
      float duration,
      EasingFunction easing = null,
      TweenSettings settings = null,
      bool isRelativeTo = false)
    {
      return graphic.TweenColor(to, duration, easing, settings, isRelativeTo, false).ToObservable(true);
    }
  }
}
