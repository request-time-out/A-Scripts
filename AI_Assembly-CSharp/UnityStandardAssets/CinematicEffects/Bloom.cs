// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.CinematicEffects.Bloom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (Camera))]
  [AddComponentMenu("Image Effects/Cinematic/Bloom")]
  [ImageEffectAllowedInSceneView]
  public class Bloom : MonoBehaviour
  {
    [SerializeField]
    public Bloom.Settings settings;
    [SerializeField]
    [HideInInspector]
    private Shader m_Shader;
    private Material m_Material;
    private const int kMaxIterations = 16;
    private RenderTexture[] m_blurBuffer1;
    private RenderTexture[] m_blurBuffer2;
    private int m_Threshold;
    private int m_Curve;
    private int m_PrefilterOffs;
    private int m_SampleScale;
    private int m_Intensity;
    private int m_DirtTex;
    private int m_DirtIntensity;
    private int m_BaseTex;

    public Bloom()
    {
      base.\u002Ector();
    }

    public Shader shader
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Shader, (Object) null))
          this.m_Shader = Shader.Find("Hidden/Image Effects/Cinematic/Bloom");
        return this.m_Shader;
      }
    }

    public Material material
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Material, (Object) null))
          this.m_Material = ImageEffectHelper.CheckShaderAndCreateMaterial(this.shader);
        return this.m_Material;
      }
    }

    private void Awake()
    {
      this.m_Threshold = Shader.PropertyToID("_Threshold");
      this.m_Curve = Shader.PropertyToID("_Curve");
      this.m_PrefilterOffs = Shader.PropertyToID("_PrefilterOffs");
      this.m_SampleScale = Shader.PropertyToID("_SampleScale");
      this.m_Intensity = Shader.PropertyToID("_Intensity");
      this.m_DirtTex = Shader.PropertyToID("_DirtTex");
      this.m_DirtIntensity = Shader.PropertyToID("_DirtIntensity");
      this.m_BaseTex = Shader.PropertyToID("_BaseTex");
    }

    private void OnEnable()
    {
      if (ImageEffectHelper.IsSupported(this.shader, true, false, (MonoBehaviour) this))
        return;
      ((Behaviour) this).set_enabled(false);
    }

    private void OnDisable()
    {
      if (Object.op_Inequality((Object) this.m_Material, (Object) null))
        Object.DestroyImmediate((Object) this.m_Material);
      this.m_Material = (Material) null;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      bool isMobilePlatform = Application.get_isMobilePlatform();
      int width = ((Texture) source).get_width();
      int height = ((Texture) source).get_height();
      if (!this.settings.highQuality)
      {
        width /= 2;
        height /= 2;
      }
      RenderTextureFormat renderTextureFormat = !isMobilePlatform ? (RenderTextureFormat) 9 : (RenderTextureFormat) 7;
      float num1 = (float) ((double) Mathf.Log((float) height, 2f) + (double) this.settings.radius - 8.0);
      int num2 = (int) num1;
      int num3 = Mathf.Clamp(num2, 1, 16);
      float thresholdLinear = this.settings.thresholdLinear;
      this.material.SetFloat(this.m_Threshold, thresholdLinear);
      float num4 = (float) ((double) thresholdLinear * (double) this.settings.softKnee + 9.99999974737875E-06);
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(thresholdLinear - num4, num4 * 2f, 0.25f / num4);
      this.material.SetVector(this.m_Curve, Vector4.op_Implicit(vector3));
      this.material.SetFloat(this.m_PrefilterOffs, this.settings.highQuality || !this.settings.antiFlicker ? 0.0f : -0.5f);
      this.material.SetFloat(this.m_SampleScale, 0.5f + num1 - (float) num2);
      this.material.SetFloat(this.m_Intensity, Mathf.Max(0.0f, this.settings.intensity));
      bool flag = false;
      if (Object.op_Inequality((Object) this.settings.dirtTexture, (Object) null))
      {
        this.material.SetTexture(this.m_DirtTex, this.settings.dirtTexture);
        this.material.SetFloat(this.m_DirtIntensity, this.settings.dirtIntensity);
        flag = true;
      }
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, renderTextureFormat);
      Graphics.Blit((Texture) source, temporary, this.material, !this.settings.antiFlicker ? 0 : 1);
      RenderTexture renderTexture1 = temporary;
      for (int index = 0; index < num3; ++index)
      {
        this.m_blurBuffer1[index] = RenderTexture.GetTemporary(((Texture) renderTexture1).get_width() / 2, ((Texture) renderTexture1).get_height() / 2, 0, renderTextureFormat);
        Graphics.Blit((Texture) renderTexture1, this.m_blurBuffer1[index], this.material, index != 0 ? 4 : (!this.settings.antiFlicker ? 2 : 3));
        renderTexture1 = this.m_blurBuffer1[index];
      }
      for (int index = num3 - 2; index >= 0; --index)
      {
        RenderTexture renderTexture2 = this.m_blurBuffer1[index];
        this.material.SetTexture(this.m_BaseTex, (Texture) renderTexture2);
        this.m_blurBuffer2[index] = RenderTexture.GetTemporary(((Texture) renderTexture2).get_width(), ((Texture) renderTexture2).get_height(), 0, renderTextureFormat);
        Graphics.Blit((Texture) renderTexture1, this.m_blurBuffer2[index], this.material, !this.settings.highQuality ? 5 : 6);
        renderTexture1 = this.m_blurBuffer2[index];
      }
      int num5 = (!flag ? 7 : 9) + (!this.settings.highQuality ? 0 : 1);
      this.material.SetTexture(this.m_BaseTex, (Texture) source);
      Graphics.Blit((Texture) renderTexture1, destination, this.material, num5);
      for (int index = 0; index < 16; ++index)
      {
        if (Object.op_Inequality((Object) this.m_blurBuffer1[index], (Object) null))
          RenderTexture.ReleaseTemporary(this.m_blurBuffer1[index]);
        if (Object.op_Inequality((Object) this.m_blurBuffer2[index], (Object) null))
          RenderTexture.ReleaseTemporary(this.m_blurBuffer2[index]);
        this.m_blurBuffer1[index] = (RenderTexture) null;
        this.m_blurBuffer2[index] = (RenderTexture) null;
      }
      RenderTexture.ReleaseTemporary(temporary);
    }

    [Serializable]
    public struct Settings
    {
      [SerializeField]
      [Tooltip("Filters out pixels under this level of brightness.")]
      public float threshold;
      [SerializeField]
      [Range(0.0f, 1f)]
      [Tooltip("Makes transition between under/over-threshold gradual.")]
      public float softKnee;
      [SerializeField]
      [Range(1f, 7f)]
      [Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
      public float radius;
      [SerializeField]
      [Tooltip("Blend factor of the result image.")]
      public float intensity;
      [SerializeField]
      [Tooltip("Controls filter quality and buffer resolution.")]
      public bool highQuality;
      [SerializeField]
      [Tooltip("Reduces flashing noise with an additional filter.")]
      public bool antiFlicker;
      [Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
      public Texture dirtTexture;
      [Min(0.0f)]
      [Tooltip("Amount of lens dirtiness.")]
      public float dirtIntensity;

      public float thresholdGamma
      {
        set
        {
          this.threshold = value;
        }
        get
        {
          return Mathf.Max(0.0f, this.threshold);
        }
      }

      public float thresholdLinear
      {
        set
        {
          this.threshold = Mathf.LinearToGammaSpace(value);
        }
        get
        {
          return Mathf.GammaToLinearSpace(this.thresholdGamma);
        }
      }

      public static Bloom.Settings defaultSettings
      {
        get
        {
          return new Bloom.Settings()
          {
            threshold = 0.9f,
            softKnee = 0.5f,
            radius = 2f,
            intensity = 0.7f,
            highQuality = true,
            antiFlicker = false,
            dirtTexture = (Texture) null,
            dirtIntensity = 2.5f
          };
        }
      }
    }
  }
}
