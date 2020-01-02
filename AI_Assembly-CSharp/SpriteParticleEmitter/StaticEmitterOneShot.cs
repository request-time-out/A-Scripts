// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.StaticEmitterOneShot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace SpriteParticleEmitter
{
  public class StaticEmitterOneShot : StaticSpriteEmitter
  {
    [Tooltip("Must the script disable referenced spriteRenderer component?")]
    public bool HideOriginalSpriteOnPlay = true;
    [Header("Silent Emission")]
    [Tooltip("Should start Silent Emitting as soon as has cache ended? (Refer to manual for further explanation)")]
    public bool SilentEmitOnAwake = true;
    [Tooltip("Silent emission can be expensive. This defines the lower limit fps can go before continue silent emission on next frame (Refer to manual for further explanation)")]
    public float WantedFPSDuringSilentEmission = 60f;
    protected bool SilentEmissionEnded;
    protected bool hasSilentEmissionAlreadyBeenShot;

    public override event SimpleEvent OnAvailableToPlay;

    protected override void Awake()
    {
      base.Awake();
      this.SilentEmissionEnded = false;
      if (!this.SilentEmitOnAwake)
        return;
      this.EmitSilently();
    }

    public override void CacheSprite(bool relativeToParent = false)
    {
      base.CacheSprite(this.SimulationSpace == 1);
      if (((ParticleSystem.MainModule) ref this.mainModule).get_maxParticles() < this.particlesCacheCount)
        ((ParticleSystem.MainModule) ref this.mainModule).set_maxParticles(Mathf.CeilToInt((float) this.particlesCacheCount));
      this.SilentEmissionEnded = false;
      this.hasSilentEmissionAlreadyBeenShot = false;
    }

    public void EmitSilently()
    {
      this.StartCoroutine(this.EmitParticlesSilently());
    }

    [DebuggerHidden]
    private IEnumerator EmitParticlesSilently()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StaticEmitterOneShot.\u003CEmitParticlesSilently\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void SetHideSpriteOnPlay(bool hideOriginalSprite)
    {
      this.HideOriginalSpriteOnPlay = hideOriginalSprite;
    }

    private bool PlayOneShot()
    {
      if (this.HideOriginalSpriteOnPlay)
        ((Renderer) this.spriteRenderer).set_enabled(false);
      if (!this.SilentEmissionEnded)
      {
        Debug.LogError((object) "Silent particles haven't been emitted yet. Particles need to be emitted silently first for PlayOneShot to work (Please Refer to manual)");
        return false;
      }
      this.particlesSystem.Play();
      this.isPlaying = true;
      this.hasSilentEmissionAlreadyBeenShot = true;
      return true;
    }

    public override void Play()
    {
      if (!this.IsAvailableToPlay())
        return;
      if (!this.hasSilentEmissionAlreadyBeenShot)
      {
        if (this.isPlaying)
          return;
        this.PlayOneShot();
      }
      else
      {
        if (this.isPlaying)
          return;
        this.particlesSystem.Play();
        this.isPlaying = true;
      }
    }

    public override void Stop()
    {
      if (this.isPlaying)
        this.particlesSystem.Pause();
      this.isPlaying = false;
    }

    public override void Pause()
    {
      if (this.isPlaying)
        this.particlesSystem.Pause();
      this.isPlaying = false;
    }

    public void Reset()
    {
      this.EmitSilently();
    }

    public override bool IsAvailableToPlay()
    {
      return this.hasCachingEnded && !this.isPlaying && this.SilentEmissionEnded;
    }
  }
}
