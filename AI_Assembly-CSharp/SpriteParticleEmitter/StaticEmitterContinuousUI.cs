// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.StaticEmitterContinuousUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SpriteParticleEmitter
{
  public class StaticEmitterContinuousUI : StaticUIImageEmitter
  {
    [Header("Emission")]
    [Tooltip("Particles to emit per second")]
    public float EmissionRate = 1000f;
    protected float wMult = 100f;
    protected float hMult = 100f;
    protected float ParticlesToEmitThisFrame;
    private RectTransform targetRectTransform;
    private RectTransform currentRectTransform;
    protected Vector2 offsetXY;

    public override event SimpleEvent OnAvailableToPlay;

    protected override void Awake()
    {
      base.Awake();
      this.currentRectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      this.targetRectTransform = (RectTransform) ((Component) this.imageRenderer).GetComponent<RectTransform>();
    }

    protected override void Update()
    {
      base.Update();
      if (!this.isPlaying || !this.hasCachingEnded)
        return;
      this.ProcessPositionAndScale();
      this.Emit();
    }

    private void ProcessPositionAndScale()
    {
      if (this.matchImageRendererPostionData)
        ((Transform) this.currentRectTransform).set_position(new Vector3((float) ((Transform) this.targetRectTransform).get_position().x, (float) ((Transform) this.targetRectTransform).get_position().y, (float) ((Transform) this.targetRectTransform).get_position().z));
      this.currentRectTransform.set_pivot(this.targetRectTransform.get_pivot());
      if (this.matchImageRendererPostionData)
      {
        this.currentRectTransform.set_anchoredPosition(this.targetRectTransform.get_anchoredPosition());
        this.currentRectTransform.set_anchorMin(this.targetRectTransform.get_anchorMin());
        this.currentRectTransform.set_anchorMax(this.targetRectTransform.get_anchorMax());
        this.currentRectTransform.set_offsetMin(this.targetRectTransform.get_offsetMin());
        this.currentRectTransform.set_offsetMax(this.targetRectTransform.get_offsetMax());
      }
      if (this.matchImageRendererScale)
        ((Transform) this.currentRectTransform).set_localScale(((Transform) this.targetRectTransform).get_localScale());
      ((Transform) this.currentRectTransform).set_rotation(((Transform) this.targetRectTransform).get_rotation());
      RectTransform currentRectTransform = this.currentRectTransform;
      Rect rect1 = this.targetRectTransform.get_rect();
      double width1 = (double) ((Rect) ref rect1).get_width();
      Rect rect2 = this.targetRectTransform.get_rect();
      double height1 = (double) ((Rect) ref rect2).get_height();
      Vector2 vector2 = new Vector2((float) width1, (float) height1);
      currentRectTransform.set_sizeDelta(vector2);
      double num1 = 1.0 - this.currentRectTransform.get_pivot().x;
      Rect rect3 = this.currentRectTransform.get_rect();
      double width2 = (double) ((Rect) ref rect3).get_width();
      double num2 = num1 * width2;
      Rect rect4 = this.currentRectTransform.get_rect();
      double num3 = (double) ((Rect) ref rect4).get_width() / 2.0;
      float num4 = (float) (num2 - num3);
      double num5 = 1.0 - this.currentRectTransform.get_pivot().y;
      Rect rect5 = this.currentRectTransform.get_rect();
      double num6 = -(double) ((Rect) ref rect5).get_height();
      double num7 = num5 * num6;
      Rect rect6 = this.currentRectTransform.get_rect();
      double num8 = (double) ((Rect) ref rect6).get_height() / 2.0;
      float num9 = (float) (num7 + num8);
      this.offsetXY = new Vector2(num4, num9);
      Sprite sprite = this.imageRenderer.get_sprite();
      double pixelsPerUnit1 = (double) sprite.get_pixelsPerUnit();
      Rect rect7 = this.currentRectTransform.get_rect();
      double width3 = (double) ((Rect) ref rect7).get_width();
      Rect rect8 = sprite.get_rect();
      // ISSUE: variable of the null type
      __Null x = ((Rect) ref rect8).get_size().x;
      double num10 = width3 / x;
      this.wMult = (float) (pixelsPerUnit1 * num10);
      double pixelsPerUnit2 = (double) sprite.get_pixelsPerUnit();
      Rect rect9 = this.currentRectTransform.get_rect();
      double height2 = (double) ((Rect) ref rect9).get_height();
      Rect rect10 = sprite.get_rect();
      // ISSUE: variable of the null type
      __Null y = ((Rect) ref rect10).get_size().y;
      double num11 = height2 / y;
      this.hMult = (float) (pixelsPerUnit2 * num11);
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
      Vector3 position = ((Transform) this.currentRectTransform).get_position();
      Quaternion rotation = ((Transform) this.currentRectTransform).get_rotation();
      Vector3 localScale = ((Transform) this.currentRectTransform).get_localScale();
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
        ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
        if (this.UsePixelSourceColor)
          ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(particleInitColorCache[index2]));
        ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
        Vector3 vector3 = initPositionsCache[index2];
        if (simulationSpace == 1)
        {
          zero.x = (__Null) (vector3.x * (double) this.wMult * localScale.x + this.offsetXY.x);
          zero.y = (__Null) (vector3.y * (double) this.hMult * localScale.y - this.offsetXY.y);
          ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(rotation, zero), position));
          this.particlesSystem.Emit(emitParams, 1);
        }
        else
        {
          zero.x = (__Null) (vector3.x * (double) this.wMult + this.offsetXY.x);
          zero.y = (__Null) (vector3.y * (double) this.hMult - this.offsetXY.y);
          ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
      this.ParticlesToEmitThisFrame -= (float) particlesToEmitThisFrame;
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
