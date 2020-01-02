// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.DynamicEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using SpriteToParticlesAsset;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace SpriteParticleEmitter
{
  [ExecuteInEditMode]
  public class DynamicEmitter : EmitterBase
  {
    [Tooltip("Should the system cache sprites data? (Refer to manual for further explanation)")]
    public bool CacheSprites = true;
    private Color[] colorCache = new Color[1];
    private int[] indexCache = new int[1];
    protected Dictionary<Sprite, Color[]> spritesSoFar = new Dictionary<Sprite, Color[]>();

    protected override void Awake()
    {
      base.Awake();
      if (this.PlayOnAwake)
        this.isPlaying = true;
      if ((double) ((ParticleSystem.MainModule) ref this.mainModule).get_maxParticles() >= (double) this.EmissionRate)
        return;
      ((ParticleSystem.MainModule) ref this.mainModule).set_maxParticles(Mathf.CeilToInt(this.EmissionRate));
    }

    protected void Update()
    {
      if (!this.isPlaying)
        return;
      this.ParticlesToEmitThisFrame += this.EmissionRate * Time.get_deltaTime();
      int particlesToEmitThisFrame = (int) this.ParticlesToEmitThisFrame;
      if (particlesToEmitThisFrame > 0)
        this.Emit(particlesToEmitThisFrame);
      this.ParticlesToEmitThisFrame -= (float) particlesToEmitThisFrame;
    }

    public void Emit(int emitCount)
    {
      Sprite sprite1 = this.spriteRenderer.get_sprite();
      if (!Object.op_Implicit((Object) sprite1))
      {
        if (!this.verboseDebug)
          return;
        Debug.LogError((object) ("Unable to emit. Sprite is null in game object " + ((Object) this).get_name()));
      }
      else
      {
        float r = (float) this.EmitFromColor.r;
        float g = (float) this.EmitFromColor.g;
        float b = (float) this.EmitFromColor.b;
        Vector3 vector3_1 = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_position();
        Quaternion quaternion = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_rotation();
        Vector3 vector3_2 = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_lossyScale();
        if (this.SimulationSpace == null)
        {
          vector3_1 = Vector3.get_zero();
          vector3_2 = Vector3.get_one();
          quaternion = Quaternion.get_identity();
        }
        bool flipX = this.spriteRenderer.get_flipX();
        bool flipY = this.spriteRenderer.get_flipY();
        float pixelsPerUnit = sprite1.get_pixelsPerUnit();
        Rect rect1 = sprite1.get_rect();
        float x1 = (float) (int) ((Rect) ref rect1).get_size().x;
        Rect rect2 = sprite1.get_rect();
        float y1 = (float) (int) ((Rect) ref rect2).get_size().y;
        int num1 = (int) x1;
        float num2 = (float) (1.0 / (double) pixelsPerUnit / 2.0);
        double num3 = (double) (1f / pixelsPerUnit);
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        double constant = (double) ((ParticleSystem.MinMaxCurve) ref startSize).get_constant();
        float num4 = (float) (num3 * constant);
        float num5 = (float) sprite1.get_pivot().x / pixelsPerUnit;
        float num6 = (float) sprite1.get_pivot().y / pixelsPerUnit;
        Color[] colorArray;
        if (this.useSpritesSharingCache && Application.get_isPlaying())
        {
          Sprite sprite2 = sprite1;
          Rect rect3 = sprite1.get_rect();
          int x2 = (int) ((Rect) ref rect3).get_position().x;
          Rect rect4 = sprite1.get_rect();
          int y2 = (int) ((Rect) ref rect4).get_position().y;
          int blockWidth = (int) x1;
          int blockHeight = (int) y1;
          colorArray = SpritesDataPool.GetSpriteColors(sprite2, x2, y2, blockWidth, blockHeight);
        }
        else if (this.CacheSprites)
        {
          if (this.spritesSoFar.ContainsKey(sprite1))
          {
            colorArray = this.spritesSoFar[sprite1];
          }
          else
          {
            Texture2D texture = sprite1.get_texture();
            Rect rect3 = sprite1.get_rect();
            int x2 = (int) ((Rect) ref rect3).get_position().x;
            Rect rect4 = sprite1.get_rect();
            int y2 = (int) ((Rect) ref rect4).get_position().y;
            int num7 = (int) x1;
            int num8 = (int) y1;
            colorArray = texture.GetPixels(x2, y2, num7, num8);
            this.spritesSoFar.Add(sprite1, colorArray);
          }
        }
        else
        {
          Texture2D texture = sprite1.get_texture();
          Rect rect3 = sprite1.get_rect();
          int x2 = (int) ((Rect) ref rect3).get_position().x;
          Rect rect4 = sprite1.get_rect();
          int y2 = (int) ((Rect) ref rect4).get_position().y;
          int num7 = (int) x1;
          int num8 = (int) y1;
          colorArray = texture.GetPixels(x2, y2, num7, num8);
        }
        float redTolerance = this.RedTolerance;
        float greenTolerance = this.GreenTolerance;
        float blueTolerance = this.BlueTolerance;
        float num9 = x1 * y1;
        Color[] colorCache = this.colorCache;
        int[] indexCache = this.indexCache;
        if ((double) colorCache.Length < (double) num9)
        {
          this.colorCache = new Color[(int) num9];
          this.indexCache = new int[(int) num9];
          colorCache = this.colorCache;
          indexCache = this.indexCache;
        }
        bool emissionFromColor = this.UseEmissionFromColor;
        int index1 = 0;
        bool flag1 = this.borderEmission == EmitterBase.BorderEmission.Fast || this.borderEmission == EmitterBase.BorderEmission.Precise;
        if (flag1)
        {
          bool flag2 = false;
          Color color1 = colorArray[0];
          int num7 = (int) x1;
          bool flag3 = this.borderEmission == EmitterBase.BorderEmission.Precise;
          for (int index2 = 0; (double) index2 < (double) num9; ++index2)
          {
            Color color2 = colorArray[index2];
            bool flag4 = color2.a > 0.0;
            if (flag3)
            {
              int index3 = index2 - num7;
              if (index3 > 0)
              {
                Color color3 = colorArray[index3];
                bool flag5 = color3.a > 0.0;
                if (flag4)
                {
                  if (!flag5)
                  {
                    if (!emissionFromColor || FloatComparer.AreEqual(r, (float) color2.r, redTolerance) && FloatComparer.AreEqual(g, (float) color2.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color2.b, blueTolerance))
                    {
                      colorCache[index1] = color2;
                      indexCache[index1] = index2;
                      ++index1;
                      color1 = color2;
                      flag2 = true;
                      continue;
                    }
                    continue;
                  }
                }
                else if (flag5)
                {
                  if (!emissionFromColor || FloatComparer.AreEqual(r, (float) color3.r, redTolerance) && FloatComparer.AreEqual(g, (float) color3.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color3.b, blueTolerance))
                  {
                    colorCache[index1] = color3;
                    indexCache[index1] = index3;
                    ++index1;
                  }
                  else
                    continue;
                }
              }
            }
            if (flag1 && !flag4 && flag2)
            {
              if (!emissionFromColor || FloatComparer.AreEqual(r, (float) color1.r, redTolerance) && FloatComparer.AreEqual(g, (float) color1.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color1.b, blueTolerance))
              {
                colorCache[index1] = color1;
                indexCache[index1] = index2 - 1;
                ++index1;
                flag2 = true;
              }
              else
                continue;
            }
            color1 = color2;
            if (!flag4)
              flag2 = false;
            else if ((!flag1 || flag4 && !flag2) && (!emissionFromColor || FloatComparer.AreEqual(r, (float) color2.r, redTolerance) && FloatComparer.AreEqual(g, (float) color2.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color2.b, blueTolerance)))
            {
              colorCache[index1] = color2;
              indexCache[index1] = index2;
              ++index1;
              flag2 = true;
            }
          }
        }
        else
        {
          for (int index2 = 0; (double) index2 < (double) num9; ++index2)
          {
            Color color = colorArray[index2];
            if (color.a > 0.0 && (!emissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
            {
              colorCache[index1] = color;
              indexCache[index1] = index2;
              ++index1;
            }
          }
        }
        if (index1 <= 0)
          return;
        Vector3 zero = Vector3.get_zero();
        for (int index2 = 0; index2 < emitCount; ++index2)
        {
          int index3 = Random.Range(0, index1 - 1);
          int num7 = indexCache[index3];
          float num8 = (float) num7 % x1 / pixelsPerUnit - num5;
          float num10 = (float) (num7 / num1) / pixelsPerUnit - num6;
          if (flipX)
            num8 = (float) ((double) x1 / (double) pixelsPerUnit - (double) num8 - (double) num5 * 2.0);
          if (flipY)
            num10 = (float) ((double) y1 / (double) pixelsPerUnit - (double) num10 - (double) num6 * 2.0);
          zero.x = (__Null) ((double) num8 * vector3_2.x - (double) num2);
          zero.y = (__Null) ((double) num10 * vector3_2.y + (double) num2);
          ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
          ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(quaternion, zero), vector3_1));
          if (this.UsePixelSourceColor)
            ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(colorCache[index3]));
          ((ParticleSystem.EmitParams) ref emitParams).set_startSize(num4);
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
    }

    public void EmitAll(bool hideSprite = true)
    {
      if (hideSprite)
        ((Renderer) this.spriteRenderer).set_enabled(false);
      Sprite sprite1 = this.spriteRenderer.get_sprite();
      if (!Object.op_Implicit((Object) sprite1))
      {
        if (!this.verboseDebug)
          return;
        Debug.LogError((object) ("Unable to emit. Sprite is null in game object " + ((Object) this).get_name()));
      }
      else
      {
        float r = (float) this.EmitFromColor.r;
        float g = (float) this.EmitFromColor.g;
        float b = (float) this.EmitFromColor.b;
        Vector3 vector3_1 = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_position();
        Quaternion quaternion = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_rotation();
        Vector3 vector3_2 = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_lossyScale();
        if (this.SimulationSpace == null)
        {
          vector3_1 = Vector3.get_zero();
          vector3_2 = Vector3.get_one();
          quaternion = Quaternion.get_identity();
        }
        bool flipX = this.spriteRenderer.get_flipX();
        bool flipY = this.spriteRenderer.get_flipY();
        float pixelsPerUnit = sprite1.get_pixelsPerUnit();
        Rect rect1 = sprite1.get_rect();
        float x1 = (float) (int) ((Rect) ref rect1).get_size().x;
        Rect rect2 = sprite1.get_rect();
        float y1 = (float) (int) ((Rect) ref rect2).get_size().y;
        double num1 = (double) (1f / pixelsPerUnit);
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        double constant = (double) ((ParticleSystem.MinMaxCurve) ref startSize).get_constant();
        float num2 = (float) (num1 * constant);
        float num3 = (float) sprite1.get_pivot().x / pixelsPerUnit;
        float num4 = (float) sprite1.get_pivot().y / pixelsPerUnit;
        Color[] colorArray;
        if (this.useSpritesSharingCache && Application.get_isPlaying())
        {
          Sprite sprite2 = sprite1;
          Rect rect3 = sprite1.get_rect();
          int x2 = (int) ((Rect) ref rect3).get_position().x;
          Rect rect4 = sprite1.get_rect();
          int y2 = (int) ((Rect) ref rect4).get_position().y;
          int blockWidth = (int) x1;
          int blockHeight = (int) y1;
          colorArray = SpritesDataPool.GetSpriteColors(sprite2, x2, y2, blockWidth, blockHeight);
        }
        else if (this.CacheSprites)
        {
          if (this.spritesSoFar.ContainsKey(sprite1))
          {
            colorArray = this.spritesSoFar[sprite1];
          }
          else
          {
            Texture2D texture = sprite1.get_texture();
            Rect rect3 = sprite1.get_rect();
            int x2 = (int) ((Rect) ref rect3).get_position().x;
            Rect rect4 = sprite1.get_rect();
            int y2 = (int) ((Rect) ref rect4).get_position().y;
            int num5 = (int) x1;
            int num6 = (int) y1;
            colorArray = texture.GetPixels(x2, y2, num5, num6);
            this.spritesSoFar.Add(sprite1, colorArray);
          }
        }
        else
        {
          Texture2D texture = sprite1.get_texture();
          Rect rect3 = sprite1.get_rect();
          int x2 = (int) ((Rect) ref rect3).get_position().x;
          Rect rect4 = sprite1.get_rect();
          int y2 = (int) ((Rect) ref rect4).get_position().y;
          int num5 = (int) x1;
          int num6 = (int) y1;
          colorArray = texture.GetPixels(x2, y2, num5, num6);
        }
        float redTolerance = this.RedTolerance;
        float greenTolerance = this.GreenTolerance;
        float blueTolerance = this.BlueTolerance;
        float num7 = x1 * y1;
        Vector3 zero = Vector3.get_zero();
        for (int index = 0; (double) index < (double) num7; ++index)
        {
          Color color = colorArray[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num5 = (float) index % x1 / pixelsPerUnit - num3;
            float num6 = (float) index / x1 / pixelsPerUnit - num4;
            if (flipX)
              num5 = (float) ((double) x1 / (double) pixelsPerUnit - (double) num5 - (double) num3 * 2.0);
            if (flipY)
              num6 = (float) ((double) y1 / (double) pixelsPerUnit - (double) num6 - (double) num4 * 2.0);
            zero.x = (__Null) ((double) num5 * vector3_2.x);
            zero.y = (__Null) ((double) num6 * vector3_2.y);
            ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
            ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(quaternion, zero), vector3_1));
            if (this.UsePixelSourceColor)
              ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(color));
            ((ParticleSystem.EmitParams) ref emitParams).set_startSize(num2);
            this.particlesSystem.Emit(emitParams, 1);
          }
        }
      }
    }

    public void RestoreSprite()
    {
      ((Renderer) this.spriteRenderer).set_enabled(true);
    }

    public override void Play()
    {
      if (!this.isPlaying)
        this.particlesSystem.Play();
      this.isPlaying = true;
    }

    public override void Pause()
    {
      if (this.isPlaying)
        this.particlesSystem.Pause();
      this.isPlaying = false;
    }

    public override void Stop()
    {
      this.isPlaying = false;
    }

    public override bool IsPlaying()
    {
      return this.isPlaying;
    }

    public override bool IsAvailableToPlay()
    {
      return true;
    }

    public void ClearCachedSprites()
    {
      this.spritesSoFar = new Dictionary<Sprite, Color[]>();
    }

    private void DummyMethod()
    {
    }
  }
}
