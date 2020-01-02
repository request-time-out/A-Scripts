// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.StaticSpriteEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using SpriteToParticlesAsset;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace SpriteParticleEmitter
{
  [ExecuteInEditMode]
  public class StaticSpriteEmitter : EmitterBase
  {
    [Header("Awake Options")]
    [Tooltip("Should the system cache on Awake method? - Static emission needs to be cached first, if this property is not checked the CacheSprite() method should be called by code. (Refer to manual for further explanation)")]
    public bool CacheOnAwake = true;
    protected bool hasCachingEnded;
    protected int particlesCacheCount;
    protected float particleStartSize;
    protected Vector3[] particleInitPositionsCache;
    protected Color[] particleInitColorCache;

    public override event SimpleEvent OnCacheEnded;

    public override event SimpleEvent OnAvailableToPlay;

    protected override void Awake()
    {
      base.Awake();
      if (this.PlayOnAwake)
      {
        this.isPlaying = true;
        this.CacheOnAwake = true;
      }
      if (!this.CacheOnAwake)
        return;
      this.CacheSprite(false);
    }

    public virtual void CacheSprite(bool relativeToParent = false)
    {
      this.hasCachingEnded = false;
      this.particlesCacheCount = 0;
      Sprite sprite1 = this.spriteRenderer.get_sprite();
      if (!Object.op_Implicit((Object) sprite1))
      {
        if (!this.verboseDebug)
          return;
        Debug.LogError((object) ("Unable to cache. Sprite is null in game object " + ((Object) this).get_name()));
      }
      else
      {
        float r = (float) this.EmitFromColor.r;
        float g = (float) this.EmitFromColor.g;
        float b = (float) this.EmitFromColor.b;
        Vector3 position = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_position();
        Quaternion rotation = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_rotation();
        Vector3 lossyScale = ((Component) this.spriteRenderer).get_gameObject().get_transform().get_lossyScale();
        bool flipX = this.spriteRenderer.get_flipX();
        bool flipY = this.spriteRenderer.get_flipY();
        float pixelsPerUnit = sprite1.get_pixelsPerUnit();
        if ((Object.op_Equality((Object) this.spriteRenderer, (Object) null) || Object.op_Equality((Object) this.spriteRenderer.get_sprite(), (Object) null)) && this.verboseDebug)
          Debug.LogError((object) "Sprite reference missing");
        Rect rect1 = sprite1.get_rect();
        float x1 = (float) (int) ((Rect) ref rect1).get_size().x;
        Rect rect2 = sprite1.get_rect();
        float y1 = (float) (int) ((Rect) ref rect2).get_size().y;
        this.particleStartSize = 1f / pixelsPerUnit;
        StaticSpriteEmitter staticSpriteEmitter = this;
        double particleStartSize = (double) staticSpriteEmitter.particleStartSize;
        ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref this.mainModule).get_startSize();
        double constant = (double) ((ParticleSystem.MinMaxCurve) ref startSize).get_constant();
        staticSpriteEmitter.particleStartSize = (float) (particleStartSize * constant);
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
        List<Color> colorList = new List<Color>();
        List<Vector3> vector3List = new List<Vector3>();
        for (int index = 0; (double) index < (double) num5; ++index)
        {
          Color color = colorArray[index];
          if (color.a > 0.0 && (!this.UseEmissionFromColor || FloatComparer.AreEqual(r, (float) color.r, redTolerance) && FloatComparer.AreEqual(g, (float) color.g, greenTolerance) && FloatComparer.AreEqual(b, (float) color.b, blueTolerance)))
          {
            float num3 = (float) index % x1 / pixelsPerUnit - num1;
            float num4 = (float) index / x1 / pixelsPerUnit - num2;
            if (flipX)
              num3 = (float) ((double) x1 / (double) pixelsPerUnit - (double) num3 - (double) num1 * 2.0);
            if (flipY)
              num4 = (float) ((double) y1 / (double) pixelsPerUnit - (double) num4 - (double) num2 * 2.0);
            Vector3 vector3;
            if (relativeToParent)
              vector3 = Vector3.op_Addition(Quaternion.op_Multiply(rotation, new Vector3(num3 * (float) lossyScale.x, num4 * (float) lossyScale.y, 0.0f)), position);
            else
              ((Vector3) ref vector3).\u002Ector(num3, num4, 0.0f);
            vector3List.Add(vector3);
            colorList.Add(color);
            ++this.particlesCacheCount;
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
          if (this.OnCacheEnded == null)
            return;
          this.OnCacheEnded();
        }
      }
    }

    protected virtual void Update()
    {
    }

    public override void Play()
    {
    }

    public override void Stop()
    {
    }

    public override void Pause()
    {
    }

    public override bool IsPlaying()
    {
      return this.isPlaying;
    }

    public override bool IsAvailableToPlay()
    {
      return this.hasCachingEnded;
    }

    private void DummyMethod()
    {
      if (this.OnAvailableToPlay != null)
        this.OnAvailableToPlay();
      if (this.OnCacheEnded == null)
        return;
      this.OnCacheEnded();
    }
  }
}
