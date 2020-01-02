// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.DynamicEmitterUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using SpriteToParticlesAsset;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace SpriteParticleEmitter
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (UIParticleRenderer))]
  public class DynamicEmitterUI : EmitterBaseUI
  {
    [Tooltip("Start emitting as soon as able")]
    public bool PlayOnAwake = true;
    [Header("Emission")]
    [Tooltip("Particles to emit per second")]
    public float EmissionRate = 1000f;
    [Tooltip("Should the system cache sprites data? (Refer to manual for further explanation)")]
    public bool CacheSprites = true;
    private Color[] colorCache = new Color[1];
    private int[] indexCache = new int[1];
    protected Dictionary<Sprite, Color[]> spritesSoFar = new Dictionary<Sprite, Color[]>();
    protected float wMult = 100f;
    protected float hMult = 100f;
    protected float ParticlesToEmitThisFrame;
    private RectTransform targetRectTransform;
    private RectTransform currentRectTransform;
    protected Vector2 offsetXY;

    protected override void Awake()
    {
      base.Awake();
      if (this.PlayOnAwake)
        this.isPlaying = true;
      this.currentRectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      this.targetRectTransform = (RectTransform) ((Component) this.imageRenderer).GetComponent<RectTransform>();
      if ((double) ((ParticleSystem.MainModule) ref this.mainModule).get_maxParticles() >= (double) this.EmissionRate)
        return;
      ((ParticleSystem.MainModule) ref this.mainModule).set_maxParticles(Mathf.CeilToInt(this.EmissionRate));
    }

    protected void Update()
    {
      if (!this.isPlaying)
        return;
      if (Object.op_Equality((Object) this.imageRenderer, (Object) null))
      {
        if (this.verboseDebug)
          Debug.LogError((object) "Image Renderer component not referenced in DynamicEmitterUI component");
        this.isPlaying = false;
      }
      else
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
        double num1 = 1.0 - this.targetRectTransform.get_pivot().x;
        Rect rect3 = this.targetRectTransform.get_rect();
        double width2 = (double) ((Rect) ref rect3).get_width();
        double num2 = num1 * width2;
        Rect rect4 = this.targetRectTransform.get_rect();
        double num3 = (double) ((Rect) ref rect4).get_width() / 2.0;
        float num4 = (float) (num2 - num3);
        double num5 = 1.0 - this.targetRectTransform.get_pivot().y;
        Rect rect5 = this.targetRectTransform.get_rect();
        double num6 = -(double) ((Rect) ref rect5).get_height();
        double num7 = num5 * num6;
        Rect rect6 = this.targetRectTransform.get_rect();
        double num8 = (double) ((Rect) ref rect6).get_height() / 2.0;
        float num9 = (float) (num7 + num8);
        this.offsetXY = new Vector2(num4, num9);
        Sprite sprite = this.imageRenderer.get_sprite();
        if (!Object.op_Implicit((Object) sprite))
        {
          if (!this.verboseDebug)
            return;
          Debug.LogError((object) ("Unable to get positions. Sprite is null in game object " + ((Object) this).get_name()));
        }
        else
        {
          double pixelsPerUnit1 = (double) sprite.get_pixelsPerUnit();
          Rect rect7 = this.targetRectTransform.get_rect();
          double width3 = (double) ((Rect) ref rect7).get_width();
          Rect rect8 = sprite.get_rect();
          // ISSUE: variable of the null type
          __Null x = ((Rect) ref rect8).get_size().x;
          double num10 = width3 / x;
          this.wMult = (float) (pixelsPerUnit1 * num10);
          double pixelsPerUnit2 = (double) sprite.get_pixelsPerUnit();
          Rect rect9 = this.targetRectTransform.get_rect();
          double height2 = (double) ((Rect) ref rect9).get_height();
          Rect rect10 = sprite.get_rect();
          // ISSUE: variable of the null type
          __Null y = ((Rect) ref rect10).get_size().y;
          double num11 = height2 / y;
          this.hMult = (float) (pixelsPerUnit2 * num11);
          this.ParticlesToEmitThisFrame += this.EmissionRate * Time.get_deltaTime();
          int particlesToEmitThisFrame = (int) this.ParticlesToEmitThisFrame;
          if (particlesToEmitThisFrame > 0)
            this.Emit(particlesToEmitThisFrame);
          this.ParticlesToEmitThisFrame -= (float) particlesToEmitThisFrame;
        }
      }
    }

    public void Emit(int emitCount)
    {
      Sprite key = this.imageRenderer.get_sprite();
      if (Object.op_Implicit((Object) this.imageRenderer.get_overrideSprite()))
        key = this.imageRenderer.get_overrideSprite();
      if (!Object.op_Implicit((Object) key))
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
        float pixelsPerUnit = key.get_pixelsPerUnit();
        Rect rect1 = key.get_rect();
        float x1 = (float) (int) ((Rect) ref rect1).get_size().x;
        Rect rect2 = key.get_rect();
        float y1 = (float) (int) ((Rect) ref rect2).get_size().y;
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        float num1 = (float) key.get_pivot().x / pixelsPerUnit;
        float num2 = (float) key.get_pivot().y / pixelsPerUnit;
        Color[] colorArray;
        if (this.useSpritesSharingCache && Application.get_isPlaying())
        {
          Sprite sprite = key;
          Rect rect3 = key.get_rect();
          int x2 = (int) ((Rect) ref rect3).get_position().x;
          Rect rect4 = key.get_rect();
          int y2 = (int) ((Rect) ref rect4).get_position().y;
          int blockWidth = (int) x1;
          int blockHeight = (int) y1;
          colorArray = SpritesDataPool.GetSpriteColors(sprite, x2, y2, blockWidth, blockHeight);
        }
        else if (this.CacheSprites)
        {
          if (this.spritesSoFar.ContainsKey(key))
          {
            colorArray = this.spritesSoFar[key];
          }
          else
          {
            Texture2D texture = key.get_texture();
            Rect rect3 = key.get_rect();
            int x2 = (int) ((Rect) ref rect3).get_position().x;
            Rect rect4 = key.get_rect();
            int y2 = (int) ((Rect) ref rect4).get_position().y;
            int num3 = (int) x1;
            int num4 = (int) y1;
            colorArray = texture.GetPixels(x2, y2, num3, num4);
            this.spritesSoFar.Add(key, colorArray);
          }
        }
        else
        {
          Texture2D texture = key.get_texture();
          Rect rect3 = key.get_rect();
          int x2 = (int) ((Rect) ref rect3).get_position().x;
          Rect rect4 = key.get_rect();
          int y2 = (int) ((Rect) ref rect4).get_position().y;
          int num3 = (int) x1;
          int num4 = (int) y1;
          colorArray = texture.GetPixels(x2, y2, num3, num4);
        }
        float redTolerance = this.RedTolerance;
        float greenTolerance = this.GreenTolerance;
        float blueTolerance = this.BlueTolerance;
        float num5 = x1 * y1;
        Color[] colorCache = this.colorCache;
        int[] indexCache = this.indexCache;
        if ((double) colorCache.Length < (double) num5)
        {
          this.colorCache = new Color[(int) num5];
          this.indexCache = new int[(int) num5];
          colorCache = this.colorCache;
          indexCache = this.indexCache;
        }
        int index1 = 0;
        for (int index2 = 0; (double) index2 < (double) num5; ++index2)
        {
          Color color = colorArray[index2];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            colorCache[index1] = color;
            indexCache[index1] = index2;
            ++index1;
          }
        }
        if (index1 <= 0)
          return;
        Vector3 zero = Vector3.get_zero();
        for (int index2 = 0; index2 < emitCount; ++index2)
        {
          int index3 = Random.Range(0, index1 - 1);
          int num3 = indexCache[index3];
          float num4 = (float) num3 % x1 / pixelsPerUnit - num1;
          float num6 = (float) num3 / x1 / pixelsPerUnit - num2;
          ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
          zero.x = (__Null) ((double) num4 * (double) this.wMult + this.offsetXY.x);
          zero.y = (__Null) ((double) num6 * (double) this.hMult - this.offsetXY.y);
          ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
          if (this.UsePixelSourceColor)
            ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(colorCache[index3]));
          ((ParticleSystem.EmitParams) ref emitParams).set_startSize(((ParticleSystem.MinMaxCurve) ref startSize).get_constant());
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
    }

    public void EmitAll(bool hideSprite = true)
    {
      if (hideSprite)
        ((Behaviour) this.imageRenderer).set_enabled(false);
      Sprite sprite1 = this.imageRenderer.get_sprite();
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
        float pixelsPerUnit = sprite1.get_pixelsPerUnit();
        Rect rect1 = sprite1.get_rect();
        float x1 = (float) (int) ((Rect) ref rect1).get_size().x;
        Rect rect2 = sprite1.get_rect();
        float y1 = (float) (int) ((Rect) ref rect2).get_size().y;
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        float constant = ((ParticleSystem.MinMaxCurve) ref startSize).get_constant();
        float num1 = (float) sprite1.get_pivot().x / pixelsPerUnit;
        float num2 = (float) sprite1.get_pivot().y / pixelsPerUnit;
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
            int num3 = (int) x1;
            int num4 = (int) y1;
            colorArray = texture.GetPixels(x2, y2, num3, num4);
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
          int num3 = (int) x1;
          int num4 = (int) y1;
          colorArray = texture.GetPixels(x2, y2, num3, num4);
        }
        float redTolerance = this.RedTolerance;
        float greenTolerance = this.GreenTolerance;
        float blueTolerance = this.BlueTolerance;
        float num5 = x1 * y1;
        Vector3 zero = Vector3.get_zero();
        for (int index = 0; (double) index < (double) num5; ++index)
        {
          Color color = colorArray[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num3 = (float) index % x1 / pixelsPerUnit - num1;
            float num4 = (float) index / x1 / pixelsPerUnit - num2;
            ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
            zero.x = (__Null) ((double) num3 * (double) this.wMult + this.offsetXY.x);
            zero.y = (__Null) ((double) num4 * (double) this.hMult - this.offsetXY.y);
            ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
            if (this.UsePixelSourceColor)
              ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(color));
            ((ParticleSystem.EmitParams) ref emitParams).set_startSize(constant);
            this.particlesSystem.Emit(emitParams, 1);
          }
        }
      }
    }

    public void RestoreSprite()
    {
      if (!Object.op_Implicit((Object) this.imageRenderer))
        return;
      ((Behaviour) this.imageRenderer).set_enabled(true);
    }

    public override void Play()
    {
      if (!this.isPlaying && Object.op_Implicit((Object) this.particlesSystem))
        this.particlesSystem.Play();
      this.isPlaying = true;
    }

    public override void Pause()
    {
      if (this.isPlaying && Object.op_Implicit((Object) this.particlesSystem))
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
  }
}
