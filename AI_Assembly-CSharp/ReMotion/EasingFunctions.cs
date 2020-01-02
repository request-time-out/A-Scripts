// Decompiled with JetBrains decompiler
// Type: ReMotion.EasingFunctions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ReMotion
{
  public static class EasingFunctions
  {
    private const float DefaultOvershoot = 1.70158f;
    private const float DefaultAmplitude = 1.70158f;
    private const float DefaultPeriod = 0.0f;
    private const float PiDivide2 = 1.570796f;
    private const float PiMultiply2 = 6.283185f;
    public static readonly EasingFunction Linear;
    public static readonly EasingFunction EaseInSine;
    public static readonly EasingFunction EaseOutSine;
    public static readonly EasingFunction EaseInOutSine;
    public static readonly EasingFunction EaseInQuad;
    public static readonly EasingFunction EaseOutQuad;
    public static readonly EasingFunction EaseInOutQuad;
    public static readonly EasingFunction EaseInCubic;
    public static readonly EasingFunction EaseOutCubic;
    public static readonly EasingFunction EaseInOutCubic;
    public static readonly EasingFunction EaseInQuart;
    public static readonly EasingFunction EaseOutQuart;
    public static readonly EasingFunction EaseInOutQuart;
    public static readonly EasingFunction EaseInQuint;
    public static readonly EasingFunction EaseOutQuint;
    public static readonly EasingFunction EaseInOutQuint;
    public static readonly EasingFunction EaseInExpo;
    public static readonly EasingFunction EaseOutExpo;
    public static readonly EasingFunction EaseInOutExpo;
    public static readonly EasingFunction EaseInCirc;
    public static readonly EasingFunction EaseOutCirc;
    public static readonly EasingFunction EaseInOutCirc;
    public static readonly EasingFunction EaseInBounce;
    public static readonly EasingFunction EaseOutBounce;
    public static readonly EasingFunction EaseInOutBounce;
    private static readonly EasingFunction defaultEaseInBack;
    private static readonly EasingFunction defaultEaseOutBack;
    private static readonly EasingFunction defaultEaseInOutBack;
    private static readonly EasingFunction defaultEaseInElastic;
    private static readonly EasingFunction defaultEaseOutElastic;
    private static readonly EasingFunction defaultEaseInOutElastic;

    public static EasingFunction EaseInBack(float overshoot = 1.70158f)
    {
      return (double) overshoot == 1.70158004760742 ? EasingFunctions.defaultEaseInBack : (EasingFunction) ((time, duration) => EasingFunctions.EaseInBack_(time, duration, overshoot));
    }

    public static EasingFunction EaseOutBack(float overshoot = 1.70158f)
    {
      return (double) overshoot == 1.70158004760742 ? EasingFunctions.defaultEaseOutBack : (EasingFunction) ((time, duration) => EasingFunctions.EaseOutBack_(time, duration, overshoot));
    }

    public static EasingFunction EaseInOutBack(float overshoot = 1.70158f)
    {
      return (double) overshoot == 1.70158004760742 ? EasingFunctions.defaultEaseInOutBack : (EasingFunction) ((time, duration) => EasingFunctions.EaseInOutBack_(time, duration, overshoot));
    }

    public static EasingFunction EaseInElastic(float amplitude = 1.70158f, float period = 0.0f)
    {
      return (double) amplitude == 1.70158004760742 && (double) period == 0.0 ? EasingFunctions.defaultEaseInElastic : (EasingFunction) ((time, duration) => EasingFunctions.EaseInElastic_(time, duration, amplitude, period));
    }

    public static EasingFunction EaseOutElastic(float amplitude = 1.70158f, float period = 0.0f)
    {
      return (double) amplitude == 1.70158004760742 && (double) period == 0.0 ? EasingFunctions.defaultEaseOutElastic : (EasingFunction) ((time, duration) => EasingFunctions.EaseOutElastic_(time, duration, amplitude, period));
    }

    public static EasingFunction EaseInOutElastic(float amplitude = 1.70158f, float period = 0.0f)
    {
      return (double) amplitude == 1.70158004760742 && (double) period == 0.0 ? EasingFunctions.defaultEaseInOutElastic : (EasingFunction) ((time, duration) => EasingFunctions.EaseInOutElastic_(time, duration, amplitude, period));
    }

    public static EasingFunction Shake(float amplitude = 1f)
    {
      return (EasingFunction) ((time, duration) => Random.Range(0.0f, amplitude));
    }

    public static EasingFunction AnimationCurve(AnimationCurve animationCurve)
    {
      if (animationCurve.get_keys().Length == 0)
        return EasingFunctions.Linear;
      float curveDuration = ((Keyframe) ref animationCurve.get_keys()[animationCurve.get_keys().Length - 1]).get_time();
      return (EasingFunction) ((time, duration) => animationCurve.Evaluate(time * curveDuration / duration));
    }

    private static float Linear_(float time, float duration)
    {
      return time / duration;
    }

    private static float EaseInSine_(float time, float duration)
    {
      return (float) (-1.0 * Math.Cos((double) time / (double) duration * 1.57079637050629) + 1.0);
    }

    private static float EaseOutSine_(float time, float duration)
    {
      return (float) Math.Sin((double) time / (double) duration * 1.57079637050629);
    }

    private static float EaseInOutSine_(float time, float duration)
    {
      return (float) (-0.5 * (Math.Cos(3.14159274101257 * (double) time / (double) duration) - 1.0));
    }

    private static float EaseInQuad_(float time, float duration)
    {
      time /= duration;
      return time * time;
    }

    private static float EaseOutQuad_(float time, float duration)
    {
      time /= duration;
      return (float) (-(double) time * ((double) time - 2.0));
    }

    private static float EaseInOutQuad_(float time, float duration)
    {
      time /= duration * 0.5f;
      if ((double) time < 1.0)
        return 0.5f * time * time;
      --time;
      return (float) (-0.5 * ((double) time * ((double) time - 2.0) - 1.0));
    }

    private static float EaseInCubic_(float time, float duration)
    {
      time /= duration;
      return time * time * time;
    }

    private static float EaseOutCubic_(float time, float duration)
    {
      time = (float) ((double) time / (double) duration - 1.0);
      return (float) ((double) time * (double) time * (double) time + 1.0);
    }

    private static float EaseInOutCubic_(float time, float duration)
    {
      time /= duration * 0.5f;
      if ((double) time < 1.0)
        return 0.5f * time * time * time;
      time -= 2f;
      return (float) (0.5 * ((double) time * (double) time * (double) time + 2.0));
    }

    private static float EaseInQuart_(float time, float duration)
    {
      time /= duration;
      return time * time * time * time;
    }

    private static float EaseOutQuart_(float time, float duration)
    {
      time = (float) ((double) time / (double) duration - 1.0);
      return (float) -((double) time * (double) time * (double) time * (double) time - 1.0);
    }

    private static float EaseInOutQuart_(float time, float duration)
    {
      time /= duration * 0.5f;
      if ((double) time < 1.0)
        return 0.5f * time * time * time * time;
      time -= 2f;
      return (float) (-0.5 * ((double) time * (double) time * (double) time * (double) time - 2.0));
    }

    private static float EaseInQuint_(float time, float duration)
    {
      time /= duration;
      return time * time * time * time * time;
    }

    private static float EaseOutQuint_(float time, float duration)
    {
      time = (float) ((double) time / (double) duration - 1.0);
      return (float) ((double) time * (double) time * (double) time * (double) time * (double) time + 1.0);
    }

    private static float EaseInOutQuint_(float time, float duration)
    {
      time /= duration * 0.5f;
      if ((double) time < 1.0)
        return 0.5f * time * time * time * time * time;
      time -= 2f;
      return (float) (0.5 * ((double) time * (double) time * (double) time * (double) time * (double) time + 2.0));
    }

    private static float EaseInExpo_(float time, float duration)
    {
      return (double) time == 0.0 ? 0.0f : (float) Math.Pow(2.0, 10.0 * ((double) time / (double) duration - 1.0));
    }

    private static float EaseOutExpo_(float time, float duration)
    {
      return (double) time == (double) duration ? 1f : (float) (-Math.Pow(2.0, -10.0 * (double) time / (double) duration) + 1.0);
    }

    private static float EaseInOutExpo_(float time, float duration)
    {
      if ((double) time == 0.0)
        return 0.0f;
      if ((double) time == (double) duration)
        return 1f;
      time /= duration * 0.5f;
      return (double) time < 1.0 ? 0.5f * (float) Math.Pow(2.0, 10.0 * ((double) time - 1.0)) : (float) (0.5 * (-Math.Pow(2.0, -10.0 * (double) --time) + 2.0));
    }

    private static float EaseInCirc_(float time, float duration)
    {
      time /= duration;
      return (float) -(Math.Sqrt(1.0 - (double) time * (double) time) - 1.0);
    }

    private static float EaseOutCirc_(float time, float duration)
    {
      time = (float) ((double) time / (double) duration - 1.0);
      return (float) Math.Sqrt(1.0 - (double) time * (double) time);
    }

    private static float EaseInOutCirc_(float time, float duration)
    {
      time /= duration * 0.5f;
      if ((double) time < 1.0)
        return (float) (-0.5 * (Math.Sqrt(1.0 - (double) time * (double) time) - 1.0));
      time -= 2f;
      return (float) (0.5 * (Math.Sqrt(1.0 - (double) time * (double) time) + 1.0));
    }

    private static float EaseInBack_(float time, float duration, float overshoot)
    {
      time /= duration;
      return (float) ((double) time * (double) time * (((double) overshoot + 1.0) * (double) time - (double) overshoot));
    }

    private static float EaseOutBack_(float time, float duration, float overshoot)
    {
      time = (float) ((double) time / (double) duration - 1.0);
      return (float) ((double) time * (double) time * (((double) overshoot + 1.0) * (double) time + (double) overshoot) + 1.0);
    }

    private static float EaseInOutBack_(float time, float duration, float overshoot)
    {
      time /= duration * 0.5f;
      if ((double) time < 1.0)
        return (float) (0.5 * ((double) time * (double) time * (((double) (overshoot *= 1.525f) + 1.0) * (double) time - (double) overshoot)));
      time -= 2f;
      return (float) (0.5 * ((double) time * (double) time * (((double) (overshoot *= 1.525f) + 1.0) * (double) time + (double) overshoot) + 2.0));
    }

    private static float EaseInElastic_(float time, float duration, float amplitude, float period)
    {
      if ((double) time == 0.0)
        return 0.0f;
      time /= duration;
      if ((double) time == 1.0)
        return 1f;
      if ((double) period == 0.0)
        period = duration * 0.3f;
      float num;
      if ((double) amplitude < 1.0)
      {
        amplitude = 1f;
        num = period / 4f;
      }
      else
        num = period / 6.283185f * (float) Math.Asin(1.0 / (double) amplitude);
      --time;
      return (float) -((double) amplitude * Math.Pow(2.0, 10.0 * (double) time) * Math.Sin(((double) time * (double) duration - (double) num) * 6.28318548202515 / (double) period));
    }

    private static float EaseOutElastic_(
      float time,
      float duration,
      float amplitude,
      float period)
    {
      if ((double) time == 0.0)
        return 0.0f;
      time /= duration;
      if ((double) time == 1.0)
        return 1f;
      if ((double) period == 0.0)
        period = duration * 0.3f;
      float num;
      if ((double) amplitude < 1.0)
      {
        amplitude = 1f;
        num = period / 4f;
      }
      else
        num = period / 6.283185f * (float) Math.Asin(1.0 / (double) amplitude);
      return (float) ((double) amplitude * Math.Pow(2.0, -10.0 * (double) time) * Math.Sin(((double) time * (double) duration - (double) num) * 6.28318548202515 / (double) period) + 1.0);
    }

    private static float EaseInOutElastic_(
      float time,
      float duration,
      float amplitude,
      float period)
    {
      if ((double) time == 0.0)
        return 0.0f;
      time /= duration * 0.5f;
      if ((double) time == 2.0)
        return 1f;
      if ((double) period == 0.0)
        period = duration * 0.45f;
      float num;
      if ((double) amplitude < 1.0)
      {
        amplitude = 1f;
        num = period / 4f;
      }
      else
        num = period / 6.283185f * (float) Math.Asin(1.0 / (double) amplitude);
      if ((double) time < 1.0)
      {
        --time;
        return (float) (-0.5 * ((double) amplitude * Math.Pow(2.0, 10.0 * (double) time) * Math.Sin(((double) time * (double) duration - (double) num) * 6.28318548202515 / (double) period)));
      }
      --time;
      return (float) ((double) amplitude * Math.Pow(2.0, -10.0 * (double) time) * Math.Sin(((double) time * (double) duration - (double) num) * 6.28318548202515 / (double) period) * 0.5 + 1.0);
    }

    private static float EaseInBounce_(float time, float duration)
    {
      return 1f - EasingFunctions.EaseOutBounce_(duration - time, duration);
    }

    private static float EaseOutBounce_(float time, float duration)
    {
      time /= duration;
      if ((double) time < 0.363636374473572)
        return 121f / 16f * time * time;
      if ((double) time < 0.727272748947144)
      {
        time -= 0.5454546f;
        return (float) (121.0 / 16.0 * (double) time * (double) time + 0.75);
      }
      if ((double) time < 0.909090936183929)
      {
        time -= 0.8181818f;
        return (float) (121.0 / 16.0 * (double) time * (double) time + 15.0 / 16.0);
      }
      time -= 0.9545454f;
      return (float) (121.0 / 16.0 * (double) time * (double) time + 63.0 / 64.0);
    }

    private static float EaseInOutBounce_(float time, float duration)
    {
      return (double) time < (double) duration * 0.5 ? EasingFunctions.EaseInBounce_(time * 2f, duration) * 0.5f : (float) ((double) EasingFunctions.EaseOutBounce_(time * 2f - duration, duration) * 0.5 + 0.5);
    }

    static EasingFunctions()
    {
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache0 = new EasingFunction(EasingFunctions.Linear_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.Linear = EasingFunctions.\u003C\u003Ef__mg\u0024cache0;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache1 = new EasingFunction(EasingFunctions.EaseInSine_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInSine = EasingFunctions.\u003C\u003Ef__mg\u0024cache1;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache2 = new EasingFunction(EasingFunctions.EaseOutSine_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutSine = EasingFunctions.\u003C\u003Ef__mg\u0024cache2;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache3 = new EasingFunction(EasingFunctions.EaseInOutSine_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutSine = EasingFunctions.\u003C\u003Ef__mg\u0024cache3;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache4 = new EasingFunction(EasingFunctions.EaseInQuad_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInQuad = EasingFunctions.\u003C\u003Ef__mg\u0024cache4;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache5 = new EasingFunction(EasingFunctions.EaseOutQuad_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutQuad = EasingFunctions.\u003C\u003Ef__mg\u0024cache5;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache6 = new EasingFunction(EasingFunctions.EaseInOutQuad_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutQuad = EasingFunctions.\u003C\u003Ef__mg\u0024cache6;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache7 = new EasingFunction(EasingFunctions.EaseInCubic_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInCubic = EasingFunctions.\u003C\u003Ef__mg\u0024cache7;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache8 = new EasingFunction(EasingFunctions.EaseOutCubic_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutCubic = EasingFunctions.\u003C\u003Ef__mg\u0024cache8;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache9 = new EasingFunction(EasingFunctions.EaseInOutCubic_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutCubic = EasingFunctions.\u003C\u003Ef__mg\u0024cache9;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cacheA == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cacheA = new EasingFunction(EasingFunctions.EaseInQuart_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInQuart = EasingFunctions.\u003C\u003Ef__mg\u0024cacheA;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cacheB == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cacheB = new EasingFunction(EasingFunctions.EaseOutQuart_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutQuart = EasingFunctions.\u003C\u003Ef__mg\u0024cacheB;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cacheC == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cacheC = new EasingFunction(EasingFunctions.EaseInOutQuart_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutQuart = EasingFunctions.\u003C\u003Ef__mg\u0024cacheC;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cacheD == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cacheD = new EasingFunction(EasingFunctions.EaseInQuint_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInQuint = EasingFunctions.\u003C\u003Ef__mg\u0024cacheD;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cacheE == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cacheE = new EasingFunction(EasingFunctions.EaseOutQuint_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutQuint = EasingFunctions.\u003C\u003Ef__mg\u0024cacheE;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cacheF == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cacheF = new EasingFunction(EasingFunctions.EaseInOutQuint_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutQuint = EasingFunctions.\u003C\u003Ef__mg\u0024cacheF;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache10 = new EasingFunction(EasingFunctions.EaseInExpo_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInExpo = EasingFunctions.\u003C\u003Ef__mg\u0024cache10;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache11 = new EasingFunction(EasingFunctions.EaseOutExpo_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutExpo = EasingFunctions.\u003C\u003Ef__mg\u0024cache11;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache12 = new EasingFunction(EasingFunctions.EaseInOutExpo_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutExpo = EasingFunctions.\u003C\u003Ef__mg\u0024cache12;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache13 = new EasingFunction(EasingFunctions.EaseInCirc_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInCirc = EasingFunctions.\u003C\u003Ef__mg\u0024cache13;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache14 = new EasingFunction(EasingFunctions.EaseOutCirc_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutCirc = EasingFunctions.\u003C\u003Ef__mg\u0024cache14;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache15 = new EasingFunction(EasingFunctions.EaseInOutCirc_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutCirc = EasingFunctions.\u003C\u003Ef__mg\u0024cache15;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache16 = new EasingFunction(EasingFunctions.EaseInBounce_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInBounce = EasingFunctions.\u003C\u003Ef__mg\u0024cache16;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache17 = new EasingFunction(EasingFunctions.EaseOutBounce_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseOutBounce = EasingFunctions.\u003C\u003Ef__mg\u0024cache17;
      // ISSUE: reference to a compiler-generated field
      if (EasingFunctions.\u003C\u003Ef__mg\u0024cache18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EasingFunctions.\u003C\u003Ef__mg\u0024cache18 = new EasingFunction(EasingFunctions.EaseInOutBounce_);
      }
      // ISSUE: reference to a compiler-generated field
      EasingFunctions.EaseInOutBounce = EasingFunctions.\u003C\u003Ef__mg\u0024cache18;
      EasingFunctions.defaultEaseInBack = (EasingFunction) ((time, duration) => EasingFunctions.EaseInBack_(time, duration, 1.70158f));
      EasingFunctions.defaultEaseOutBack = (EasingFunction) ((time, duration) => EasingFunctions.EaseOutBack_(time, duration, 1.70158f));
      EasingFunctions.defaultEaseInOutBack = (EasingFunction) ((time, duration) => EasingFunctions.EaseInOutBack_(time, duration, 1.70158f));
      EasingFunctions.defaultEaseInElastic = (EasingFunction) ((time, duration) => EasingFunctions.EaseInElastic_(time, duration, 1.70158f, 0.0f));
      EasingFunctions.defaultEaseOutElastic = (EasingFunction) ((time, duration) => EasingFunctions.EaseOutElastic_(time, duration, 1.70158f, 0.0f));
      EasingFunctions.defaultEaseInOutElastic = (EasingFunction) ((time, duration) => EasingFunctions.EaseInOutElastic_(time, duration, 1.70158f, 0.0f));
    }
  }
}
