// Decompiled with JetBrains decompiler
// Type: ReMotion.ObservableEasing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;

namespace ReMotion
{
  public class ObservableEasing
  {
    private const float DefaultOvershoot = 1.70158f;
    private const float DefaultAmplitude = 1.70158f;
    private const float DefaultPeriod = 0.0f;

    public static IObservable<float> Create(
      EasingFunction easing,
      float duration,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(easing, duration, ignoreTimeScale);
    }

    public static IObservable<float> Linear(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.Linear, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInSine(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInSine, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutSine(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutSine, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutSine(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutSine, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInQuad(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInQuad, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutQuad(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutQuad, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutQuad(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutQuad, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInCubic(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInCubic, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutCubic(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutCubic, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutCubic(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutCubic, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInQuart(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInQuart, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutQuart(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutQuart, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutQuart(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutQuart, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInQuint(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInQuint, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutQuint(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutQuint, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutQuint(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutQuint, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInExpo(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInExpo, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutExpo(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutExpo, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutExpo(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutExpo, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInCirc(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInCirc, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutCirc(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutCirc, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutCirc(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutCirc, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInBack(
      float duration,
      float overshoot = 1.70158f,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInBack(overshoot), duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutBack(
      float duration,
      float overshoot = 1.70158f,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutBack(overshoot), duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutBack(
      float duration,
      float overshoot = 1.70158f,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutBack(overshoot), duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInElastic(
      float duration,
      float amplitude = 1.70158f,
      float period = 0.0f,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInElastic(amplitude, period), duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutElastic(
      float duration,
      float amplitude = 1.70158f,
      float period = 0.0f,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutElastic(amplitude, period), duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutElastic(
      float duration,
      float amplitude = 1.70158f,
      float period = 0.0f,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutElastic(amplitude, period), duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInBounce(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInBounce, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseOutBounce(float duration, bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseOutBounce, duration, ignoreTimeScale);
    }

    public static IObservable<float> EaseInOutBounce(
      float duration,
      bool ignoreTimeScale = false)
    {
      return ObservableEasing.ObservableTween.CreateObservable(EasingFunctions.EaseInOutBounce, duration, ignoreTimeScale);
    }

    private class ObservableTween : ITween
    {
      private readonly IObserver<float> observer;
      private readonly BooleanDisposable cancellation;
      private readonly EasingFunction easing;
      private readonly bool ignoreTimeScale;
      private readonly float duration;
      private float currentTime;

      private ObservableTween(
        IObserver<float> observer,
        BooleanDisposable cancellation,
        EasingFunction easing,
        float duration,
        bool ignoreTimeScale)
      {
        this.observer = observer;
        this.cancellation = cancellation;
        this.easing = easing;
        this.ignoreTimeScale = ignoreTimeScale;
        this.duration = duration;
      }

      public static IObservable<float> CreateObservable(
        EasingFunction easing,
        float duration,
        bool ignoreTimeScale)
      {
        return (IObservable<float>) Observable.CreateSafe<float>((Func<IObserver<M0>, IDisposable>) (observer =>
        {
          observer.OnNext(0.0f);
          BooleanDisposable cancellation = new BooleanDisposable();
          TweenEngine.AddTween((ITween) new ObservableEasing.ObservableTween(observer, cancellation, easing, duration, ignoreTimeScale));
          return (IDisposable) cancellation;
        }), false);
      }

      public bool MoveNext(ref float deltaTime, ref float unscaledDeltaTime)
      {
        if (this.cancellation.get_IsDisposed())
          return false;
        float num = !this.ignoreTimeScale ? deltaTime : unscaledDeltaTime;
        if ((double) num == 0.0)
          return true;
        this.currentTime += num;
        bool flag = false;
        if ((double) this.currentTime >= (double) this.duration)
        {
          this.currentTime = this.duration;
          flag = true;
        }
        this.observer.OnNext(this.easing(this.currentTime, this.duration));
        if (!flag)
          return true;
        this.observer.OnCompleted();
        return false;
      }
    }
  }
}
