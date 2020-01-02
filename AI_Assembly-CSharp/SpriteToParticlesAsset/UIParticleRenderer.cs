// Decompiled with JetBrains decompiler
// Type: SpriteToParticlesAsset.UIParticleRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpriteToParticlesAsset
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (CanvasRenderer))]
  [RequireComponent(typeof (ParticleSystem))]
  [AddComponentMenu("UI/Effects/Extensions/UI Particle System")]
  public class UIParticleRenderer : MaskableGraphic
  {
    [Tooltip("Having this enabled run the system in LateUpdate rather than in Update making it faster but less precise (more clunky)")]
    public bool fixedTime;
    private Transform _transform;
    private ParticleSystem pSystem;
    private ParticleSystem.Particle[] particles;
    private UIVertex[] _quad;
    private Vector4 imageUV;
    private ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;
    private int textureSheetAnimationFrames;
    private Vector2 textureSheetAnimationFrameSize;
    private ParticleSystemRenderer pRenderer;
    private Material currentMaterial;
    private Texture currentTexture;
    private ParticleSystem.MainModule mainModule;

    public UIParticleRenderer()
    {
      base.\u002Ector();
    }

    public virtual Texture mainTexture
    {
      get
      {
        return this.currentTexture;
      }
    }

    protected bool Initialize()
    {
      if (Object.op_Equality((Object) this._transform, (Object) null))
        this._transform = ((Component) this).get_transform();
      if (Object.op_Equality((Object) this.pSystem, (Object) null))
      {
        this.pSystem = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
        if (Object.op_Equality((Object) this.pSystem, (Object) null))
          return false;
        this.mainModule = this.pSystem.get_main();
        ParticleSystem.MainModule main = this.pSystem.get_main();
        if (((ParticleSystem.MainModule) ref main).get_maxParticles() > 14000)
          ((ParticleSystem.MainModule) ref this.mainModule).set_maxParticles(14000);
        this.pRenderer = (ParticleSystemRenderer) ((Component) this.pSystem).GetComponent<ParticleSystemRenderer>();
        if (Object.op_Inequality((Object) this.pRenderer, (Object) null))
          ((Renderer) this.pRenderer).set_enabled(false);
        if (Object.op_Equality((Object) ((Graphic) this).get_material(), (Object) null))
          ((Graphic) this).set_material(new Material(Shader.Find("UI/Particles/Additive")));
        this.currentMaterial = ((Graphic) this).get_material();
        if (Object.op_Implicit((Object) this.currentMaterial) && this.currentMaterial.HasProperty("_MainTex"))
        {
          this.currentTexture = this.currentMaterial.get_mainTexture();
          if (Object.op_Equality((Object) this.currentTexture, (Object) null))
            this.currentTexture = (Texture) Texture2D.get_whiteTexture();
        }
        ((Graphic) this).set_material(this.currentMaterial);
        ((ParticleSystem.MainModule) ref this.mainModule).set_scalingMode((ParticleSystemScalingMode) 0);
        this.particles = (ParticleSystem.Particle[]) null;
      }
      if (this.particles == null)
      {
        ParticleSystem.MainModule main = this.pSystem.get_main();
        this.particles = new ParticleSystem.Particle[((ParticleSystem.MainModule) ref main).get_maxParticles()];
      }
      this.imageUV = new Vector4(0.0f, 0.0f, 1f, 1f);
      this.textureSheetAnimation = this.pSystem.get_textureSheetAnimation();
      this.textureSheetAnimationFrames = 0;
      this.textureSheetAnimationFrameSize = Vector2.get_zero();
      if (((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_enabled())
      {
        this.textureSheetAnimationFrames = ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesX() * ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesY();
        this.textureSheetAnimationFrameSize = new Vector2(1f / (float) ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesX(), 1f / (float) ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesY());
      }
      return true;
    }

    protected virtual void Awake()
    {
      ((UIBehaviour) this).Awake();
      if (this.Initialize())
        return;
      ((Behaviour) this).set_enabled(false);
    }

    protected virtual void OnPopulateMesh(VertexHelper vh)
    {
      vh.Clear();
      if (!((Component) this).get_gameObject().get_activeInHierarchy())
        return;
      Vector2 zero1 = Vector2.get_zero();
      Vector2 zero2 = Vector2.get_zero();
      Vector2 zero3 = Vector2.get_zero();
      int particles = this.pSystem.GetParticles(this.particles);
      for (int index = 0; index < particles; ++index)
      {
        ParticleSystem.Particle particle = this.particles[index];
        Vector2 vector2_1 = Vector2.op_Implicit(((ParticleSystem.MainModule) ref this.mainModule).get_simulationSpace() != null ? this._transform.InverseTransformPoint(((ParticleSystem.Particle) ref particle).get_position()) : ((ParticleSystem.Particle) ref particle).get_position());
        float num1 = (float) (-(double) ((ParticleSystem.Particle) ref particle).get_rotation() * (Math.PI / 180.0));
        float num2 = num1 + 1.570796f;
        Color32 currentColor = ((ParticleSystem.Particle) ref particle).GetCurrentColor(this.pSystem);
        float num3 = ((ParticleSystem.Particle) ref particle).GetCurrentSize(this.pSystem) * 0.5f;
        if (((ParticleSystem.MainModule) ref this.mainModule).get_scalingMode() == 2)
          vector2_1 = Vector2.op_Division(vector2_1, ((Graphic) this).get_canvas().get_scaleFactor());
        Vector4 imageUv = this.imageUV;
        if (((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_enabled())
        {
          ParticleSystem.MinMaxCurve frameOverTime = ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_frameOverTime();
          float num4 = Mathf.Repeat(((ParticleSystem.MinMaxCurve) ref frameOverTime).get_curveMin().Evaluate((float) (1.0 - (double) ((ParticleSystem.Particle) ref particle).get_remainingLifetime() / (double) ((ParticleSystem.Particle) ref particle).get_startLifetime())) * (float) ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_cycleCount(), 1f);
          int num5 = 0;
          ParticleSystemAnimationType animation = ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_animation();
          if (animation != null)
          {
            if (animation == 1)
              num5 = Mathf.FloorToInt(num4 * (float) ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesX()) + ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_rowIndex() * ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesX();
          }
          else
            num5 = Mathf.FloorToInt(num4 * (float) this.textureSheetAnimationFrames);
          int num6 = num5 % this.textureSheetAnimationFrames;
          imageUv.x = (__Null) ((double) (num6 % ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesX()) * this.textureSheetAnimationFrameSize.x);
          imageUv.y = (__Null) ((double) Mathf.FloorToInt((float) (num6 / ((ParticleSystem.TextureSheetAnimationModule) ref this.textureSheetAnimation).get_numTilesX())) * this.textureSheetAnimationFrameSize.y);
          imageUv.z = imageUv.x + this.textureSheetAnimationFrameSize.x;
          imageUv.w = imageUv.y + this.textureSheetAnimationFrameSize.y;
        }
        zero1.x = imageUv.x;
        zero1.y = imageUv.y;
        this._quad[0] = (UIVertex) UIVertex.simpleVert;
        this._quad[0].color = (__Null) currentColor;
        this._quad[0].uv0 = (__Null) zero1;
        zero1.x = imageUv.x;
        zero1.y = imageUv.w;
        this._quad[1] = (UIVertex) UIVertex.simpleVert;
        this._quad[1].color = (__Null) currentColor;
        this._quad[1].uv0 = (__Null) zero1;
        zero1.x = imageUv.z;
        zero1.y = imageUv.w;
        this._quad[2] = (UIVertex) UIVertex.simpleVert;
        this._quad[2].color = (__Null) currentColor;
        this._quad[2].uv0 = (__Null) zero1;
        zero1.x = imageUv.z;
        zero1.y = imageUv.y;
        this._quad[3] = (UIVertex) UIVertex.simpleVert;
        this._quad[3].color = (__Null) currentColor;
        this._quad[3].uv0 = (__Null) zero1;
        if ((double) num1 == 0.0)
        {
          zero2.x = (__Null) (vector2_1.x - (double) num3);
          zero2.y = (__Null) (vector2_1.y - (double) num3);
          zero3.x = (__Null) (vector2_1.x + (double) num3);
          zero3.y = (__Null) (vector2_1.y + (double) num3);
          zero1.x = zero2.x;
          zero1.y = zero2.y;
          this._quad[0].position = (__Null) Vector2.op_Implicit(zero1);
          zero1.x = zero2.x;
          zero1.y = zero3.y;
          this._quad[1].position = (__Null) Vector2.op_Implicit(zero1);
          zero1.x = zero3.x;
          zero1.y = zero3.y;
          this._quad[2].position = (__Null) Vector2.op_Implicit(zero1);
          zero1.x = zero3.x;
          zero1.y = zero2.y;
          this._quad[3].position = (__Null) Vector2.op_Implicit(zero1);
        }
        else
        {
          Vector2 vector2_2 = Vector2.op_Multiply(new Vector2(Mathf.Cos(num1), Mathf.Sin(num1)), num3);
          Vector2 vector2_3 = Vector2.op_Multiply(new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)), num3);
          this._quad[0].position = (__Null) Vector2.op_Implicit(Vector2.op_Subtraction(Vector2.op_Subtraction(vector2_1, vector2_2), vector2_3));
          this._quad[1].position = (__Null) Vector2.op_Implicit(Vector2.op_Addition(Vector2.op_Subtraction(vector2_1, vector2_2), vector2_3));
          this._quad[2].position = (__Null) Vector2.op_Implicit(Vector2.op_Addition(Vector2.op_Addition(vector2_1, vector2_2), vector2_3));
          this._quad[3].position = (__Null) Vector2.op_Implicit(Vector2.op_Subtraction(Vector2.op_Addition(vector2_1, vector2_2), vector2_3));
        }
        vh.AddUIVertexQuad(this._quad);
      }
    }

    private void Update()
    {
      if (this.fixedTime || !Application.get_isPlaying())
        return;
      this.pSystem.Simulate(Time.get_unscaledDeltaTime(), false, false, true);
      ((Graphic) this).SetAllDirty();
      if ((!Object.op_Inequality((Object) this.currentMaterial, (Object) null) || !Object.op_Inequality((Object) this.currentTexture, (Object) this.currentMaterial.get_mainTexture())) && (!Object.op_Inequality((Object) ((Graphic) this).get_material(), (Object) null) || !Object.op_Inequality((Object) this.currentMaterial, (Object) null) || !Object.op_Inequality((Object) ((Graphic) this).get_material().get_shader(), (Object) this.currentMaterial.get_shader())))
        return;
      this.pSystem = (ParticleSystem) null;
      this.Initialize();
    }

    private void LateUpdate()
    {
      if (!Application.get_isPlaying())
        ((Graphic) this).SetAllDirty();
      else if (this.fixedTime)
      {
        this.pSystem.Simulate(Time.get_unscaledDeltaTime(), false, false, true);
        ((Graphic) this).SetAllDirty();
        if (Object.op_Inequality((Object) this.currentMaterial, (Object) null) && Object.op_Inequality((Object) this.currentTexture, (Object) this.currentMaterial.get_mainTexture()) || Object.op_Inequality((Object) ((Graphic) this).get_material(), (Object) null) && Object.op_Inequality((Object) this.currentMaterial, (Object) null) && Object.op_Inequality((Object) ((Graphic) this).get_material().get_shader(), (Object) this.currentMaterial.get_shader()))
        {
          this.pSystem = (ParticleSystem) null;
          this.Initialize();
        }
      }
      if (Object.op_Equality((Object) ((Graphic) this).get_material(), (Object) this.currentMaterial))
        return;
      this.pSystem = (ParticleSystem) null;
      this.Initialize();
    }
  }
}
