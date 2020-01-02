// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.StaticEmitterContinuous
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SpriteParticleEmitter
{
  public class StaticEmitterContinuous : StaticSpriteEmitter
  {
    public override event SimpleEvent OnAvailableToPlay;

    protected override void Update()
    {
      base.Update();
      if (!this.isPlaying || !this.hasCachingEnded)
        return;
      this.Emit();
    }

    public override void CacheSprite(bool relativeToParent = false)
    {
      base.CacheSprite(false);
      if (this.OnAvailableToPlay == null)
        return;
      this.OnAvailableToPlay();
    }

    protected void Emit()
    {
      if (!this.hasCachingEnded)
        return;
      this.ParticlesToEmitThisFrame += this.EmissionRate * Time.get_deltaTime();
      Vector3 position = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_position();
      Vector3 vector3_1 = position;
      Quaternion rotation = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_rotation();
      Vector3 lossyScale = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_lossyScale();
      ParticleSystemSimulationSpace simulationSpace = this.SimulationSpace;
      int particlesCacheCount = this.particlesCacheCount;
      float particleStartSize = this.particleStartSize;
      int particlesToEmitThisFrame = (int) this.ParticlesToEmitThisFrame;
      if (this.particlesCacheCount <= 0)
        return;
      Color[] particleInitColorCache = this.particleInitColorCache;
      Vector3[] initPositionsCache = this.particleInitPositionsCache;
      Vector3 zero = Vector3.get_zero();
      for (int index1 = 0; index1 < particlesToEmitThisFrame; ++index1)
      {
        int index2 = Random.Range(0, particlesCacheCount);
        if (this.useBetweenFramesPrecision)
        {
          float num = Random.Range(0.0f, 1f);
          vector3_1 = Vector3.Lerp(this.lastTransformPosition, position, num);
        }
        ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
        if (this.UsePixelSourceColor)
          ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(particleInitColorCache[index2]));
        ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
        if (simulationSpace == 1)
        {
          Vector3 vector3_2 = initPositionsCache[index2];
          zero.x = vector3_2.x * lossyScale.x;
          zero.y = vector3_2.y * lossyScale.y;
          ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(rotation, zero), vector3_1));
          this.particlesSystem.Emit(emitParams, 1);
        }
        else
        {
          ((ParticleSystem.EmitParams) ref emitParams).set_position(initPositionsCache[index2]);
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
      this.ParticlesToEmitThisFrame -= (float) particlesToEmitThisFrame;
      this.lastTransformPosition = position;
    }

    public override void Play()
    {
      if (!this.isPlaying)
        this.particlesSystem.Play();
      this.isPlaying = true;
    }

    public override void Stop()
    {
      this.isPlaying = false;
    }

    public override void Pause()
    {
      if (this.isPlaying)
        this.particlesSystem.Pause();
      this.isPlaying = false;
    }
  }
}
