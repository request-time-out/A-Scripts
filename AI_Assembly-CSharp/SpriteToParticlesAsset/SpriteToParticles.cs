// Decompiled with JetBrains decompiler
// Type: SpriteToParticlesAsset.SpriteToParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SpriteToParticlesAsset
{
  [ExecuteInEditMode]
  public class SpriteToParticles : MonoBehaviour
  {
    [Tooltip("Weather the system is being used for Sprite or Image component. ")]
    public RenderSystemUsing renderSystemType;
    [Tooltip("Weather the system is using static or dynamic mode.")]
    public SpriteMode mode;
    [Tooltip("Should log warnings and errors?")]
    public bool verboseDebug;
    [Tooltip("If none is provided the script will look for one in this game object.")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("Must be provided by other GameObject's ImageRenderer.")]
    public Image imageRenderer;
    [Tooltip("If none is provided the script will look for one in this game object.")]
    public ParticleSystem particlesSystem;
    [Tooltip("Start emitting as soon as able. (On static emission activating this will force CacheOnAwake)")]
    public bool PlayOnAwake;
    [Tooltip("Particles to emit per second")]
    public float EmissionRate;
    [Tooltip("Should new particles override ParticleSystem's startColor and use the color in the pixel they're emitting from?")]
    public bool UsePixelSourceColor;
    [Tooltip("Emit from sprite border. Fast will work on the x axis only. Precise works on both x and y axis but is more performance heavy. (Border emission only works in dynamic mode currently)")]
    public SpriteToParticles.BorderEmission borderEmission;
    [Tooltip("Activating this will make the Emitter only emit from selected color")]
    public bool UseEmissionFromColor;
    [Tooltip("Emission will take this color as only source position")]
    public Color EmitFromColor;
    [Range(0.01f, 1f)]
    [Tooltip("In conjunction with EmitFromColor. Defines how much can it deviate from red spectrum for selected color.")]
    public float RedTolerance;
    [Range(0.0f, 1f)]
    [Tooltip("In conjunction with EmitFromColor. Defines how much can it deviate from green spectrum for selected color.")]
    public float GreenTolerance;
    [Range(0.0f, 1f)]
    [Tooltip("In conjunction with EmitFromColor. Defines how much can it deviate from blue spectrum for selected color.")]
    public float BlueTolerance;
    [Tooltip("This will save memory size when dealing with same sprite being loaded repeatedly by different GameObjects.")]
    public bool useSpritesSharingPool;
    [Tooltip("Weather use BetweenFrames precision or not. (Refer to manual for further explanation)")]
    public bool useBetweenFramesPrecision;
    [Tooltip("Should the system cache sprites data? (Refer to manual for further explanation)")]
    public bool CacheSprites;
    [Tooltip("Should the transform match target Renderer GameObject Position? (For Image Component(UI) StP Object must have same parent as the Renderer Image component Transform)")]
    [FormerlySerializedAs("matchImageRendererPostionData")]
    public bool matchTargetGOPostionData;
    [Tooltip("Should the transform match target Renderer Renderer Scale? (For Image Component(UI) StP Object must have same parent as the Image component Transform. For Sprite Component it will match local scale data)")]
    [FormerlySerializedAs("matchImageRendererScale")]
    public bool matchTargetGOScale;
    private ParticleSystemSimulationSpace SimulationSpace;
    private bool isPlaying;
    public UIParticleRenderer uiParticleSystem;
    private ParticleSystem.MainModule mainModule;
    private float ParticlesToEmitThisFrame;
    private Vector3 lastTransformPosition;
    private Transform spriteTransformReference;
    private Color[] colorCache;
    private int[] indexCache;
    private Dictionary<Sprite, Color[]> spritesSoFar;
    private RectTransform targetRectTransform;
    private RectTransform currentRectTransform;
    private Vector2 offsetXY;
    private float wMult;
    private float hMult;
    [Tooltip("Should the system cache on Awake method? - Static emission needs to be cached first, if this property is not checked the CacheSprite() method should be called by code. (Refer to manual for further explanation)")]
    public bool CacheOnAwake;
    private bool hasCachingEnded;
    private int particlesCacheCount;
    private float particleStartSize;
    private Vector3[] particleInitPositionsCache;
    private Color[] particleInitColorCache;
    private bool forceHack;

    public SpriteToParticles()
    {
      base.\u002Ector();
    }

    public event SimpleEvent OnCacheEnded;

    public event SimpleEvent OnAvailableToPlay;

    protected virtual void Awake()
    {
      this.spritesSoFar = new Dictionary<Sprite, Color[]>();
      this.colorCache = new Color[1];
      this.indexCache = new int[1];
      this.particleInitPositionsCache = (Vector3[]) null;
      this.particleInitColorCache = (Color[]) null;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        if (!Object.op_Implicit((Object) this.spriteRenderer))
        {
          if (this.verboseDebug)
            Debug.LogWarning((object) "Sprite Renderer not defined, trying to find in same GameObject. ");
          this.spriteRenderer = (SpriteRenderer) ((Component) this).GetComponent<SpriteRenderer>();
          if (!Object.op_Implicit((Object) this.spriteRenderer) && this.verboseDebug)
            Debug.LogWarning((object) "Sprite Renderer not found");
        }
        if (Object.op_Implicit((Object) this.spriteRenderer))
        {
          this.spriteTransformReference = ((Component) this.spriteRenderer).get_gameObject().get_transform();
          this.lastTransformPosition = this.spriteTransformReference.get_position();
        }
      }
      else
      {
        this.uiParticleSystem = (UIParticleRenderer) ((Component) this).GetComponent<UIParticleRenderer>();
        if (!Object.op_Implicit((Object) this.uiParticleSystem))
        {
          if (this.verboseDebug)
            Debug.LogWarning((object) "UIParticleRenderer couldn't be found, component must be added in order for the system to work. ");
          this.isPlaying = false;
          return;
        }
        if (!Object.op_Implicit((Object) this.imageRenderer))
        {
          if (this.verboseDebug)
            Debug.LogWarning((object) "Image Renderer not defined, must be defined in order for the system to work. ");
          this.isPlaying = false;
          return;
        }
      }
      if (!Object.op_Implicit((Object) this.particlesSystem))
      {
        this.particlesSystem = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
        if (!Object.op_Implicit((Object) this.particlesSystem))
        {
          if (this.verboseDebug)
            Debug.LogError((object) "No particle system found. Static Sprite Emission won't work. ");
          this.isPlaying = false;
          return;
        }
      }
      this.mainModule = this.particlesSystem.get_main();
      ((ParticleSystem.MainModule) ref this.mainModule).set_loop(true);
      ((ParticleSystem.MainModule) ref this.mainModule).set_playOnAwake(true);
      this.particlesSystem.Stop();
      this.SimulationSpace = ((ParticleSystem.MainModule) ref this.mainModule).get_simulationSpace();
      if (this.PlayOnAwake)
      {
        this.isPlaying = true;
        this.particlesSystem.Emit(1);
        this.particlesSystem.Clear();
        if (Application.get_isPlaying())
          this.particlesSystem.Play();
      }
      if (this.renderSystemType == RenderSystemUsing.ImageRenderer)
      {
        this.currentRectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
        this.targetRectTransform = (RectTransform) ((Component) this.imageRenderer).GetComponent<RectTransform>();
      }
      if ((double) ((ParticleSystem.MainModule) ref this.mainModule).get_maxParticles() < (double) this.EmissionRate)
        ((ParticleSystem.MainModule) ref this.mainModule).set_maxParticles(Mathf.CeilToInt(this.EmissionRate));
      if (this.mode != SpriteMode.Static)
        return;
      if (this.PlayOnAwake)
        this.CacheOnAwake = true;
      if (!this.CacheOnAwake)
        return;
      this.CacheSprite(false);
    }

    public void Update()
    {
      bool flag = this.isPlaying;
      if (this.mode == SpriteMode.Static)
        flag = this.isPlaying && this.hasCachingEnded;
      if (!flag)
        return;
      if (this.renderSystemType == RenderSystemUsing.ImageRenderer)
      {
        this.HandlePositionAndScaleForUI();
        if (!this.isPlaying)
          return;
      }
      else
        this.HandlePosition();
      this.ParticlesToEmitThisFrame += this.EmissionRate * Time.get_deltaTime();
      int particlesToEmitThisFrame = (int) this.ParticlesToEmitThisFrame;
      if (particlesToEmitThisFrame > 0)
        this.Emit(particlesToEmitThisFrame);
      this.ParticlesToEmitThisFrame -= (float) particlesToEmitThisFrame;
      if (this.renderSystemType != RenderSystemUsing.SpriteRenderer)
        return;
      if (!Object.op_Implicit((Object) this.spriteTransformReference))
        this.spriteTransformReference = ((Component) this.spriteRenderer).get_transform();
      this.lastTransformPosition = this.spriteTransformReference.get_position();
    }

    public void Emit(int emitCount)
    {
      this.HackUnityCrash2017();
      if (this.mode == SpriteMode.Dynamic)
        this.EmitDynamic(emitCount);
      else
        this.EmitStatic(emitCount);
    }

    public void EmitAll(bool hideSprite = true)
    {
      if (hideSprite)
      {
        if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
          ((Renderer) this.spriteRenderer).set_enabled(false);
        else
          ((Behaviour) this.imageRenderer).set_enabled(false);
      }
      if (this.mode == SpriteMode.Dynamic)
        this.EmitAllDynamic();
      else
        this.EmitAllStatic();
    }

    public void RestoreSprite()
    {
      if (this.renderSystemType != RenderSystemUsing.SpriteRenderer)
        return;
      ((Renderer) this.spriteRenderer).set_enabled(true);
    }

    public void Play()
    {
      if (!this.isPlaying)
        this.particlesSystem.Play();
      this.isPlaying = true;
    }

    public void Pause()
    {
      if (this.isPlaying)
        this.particlesSystem.Pause();
      this.isPlaying = false;
    }

    public void Stop()
    {
      this.isPlaying = false;
    }

    public bool IsPlaying()
    {
      return this.isPlaying;
    }

    public bool IsAvailableToPlay()
    {
      return this.mode != SpriteMode.Static || this.hasCachingEnded;
    }

    public void ClearCachedSprites()
    {
      this.spritesSoFar = new Dictionary<Sprite, Color[]>();
    }

    private void HandlePositionAndScaleForUI()
    {
      if (this.mode == SpriteMode.Dynamic)
        this.ProcessPositionAndScaleDynamic();
      else
        this.ProcessPositionAndScaleStatic();
    }

    private void HandlePosition()
    {
      if (this.matchTargetGOPostionData && Object.op_Inequality((Object) this.spriteRenderer, (Object) null))
        ((Component) this).get_transform().set_position(((Component) this.spriteRenderer).get_transform().get_position());
      if (!this.matchTargetGOScale || !Object.op_Inequality((Object) this.spriteRenderer, (Object) null))
        return;
      ((Component) this).get_transform().set_localScale(((Component) this.spriteRenderer).get_transform().get_lossyScale());
    }

    private void ProcessPositionAndScaleDynamic()
    {
      if (Object.op_Equality((Object) this.imageRenderer, (Object) null))
      {
        if (this.verboseDebug)
          Debug.LogError((object) "Image Renderer component not referenced in DynamicEmitterUI component");
        this.isPlaying = false;
      }
      else
      {
        if (this.matchTargetGOPostionData)
          ((Transform) this.currentRectTransform).set_position(new Vector3((float) ((Transform) this.targetRectTransform).get_position().x, (float) ((Transform) this.targetRectTransform).get_position().y, (float) ((Transform) this.targetRectTransform).get_position().z));
        this.currentRectTransform.set_pivot(this.targetRectTransform.get_pivot());
        if (this.matchTargetGOPostionData)
        {
          this.currentRectTransform.set_anchoredPosition(this.targetRectTransform.get_anchoredPosition());
          this.currentRectTransform.set_anchorMin(this.targetRectTransform.get_anchorMin());
          this.currentRectTransform.set_anchorMax(this.targetRectTransform.get_anchorMax());
          this.currentRectTransform.set_offsetMin(this.targetRectTransform.get_offsetMin());
          this.currentRectTransform.set_offsetMax(this.targetRectTransform.get_offsetMax());
        }
        if (this.matchTargetGOScale)
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
        Sprite sprite = this.GetSprite();
        if (this.imageRenderer.get_preserveAspect())
        {
          Rect rect7 = sprite.get_rect();
          // ISSUE: variable of the null type
          __Null y1 = ((Rect) ref rect7).get_size().y;
          Rect rect8 = sprite.get_rect();
          // ISSUE: variable of the null type
          __Null x1 = ((Rect) ref rect8).get_size().x;
          float num10 = (float) (y1 / x1);
          Rect rect9 = this.targetRectTransform.get_rect();
          double height2 = (double) ((Rect) ref rect9).get_height();
          Rect rect10 = this.targetRectTransform.get_rect();
          double width3 = (double) ((Rect) ref rect10).get_width();
          float num11 = (float) (height2 / width3);
          if ((double) num10 > (double) num11)
          {
            double pixelsPerUnit1 = (double) sprite.get_pixelsPerUnit();
            Rect rect11 = this.targetRectTransform.get_rect();
            double width4 = (double) ((Rect) ref rect11).get_width();
            Rect rect12 = sprite.get_rect();
            // ISSUE: variable of the null type
            __Null x2 = ((Rect) ref rect12).get_size().x;
            double num12 = width4 / x2;
            this.wMult = (float) (pixelsPerUnit1 * num12 * ((double) num11 / (double) num10));
            double pixelsPerUnit2 = (double) sprite.get_pixelsPerUnit();
            Rect rect13 = this.targetRectTransform.get_rect();
            double height3 = (double) ((Rect) ref rect13).get_height();
            Rect rect14 = sprite.get_rect();
            // ISSUE: variable of the null type
            __Null y2 = ((Rect) ref rect14).get_size().y;
            double num13 = height3 / y2;
            this.hMult = (float) (pixelsPerUnit2 * num13);
          }
          else
          {
            double pixelsPerUnit1 = (double) sprite.get_pixelsPerUnit();
            Rect rect11 = this.targetRectTransform.get_rect();
            double width4 = (double) ((Rect) ref rect11).get_width();
            Rect rect12 = sprite.get_rect();
            // ISSUE: variable of the null type
            __Null x2 = ((Rect) ref rect12).get_size().x;
            double num12 = width4 / x2;
            this.wMult = (float) (pixelsPerUnit1 * num12);
            double pixelsPerUnit2 = (double) sprite.get_pixelsPerUnit();
            Rect rect13 = this.targetRectTransform.get_rect();
            double height3 = (double) ((Rect) ref rect13).get_height();
            Rect rect14 = sprite.get_rect();
            // ISSUE: variable of the null type
            __Null y2 = ((Rect) ref rect14).get_size().y;
            double num13 = height3 / y2;
            this.hMult = (float) (pixelsPerUnit2 * num13 * ((double) num10 / (double) num11));
          }
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
        }
      }
    }

    private void EmitDynamic(int emitCount)
    {
      Sprite sprite = this.GetSprite();
      if (!Object.op_Implicit((Object) sprite))
        return;
      float r = (float) this.EmitFromColor.r;
      float g = (float) this.EmitFromColor.g;
      float b = (float) this.EmitFromColor.b;
      float pixelsPerUnit = sprite.get_pixelsPerUnit();
      Rect rect1 = sprite.get_rect();
      float x = (float) (int) ((Rect) ref rect1).get_size().x;
      Rect rect2 = sprite.get_rect();
      float y = (float) (int) ((Rect) ref rect2).get_size().y;
      float particleStartSize = this.GetParticleStartSize(pixelsPerUnit);
      float num1 = (float) sprite.get_pivot().x / pixelsPerUnit;
      float num2 = (float) sprite.get_pivot().y / pixelsPerUnit;
      Color[] spriteColorsData = this.GetSpriteColorsData(sprite);
      float redTolerance = this.RedTolerance;
      float greenTolerance = this.GreenTolerance;
      float blueTolerance = this.BlueTolerance;
      float num3 = x * y;
      Color[] colorCache = this.colorCache;
      int[] indexCache = this.indexCache;
      if ((double) colorCache.Length < (double) num3)
      {
        this.colorCache = new Color[(int) num3];
        this.indexCache = new int[(int) num3];
        colorCache = this.colorCache;
        indexCache = this.indexCache;
      }
      int num4 = (int) x;
      float num5 = (float) (1.0 / (double) pixelsPerUnit / 2.0);
      Vector3 vector3_1 = Vector3.get_zero();
      Quaternion quaternion = Quaternion.get_identity();
      Vector3 vector3_2 = Vector3.get_one();
      bool flag1 = false;
      bool flag2 = false;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        if (this.SimulationSpace != null)
        {
          vector3_1 = this.spriteTransformReference.get_position();
          quaternion = this.spriteTransformReference.get_rotation();
          vector3_2 = this.spriteTransformReference.get_lossyScale();
        }
        flag1 = this.spriteRenderer.get_flipX();
        flag2 = this.spriteRenderer.get_flipY();
      }
      int index1 = 0;
      bool emissionFromColor = this.UseEmissionFromColor;
      bool flag3 = this.borderEmission == SpriteToParticles.BorderEmission.Fast || this.borderEmission == SpriteToParticles.BorderEmission.Precise;
      if (flag3)
      {
        bool flag4 = false;
        Color color1 = spriteColorsData[0];
        int num6 = (int) x;
        bool flag5 = this.borderEmission == SpriteToParticles.BorderEmission.Precise;
        for (int index2 = 0; (double) index2 < (double) num3; ++index2)
        {
          Color color2 = spriteColorsData[index2];
          bool flag6 = color2.a > 0.0;
          if (flag5)
          {
            int index3 = index2 - num6;
            if (index3 > 0)
            {
              Color color3 = spriteColorsData[index3];
              bool flag7 = color3.a > 0.0;
              if (flag6)
              {
                if (!flag7)
                {
                  if (!emissionFromColor || FloatComparer.AreEqual(r, (float) color2.r, redTolerance) && FloatComparer.AreEqual(g, (float) color2.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color2.b, blueTolerance))
                  {
                    colorCache[index1] = color2;
                    indexCache[index1] = index2;
                    ++index1;
                    color1 = color2;
                    flag4 = true;
                    continue;
                  }
                  continue;
                }
              }
              else if (flag7)
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
          if (!flag6 && flag4)
          {
            if (!emissionFromColor || FloatComparer.AreEqual(r, (float) color1.r, redTolerance) && FloatComparer.AreEqual(g, (float) color1.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color1.b, blueTolerance))
            {
              colorCache[index1] = color1;
              indexCache[index1] = index2 - 1;
              ++index1;
              flag4 = true;
            }
            else
              continue;
          }
          color1 = color2;
          if (!flag6)
            flag4 = false;
          else if ((!flag3 || flag6 && !flag4) && (!emissionFromColor || FloatComparer.AreEqual(r, (float) color2.r, redTolerance) && FloatComparer.AreEqual(g, (float) color2.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color2.b, blueTolerance)))
          {
            colorCache[index1] = color2;
            indexCache[index1] = index2;
            ++index1;
            flag4 = true;
          }
        }
      }
      else
      {
        for (int index2 = 0; (double) index2 < (double) num3; ++index2)
        {
          Color color = spriteColorsData[index2];
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
      Vector3 vector3_3 = vector3_1;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        for (int index2 = 0; index2 < emitCount; ++index2)
        {
          int index3 = Random.Range(0, index1 - 1);
          int num6 = indexCache[index3];
          if (this.useBetweenFramesPrecision)
          {
            float num7 = Random.Range(0.0f, 1f);
            vector3_3 = Vector3.Lerp(this.lastTransformPosition, vector3_1, num7);
          }
          float num8 = (float) num6 % x / pixelsPerUnit - num1 + num5;
          float num9 = (float) (num6 / num4) / pixelsPerUnit - num2 + num5;
          if (flag1)
            num8 = (float) ((double) x / (double) pixelsPerUnit - (double) num8 - (double) num1 * 2.0);
          if (flag2)
            num9 = (float) ((double) y / (double) pixelsPerUnit - (double) num9 - (double) num2 * 2.0);
          zero.x = (__Null) ((double) num8 * vector3_2.x);
          zero.y = (__Null) ((double) num9 * vector3_2.y);
          ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
          ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(quaternion, zero), vector3_3));
          if (this.UsePixelSourceColor)
            ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(colorCache[index3]));
          ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
      else
      {
        for (int index2 = 0; index2 < emitCount; ++index2)
        {
          int index3 = Random.Range(0, index1 - 1);
          int num6 = indexCache[index3];
          float num7 = (float) num6 % x / pixelsPerUnit - num1 + num5;
          float num8 = (float) (num6 / num4) / pixelsPerUnit - num2 + num5;
          ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
          zero.x = (__Null) ((double) num7 * (double) this.wMult + this.offsetXY.x);
          zero.y = (__Null) ((double) num8 * (double) this.hMult - this.offsetXY.y);
          ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
          if (this.UsePixelSourceColor)
            ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(colorCache[index3]));
          ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
    }

    private void EmitAllDynamic()
    {
      Sprite sprite = this.GetSprite();
      if (!Object.op_Implicit((Object) sprite))
        return;
      float r = (float) this.EmitFromColor.r;
      float g = (float) this.EmitFromColor.g;
      float b = (float) this.EmitFromColor.b;
      float pixelsPerUnit = sprite.get_pixelsPerUnit();
      Rect rect1 = sprite.get_rect();
      float x = (float) (int) ((Rect) ref rect1).get_size().x;
      Rect rect2 = sprite.get_rect();
      float y = (float) (int) ((Rect) ref rect2).get_size().y;
      float particleStartSize = this.GetParticleStartSize(pixelsPerUnit);
      float num1 = (float) sprite.get_pivot().x / pixelsPerUnit;
      float num2 = (float) sprite.get_pivot().y / pixelsPerUnit;
      Color[] spriteColorsData = this.GetSpriteColorsData(sprite);
      float redTolerance = this.RedTolerance;
      float greenTolerance = this.GreenTolerance;
      float blueTolerance = this.BlueTolerance;
      float num3 = x * y;
      int num4 = (int) x;
      float num5 = (float) (1.0 / (double) pixelsPerUnit / 2.0);
      Vector3 vector3_1 = Vector3.get_zero();
      Quaternion quaternion = Quaternion.get_identity();
      Vector3 vector3_2 = Vector3.get_one();
      bool flag1 = false;
      bool flag2 = false;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer && this.SimulationSpace != null)
      {
        vector3_1 = this.spriteTransformReference.get_position();
        quaternion = this.spriteTransformReference.get_rotation();
        vector3_2 = this.spriteTransformReference.get_lossyScale();
        flag1 = this.spriteRenderer.get_flipX();
        flag2 = this.spriteRenderer.get_flipY();
      }
      Vector3 zero = Vector3.get_zero();
      Vector3 vector3_3 = vector3_1;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        for (int index = 0; (double) index < (double) num3; ++index)
        {
          Color color = spriteColorsData[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num6 = (float) index % x / pixelsPerUnit - num1;
            float num7 = (float) (index / num4) / pixelsPerUnit - num2;
            if (flag1)
              num6 = (float) ((double) x / (double) pixelsPerUnit - (double) num6 - (double) num1 * 2.0);
            if (flag2)
              num7 = (float) ((double) y / (double) pixelsPerUnit - (double) num7 - (double) num2 * 2.0);
            zero.x = (__Null) ((double) num6 * vector3_2.x + (double) num5);
            zero.y = (__Null) ((double) num7 * vector3_2.y + (double) num5);
            ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
            ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(quaternion, zero), vector3_3));
            if (this.UsePixelSourceColor)
              ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(color));
            ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
            this.particlesSystem.Emit(emitParams, 1);
          }
        }
      }
      else
      {
        for (int index = 0; (double) index < (double) num3; ++index)
        {
          Color color = spriteColorsData[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num6 = (float) index % x / pixelsPerUnit - num1;
            float num7 = (float) (index / num4) / pixelsPerUnit - num2;
            ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
            zero.x = (__Null) ((double) num6 * (double) this.wMult + this.offsetXY.x + (double) num5);
            zero.y = (__Null) ((double) num7 * (double) this.hMult - this.offsetXY.y + (double) num5);
            ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
            if (this.UsePixelSourceColor)
              ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(color));
            ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
            this.particlesSystem.Emit(emitParams, 1);
          }
        }
      }
    }

    public void CacheSprite(bool relativeToParent = false)
    {
      if (!Object.op_Implicit((Object) this.particlesSystem))
        return;
      this.hasCachingEnded = false;
      this.particlesCacheCount = 0;
      Sprite sprite = this.GetSprite();
      if (!Object.op_Implicit((Object) sprite))
        return;
      float r = (float) this.EmitFromColor.r;
      float g = (float) this.EmitFromColor.g;
      float b = (float) this.EmitFromColor.b;
      float pixelsPerUnit = sprite.get_pixelsPerUnit();
      Rect rect1 = sprite.get_rect();
      float x = (float) (int) ((Rect) ref rect1).get_size().x;
      Rect rect2 = sprite.get_rect();
      float y = (float) (int) ((Rect) ref rect2).get_size().y;
      int num1 = (int) x;
      float num2 = (float) (1.0 / (double) pixelsPerUnit / 2.0);
      this.particleStartSize = this.GetParticleStartSize(pixelsPerUnit);
      float num3 = (float) sprite.get_pivot().x / pixelsPerUnit;
      float num4 = (float) sprite.get_pivot().y / pixelsPerUnit;
      Color[] spriteColorsData = this.GetSpriteColorsData(sprite);
      float redTolerance = this.RedTolerance;
      float greenTolerance = this.GreenTolerance;
      float blueTolerance = this.BlueTolerance;
      float num5 = x * y;
      Vector3 vector3_1 = Vector3.get_zero();
      Quaternion quaternion = Quaternion.get_identity();
      Vector3 vector3_2 = Vector3.get_one();
      bool flag1 = false;
      bool flag2 = false;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        vector3_1 = this.spriteTransformReference.get_position();
        quaternion = this.spriteTransformReference.get_rotation();
        vector3_2 = this.spriteTransformReference.get_lossyScale();
        flag1 = this.spriteRenderer.get_flipX();
        flag2 = this.spriteRenderer.get_flipY();
      }
      List<Color> colorList = new List<Color>();
      List<Vector3> vector3List = new List<Vector3>();
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        for (int index = 0; (double) index < (double) num5; ++index)
        {
          Color color = spriteColorsData[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num6 = (float) index % x / pixelsPerUnit - num3 + num2;
            float num7 = (float) (index / num1) / pixelsPerUnit - num4 + num2;
            if (flag1)
              num6 = (float) ((double) x / (double) pixelsPerUnit - (double) num6 - (double) num3 * 2.0);
            if (flag2)
              num7 = (float) ((double) y / (double) pixelsPerUnit - (double) num7 - (double) num4 * 2.0);
            Vector3 vector3_3;
            if (relativeToParent)
              vector3_3 = Vector3.op_Addition(Quaternion.op_Multiply(quaternion, new Vector3(num6 * (float) vector3_2.x, num7 * (float) vector3_2.y, 0.0f)), vector3_1);
            else
              ((Vector3) ref vector3_3).\u002Ector(num6, num7, 0.0f);
            vector3List.Add(vector3_3);
            colorList.Add(color);
            ++this.particlesCacheCount;
          }
        }
      }
      else
      {
        for (int index = 0; (double) index < (double) num5; ++index)
        {
          Color color = spriteColorsData[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num6 = (float) index % x / pixelsPerUnit - num3 + num2;
            float num7 = (float) (index / num1) / pixelsPerUnit - num4 + num2;
            Vector3 vector3_3;
            ((Vector3) ref vector3_3).\u002Ector(num6, num7, 0.0f);
            vector3List.Add(vector3_3);
            colorList.Add(color);
            ++this.particlesCacheCount;
          }
        }
      }
      this.particleInitPositionsCache = vector3List.ToArray();
      this.particleInitColorCache = colorList.ToArray();
      if (this.particlesCacheCount <= 0)
      {
        if (!this.verboseDebug)
          return;
        Debug.LogWarning((object) "Caching particle emission went wrong. This is most probably because couldn't find wanted color in sprite");
      }
      else
      {
        vector3List.Clear();
        colorList.Clear();
        GC.Collect();
        this.hasCachingEnded = true;
        if (this.OnCacheEnded != null)
          this.OnCacheEnded();
        if (this.OnAvailableToPlay == null)
          return;
        this.OnAvailableToPlay();
      }
    }

    private void ProcessPositionAndScaleStatic()
    {
      if (this.matchTargetGOPostionData)
        ((Transform) this.currentRectTransform).set_position(new Vector3((float) ((Transform) this.targetRectTransform).get_position().x, (float) ((Transform) this.targetRectTransform).get_position().y, (float) ((Transform) this.targetRectTransform).get_position().z));
      this.currentRectTransform.set_pivot(this.targetRectTransform.get_pivot());
      if (this.matchTargetGOPostionData)
      {
        this.currentRectTransform.set_anchoredPosition(this.targetRectTransform.get_anchoredPosition());
        this.currentRectTransform.set_anchorMin(this.targetRectTransform.get_anchorMin());
        this.currentRectTransform.set_anchorMax(this.targetRectTransform.get_anchorMax());
        this.currentRectTransform.set_offsetMin(this.targetRectTransform.get_offsetMin());
        this.currentRectTransform.set_offsetMax(this.targetRectTransform.get_offsetMax());
      }
      if (this.matchTargetGOScale)
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
      Sprite sprite = this.GetSprite();
      if (!Object.op_Implicit((Object) sprite))
        return;
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

    private void EmitStatic(int emitCount)
    {
      if (!this.hasCachingEnded)
        return;
      int particlesCacheCount = this.particlesCacheCount;
      float particleStartSize = this.particleStartSize;
      bool flag = this.renderSystemType == RenderSystemUsing.SpriteRenderer;
      if (this.particlesCacheCount <= 0)
        return;
      Vector3 position;
      Quaternion rotation;
      Vector3 lossyScale;
      if (flag)
      {
        position = this.spriteTransformReference.get_position();
        rotation = this.spriteTransformReference.get_rotation();
        lossyScale = this.spriteTransformReference.get_lossyScale();
      }
      else
      {
        position = ((Transform) this.currentRectTransform).get_position();
        rotation = ((Transform) this.currentRectTransform).get_rotation();
        lossyScale = ((Transform) this.currentRectTransform).get_lossyScale();
      }
      Vector3 vector3_1 = position;
      ParticleSystemSimulationSpace simulationSpace = this.SimulationSpace;
      Color[] particleInitColorCache = this.particleInitColorCache;
      Vector3[] initPositionsCache = this.particleInitPositionsCache;
      Vector3 zero = Vector3.get_zero();
      for (int index1 = 0; index1 < emitCount; ++index1)
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
        Vector3 vector3_2 = initPositionsCache[index2];
        if (simulationSpace == 1)
        {
          if (flag)
          {
            zero.x = vector3_2.x * lossyScale.x;
            zero.y = vector3_2.y * lossyScale.y;
          }
          else
          {
            zero.x = (__Null) (vector3_2.x * (double) this.wMult * lossyScale.x + this.offsetXY.x);
            zero.y = (__Null) (vector3_2.y * (double) this.hMult * lossyScale.y - this.offsetXY.y);
          }
          ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(rotation, zero), vector3_1));
          this.particlesSystem.Emit(emitParams, 1);
        }
        else
        {
          if (flag)
          {
            ((ParticleSystem.EmitParams) ref emitParams).set_position(initPositionsCache[index2]);
          }
          else
          {
            zero.x = (__Null) (vector3_2.x * (double) this.wMult + this.offsetXY.x);
            zero.y = (__Null) (vector3_2.y * (double) this.hMult - this.offsetXY.y);
            ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
          }
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
    }

    private void EmitAllStatic()
    {
      if (!this.hasCachingEnded)
        return;
      int particlesCacheCount = this.particlesCacheCount;
      float particleStartSize = this.particleStartSize;
      bool flag = this.renderSystemType == RenderSystemUsing.SpriteRenderer;
      if (this.particlesCacheCount <= 0)
        return;
      Vector3 position;
      Quaternion rotation;
      Vector3 lossyScale;
      if (flag)
      {
        position = this.spriteTransformReference.get_position();
        rotation = this.spriteTransformReference.get_rotation();
        lossyScale = this.spriteTransformReference.get_lossyScale();
      }
      else
      {
        position = ((Transform) this.currentRectTransform).get_position();
        rotation = ((Transform) this.currentRectTransform).get_rotation();
        lossyScale = ((Transform) this.currentRectTransform).get_lossyScale();
      }
      Vector3 vector3_1 = position;
      ParticleSystemSimulationSpace simulationSpace = this.SimulationSpace;
      Color[] particleInitColorCache = this.particleInitColorCache;
      Vector3[] initPositionsCache = this.particleInitPositionsCache;
      Vector3 zero = Vector3.get_zero();
      for (int index = 0; index < particlesCacheCount; ++index)
      {
        if (this.useBetweenFramesPrecision)
        {
          float num = Random.Range(0.0f, 1f);
          vector3_1 = Vector3.Lerp(this.lastTransformPosition, position, num);
        }
        ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
        if (this.UsePixelSourceColor)
          ((ParticleSystem.EmitParams) ref emitParams).set_startColor(Color32.op_Implicit(particleInitColorCache[index]));
        ((ParticleSystem.EmitParams) ref emitParams).set_startSize(particleStartSize);
        Vector3 vector3_2 = initPositionsCache[index];
        if (simulationSpace == 1)
        {
          if (flag)
          {
            zero.x = vector3_2.x * lossyScale.x;
            zero.y = vector3_2.y * lossyScale.y;
          }
          else
          {
            zero.x = (__Null) (vector3_2.x * (double) this.wMult * lossyScale.x + this.offsetXY.x);
            zero.y = (__Null) (vector3_2.y * (double) this.hMult * lossyScale.y - this.offsetXY.y);
          }
          ((ParticleSystem.EmitParams) ref emitParams).set_position(Vector3.op_Addition(Quaternion.op_Multiply(rotation, zero), vector3_1));
          this.particlesSystem.Emit(emitParams, 1);
        }
        else
        {
          if (flag)
          {
            ((ParticleSystem.EmitParams) ref emitParams).set_position(initPositionsCache[index]);
          }
          else
          {
            zero.x = (__Null) (vector3_2.x * (double) this.wMult + this.offsetXY.x);
            zero.y = (__Null) (vector3_2.y * (double) this.hMult - this.offsetXY.y);
            ((ParticleSystem.EmitParams) ref emitParams).set_position(zero);
          }
          this.particlesSystem.Emit(emitParams, 1);
        }
      }
    }

    public Sprite GetSprite()
    {
      Sprite sprite;
      if (this.renderSystemType == RenderSystemUsing.ImageRenderer)
      {
        if (!Object.op_Implicit((Object) this.imageRenderer))
        {
          if (this.verboseDebug)
            Debug.LogError((object) ("imageRenderer is null in game object " + ((Object) this).get_name()));
          return (Sprite) null;
        }
        sprite = this.imageRenderer.get_sprite();
        if (Object.op_Implicit((Object) this.imageRenderer.get_overrideSprite()))
          sprite = this.imageRenderer.get_overrideSprite();
      }
      else
      {
        if (!Object.op_Implicit((Object) this.spriteRenderer))
        {
          if (this.verboseDebug)
            Debug.LogError((object) ("spriteRenderer is null in game object " + ((Object) this).get_name()));
          return (Sprite) null;
        }
        sprite = this.spriteRenderer.get_sprite();
      }
      if (Object.op_Implicit((Object) sprite))
        return sprite;
      if (this.verboseDebug)
        Debug.LogError((object) ("Sprite is null in game object " + ((Object) this).get_name()));
      this.isPlaying = false;
      return (Sprite) null;
    }

    private float GetParticleStartSize(float PixelsPerUnit)
    {
      float num1;
      if (this.renderSystemType == RenderSystemUsing.SpriteRenderer)
      {
        double num2 = (double) (1f / PixelsPerUnit);
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        double constant = (double) ((ParticleSystem.MinMaxCurve) ref startSize).get_constant();
        num1 = (float) (num2 * constant);
      }
      else
      {
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        num1 = ((ParticleSystem.MinMaxCurve) ref startSize).get_constant();
      }
      return num1;
    }

    private Color[] GetSpriteColorsData(Sprite sprite)
    {
      Rect rect = sprite.get_rect();
      Color[] colorArray;
      if (this.useSpritesSharingPool && Application.get_isPlaying())
        colorArray = SpritesDataPool.GetSpriteColors(sprite, (int) ((Rect) ref rect).get_position().x, (int) ((Rect) ref rect).get_position().y, (int) ((Rect) ref rect).get_size().x, (int) ((Rect) ref rect).get_size().y);
      else if (this.CacheSprites && this.mode == SpriteMode.Dynamic)
      {
        if (this.spritesSoFar.ContainsKey(sprite))
        {
          colorArray = this.spritesSoFar[sprite];
        }
        else
        {
          colorArray = sprite.get_texture().GetPixels((int) ((Rect) ref rect).get_position().x, (int) ((Rect) ref rect).get_position().y, (int) ((Rect) ref rect).get_size().x, (int) ((Rect) ref rect).get_size().y);
          this.spritesSoFar.Add(sprite, colorArray);
        }
      }
      else
        colorArray = sprite.get_texture().GetPixels((int) ((Rect) ref rect).get_position().x, (int) ((Rect) ref rect).get_position().y, (int) ((Rect) ref rect).get_size().x, (int) ((Rect) ref rect).get_size().y);
      return colorArray;
    }

    private void HackUnityCrash2017()
    {
      if (!this.forceHack || this.particlesSystem.get_isStopped())
        return;
      this.particlesSystem.Emit(1);
      this.particlesSystem.Clear();
      this.forceHack = false;
    }

    private void ForceNextUseOfHack()
    {
      this.forceHack = true;
    }

    public enum BorderEmission
    {
      Off,
      Fast,
      Precise,
    }
  }
}
