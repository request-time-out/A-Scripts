// Decompiled with JetBrains decompiler
// Type: AIProject.EnviroParticleInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class EnviroParticleInfo : IDisposable
  {
    public int prefabInstanceID;
    public ParticleSystem particle;
    public IDisposable disposable;

    public EnviroParticleInfo(int _prefabInstanceID, ParticleSystem _particle)
    {
      this.prefabInstanceID = _prefabInstanceID;
      this.particle = _particle;
      this.disposable = (IDisposable) null;
      double num;
      if (Object.op_Equality((Object) this.particle, (Object) null))
      {
        num = 0.0;
      }
      else
      {
        ParticleSystem.EmissionModule emission = this.particle.get_emission();
        ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
        num = (double) ((ParticleSystem.MinMaxCurve) ref rateOverTime).get_constantMax();
      }
      this.EmissionMax = (float) num;
    }

    public float EmissionMax { get; private set; }

    public EnviroParticleInfo.FadeType fadeType { get; private set; }

    public float Emission
    {
      get
      {
        if (Object.op_Equality((Object) this.particle, (Object) null))
          return 0.0f;
        ParticleSystem.EmissionModule emission = this.particle.get_emission();
        ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
        return ((ParticleSystem.MinMaxCurve) ref rateOverTime).get_constantMax();
      }
      set
      {
        if (!Object.op_Inequality((Object) this.particle, (Object) null))
          return;
        ParticleSystem.EmissionModule emission = this.particle.get_emission();
        ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
        ((ParticleSystem.MinMaxCurve) ref rateOverTime).set_constantMax(value);
        ((ParticleSystem.EmissionModule) ref emission).set_rateOverTime(rateOverTime);
      }
    }

    public void PlayFadeIn(float _fadeTime)
    {
      if (Object.op_Equality((Object) this.particle, (Object) null))
        return;
      this.fadeType = EnviroParticleInfo.FadeType.FadeIn;
      this.Dispose();
      if ((double) _fadeTime <= 0.0)
      {
        this.Emission = this.EmissionMax;
        if (!this.particle.get_isPlaying())
          this.particle.Play(true);
        this.fadeType = EnviroParticleInfo.FadeType.Play;
      }
      else
      {
        float _startEmission = this.Emission;
        this.particle.Play(true);
        double num1 = (double) _fadeTime;
        ParticleSystem.MainModule main = this.particle.get_main();
        int num2 = ((ParticleSystem.MainModule) ref main).get_useUnscaledTime() ? 1 : 0;
        this.disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.Linear((float) num1, num2 != 0), (Component) this.particle), false), (Action<M0>) (time => this.Emission = Mathf.Lerp(_startEmission, this.EmissionMax, ((TimeInterval<float>) ref time).get_Value())), (Action) (() =>
        {
          this.fadeType = EnviroParticleInfo.FadeType.Play;
          this.Emission = this.EmissionMax;
          this.disposable = (IDisposable) null;
        }));
      }
    }

    public void PlayFadeOut(float _fadeTime)
    {
      if (Object.op_Equality((Object) this.particle, (Object) null))
        return;
      this.fadeType = EnviroParticleInfo.FadeType.FadeOut;
      this.Dispose();
      if ((double) _fadeTime <= 0.0)
      {
        this.Emission = 0.0f;
        if (!this.particle.get_isStopped())
          this.particle.Stop(true, (ParticleSystemStopBehavior) 0);
        this.fadeType = EnviroParticleInfo.FadeType.Stop;
      }
      else
      {
        float _startEmission = this.Emission;
        double num1 = (double) _fadeTime;
        ParticleSystem.MainModule main = this.particle.get_main();
        int num2 = ((ParticleSystem.MainModule) ref main).get_useUnscaledTime() ? 1 : 0;
        this.disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.Linear((float) num1, num2 != 0), (Component) this.particle), false), (Action<M0>) (time => this.Emission = Mathf.Lerp(_startEmission, 0.0f, ((TimeInterval<float>) ref time).get_Value())), (Action) (() =>
        {
          this.disposable = (IDisposable) null;
          if (Object.op_Inequality((Object) this.particle, (Object) null))
          {
            if (!this.particle.get_isStopped())
              this.particle.Stop(true, (ParticleSystemStopBehavior) 0);
            this.Emission = 0.0f;
          }
          this.fadeType = EnviroParticleInfo.FadeType.Stop;
        }));
      }
    }

    public void Dispose()
    {
      if (this.disposable == null)
        return;
      this.disposable.Dispose();
      this.disposable = (IDisposable) null;
    }

    public enum FadeType
    {
      Stop,
      Play,
      FadeIn,
      FadeOut,
    }
  }
}
