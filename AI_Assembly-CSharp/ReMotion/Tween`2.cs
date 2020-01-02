// Decompiled with JetBrains decompiler
// Type: ReMotion.Tween`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ReMotion
{
  public abstract class Tween<TObject, TProperty> : ITween where TObject : class
  {
    private readonly TObject target;
    private readonly TweenGetter<TObject, TProperty> getter;
    private readonly TweenSetter<TObject, TProperty> setter;
    private readonly EasingFunction easingFunction;
    private readonly float duration;
    private readonly bool isRelativeTo;
    private TProperty from;
    private TProperty to;
    private TProperty difference;
    private TProperty originalFrom;
    private TProperty originalTo;
    private Subject<Unit> completedEvent;
    private float delayTime;
    private float currentTime;
    private int repeatCount;

    public Tween(
      TweenSettings settings,
      TObject target,
      TweenGetter<TObject, TProperty> getter,
      TweenSetter<TObject, TProperty> setter,
      EasingFunction easingFunction,
      float duration,
      TProperty to,
      bool isRelativeTo)
    {
      this.Settings = settings;
      this.target = target;
      this.getter = getter;
      this.setter = setter;
      this.duration = duration;
      this.easingFunction = easingFunction;
      this.originalTo = to;
      this.isRelativeTo = isRelativeTo;
    }

    public TweenSettings Settings { get; private set; }

    public TweenStatus Status { get; private set; }

    public void Reset()
    {
      this.from = this.originalFrom;
      this.to = !this.isRelativeTo ? this.originalTo : this.AddOperator(this.from, this.originalTo);
      this.difference = this.GetDifference(this.from, this.to);
      this.currentTime = 0.0f;
      this.repeatCount = 0;
    }

    public Tween<TObject, TProperty> Start()
    {
      this.originalFrom = this.getter(this.target);
      this.delayTime = 0.0f;
      this.StartCore();
      return this;
    }

    public Tween<TObject, TProperty> Start(TProperty from, float delay)
    {
      this.originalFrom = from;
      if ((double) delay <= 0.0)
        delay = 0.0f;
      this.delayTime = delay;
      this.StartCore();
      return this;
    }

    public Tween<TObject, TProperty> Start(TProperty from, float delay, bool isRelativeFrom)
    {
      this.originalFrom = !isRelativeFrom ? from : this.AddOperator(this.getter(this.target), from);
      if ((double) delay <= 0.0)
        delay = 0.0f;
      this.delayTime = delay;
      this.StartCore();
      return this;
    }

    public Tween<TObject, TProperty> StartFrom(TProperty from)
    {
      this.originalFrom = from;
      this.delayTime = 0.0f;
      this.StartCore();
      return this;
    }

    public Tween<TObject, TProperty> StartFromRelative(TProperty from)
    {
      this.originalFrom = this.AddOperator(this.getter(this.target), from);
      this.delayTime = 0.0f;
      this.StartCore();
      return this;
    }

    public Tween<TObject, TProperty> StartAfter(float delay)
    {
      this.originalFrom = this.getter(this.target);
      if ((double) delay <= 0.0)
        delay = 0.0f;
      this.delayTime = delay;
      this.StartCore();
      return this;
    }

    private void StartCore()
    {
      this.Reset();
      switch (this.Status)
      {
        case TweenStatus.Stopped:
          this.Status = TweenStatus.Running;
          TweenEngine.Instance.Add((ITween) this);
          break;
        default:
          this.Status = TweenStatus.Running;
          break;
      }
    }

    public void Stop()
    {
      switch (this.Status)
      {
        case TweenStatus.Stopped:
          break;
        default:
          this.Status = TweenStatus.WaitingToStop;
          break;
      }
    }

    public void Pause()
    {
      switch (this.Status)
      {
        case TweenStatus.Running:
        case TweenStatus.WaitingToStop:
          this.Status = TweenStatus.Pausing;
          break;
      }
    }

    public void Resume()
    {
      switch (this.Status)
      {
        case TweenStatus.Pausing:
          this.Status = TweenStatus.Running;
          break;
      }
    }

    public void PauseOrResume()
    {
      switch (this.Status)
      {
        case TweenStatus.Running:
          this.Status = TweenStatus.Pausing;
          break;
        case TweenStatus.Pausing:
          this.Status = TweenStatus.Running;
          break;
      }
    }

    public IObservable<Unit> ToObservable(bool stopWhenDisposed = true)
    {
      if (this.completedEvent == null)
        this.completedEvent = new Subject<Unit>();
      if (this.Status != TweenStatus.Running)
        return (IObservable<Unit>) Observable.Defer<Unit>((Func<IObservable<M0>>) (() =>
        {
          if (this.Status == TweenStatus.Stopped)
            this.Start();
          IObservable<Unit> observable = (IObservable<Unit>) Observable.FirstOrDefault<Unit>((IObservable<M0>) this.completedEvent);
          return stopWhenDisposed ? (IObservable<Unit>) Observable.DoOnCancel<Unit>((IObservable<M0>) observable, (Action) (() => this.Stop())) : observable;
        }));
      IObservable<Unit> observable1 = (IObservable<Unit>) Observable.FirstOrDefault<Unit>((IObservable<M0>) this.completedEvent);
      return stopWhenDisposed ? (IObservable<Unit>) Observable.DoOnCancel<Unit>((IObservable<M0>) observable1, (Action) (() => this.Stop())) : observable1;
    }

    public void AttachSafe(GameObject gameObject)
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable(gameObject), (Action<M0>) (_ => this.Stop()));
    }

    public void AttachSafe(Component component)
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable(component), (Action<M0>) (_ => this.Stop()));
    }

    protected abstract TProperty AddOperator(TProperty left, TProperty right);

    protected abstract TProperty GetDifference(TProperty from, TProperty to);

    protected abstract void CreateValue(
      ref TProperty from,
      ref TProperty difference,
      ref float ratio,
      out TProperty value);

    public bool MoveNext(ref float deltaTime, ref float unscaledDeltaTime)
    {
      switch (this.Status)
      {
        case TweenStatus.Running:
          if ((double) this.delayTime != 0.0)
          {
            this.delayTime -= !this.Settings.IsIgnoreTimeScale ? deltaTime : unscaledDeltaTime;
            if ((double) this.delayTime > 0.0)
              return true;
            this.delayTime = 0.0f;
          }
          float time = !this.Settings.IsIgnoreTimeScale ? (this.currentTime += deltaTime) : (this.currentTime += unscaledDeltaTime);
          bool flag = false;
          if ((double) time >= (double) this.duration)
          {
            time = this.duration;
            flag = true;
          }
          float ratio = this.easingFunction(time, this.duration);
          TProperty newValue;
          this.CreateValue(ref this.from, ref this.difference, ref ratio, out newValue);
          this.setter(this.target, ref newValue);
          if (flag)
          {
            ++this.repeatCount;
            switch (this.Settings.LoopType)
            {
              case LoopType.Restart:
                this.from = this.originalFrom;
                this.currentTime = 0.0f;
                break;
              case LoopType.Cycle:
                TProperty from = this.from;
                this.from = this.to;
                this.to = from;
                this.difference = this.GetDifference(this.from, this.to);
                this.currentTime = 0.0f;
                break;
              case LoopType.CycleOnce:
                if (this.repeatCount == 2)
                  return false;
                goto case LoopType.Cycle;
              default:
                if (this.completedEvent != null)
                  this.completedEvent.OnNext(Unit.get_Default());
                return false;
            }
          }
          return true;
        case TweenStatus.Pausing:
          return true;
        default:
          this.Status = TweenStatus.Stopped;
          return false;
      }
    }
  }
}
