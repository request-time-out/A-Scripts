// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DeepSky.Haze
{
  [ExecuteInEditMode]
  [AddComponentMenu("DeepSky Haze/View", 1)]
  public class DS_HazeView : MonoBehaviour
  {
    private static string kClearRadianceCmdBufferName = "DS_Haze_ClearRadiance";
    private static string kShadowCascadesCmdBufferName = "DS_Haze_ShadowCascadesCopy";
    private static string kDirectionalLightCmdBufferName = "DS_Haze_DirectLight";
    private static string kRenderLightVolumeCmdBufferName = "DS_Haze_RenderLightVolume";
    private static string kPreviousDepthTargetName = "DS_Haze_PreviousDepthTarget";
    private static string kRadianceTarget01Name = "DS_Haze_RadianceTarget_01";
    private static string kRadianceTarget02Name = "DS_Haze_RadianceTarget_02";
    private static Shader kShader;
    [SerializeField]
    private bool m_OverrideTime;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float m_Time;
    [SerializeField]
    private bool m_OverrideContextAsset;
    [SerializeField]
    private DS_HazeContextAsset m_Context;
    [SerializeField]
    private bool m_OverrideContextVariant;
    [SerializeField]
    private int m_ContextItemIndex;
    [SerializeField]
    private Light m_DirectLight;
    [SerializeField]
    private bool m_RenderAtmosphereVolumetrics;
    [SerializeField]
    private bool m_RenderLocalVolumetrics;
    [SerializeField]
    private bool m_TemporalReprojection;
    [SerializeField]
    private DS_HazeView.SizeFactor m_DownsampleFactor;
    [SerializeField]
    private DS_HazeView.VolumeSamples m_VolumeSamples;
    [SerializeField]
    [Range(100f, 5000f)]
    private int m_GaussianDepthFalloff;
    [SerializeField]
    [Range(0.0f, 0.5f)]
    private float m_UpsampleDepthThreshold;
    [SerializeField]
    [Range(0.001f, 1f)]
    private float m_TemporalRejectionScale;
    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float m_TemporalBlendFactor;
    private ShadowProjection m_ShadowProjectionType;
    [SerializeField]
    private bool m_ApplyAirToSkybox;
    [SerializeField]
    private bool m_ApplyHazeToSkybox;
    [SerializeField]
    private bool m_ApplyFogExtinctionToSkybox;
    [SerializeField]
    private bool m_ApplyFogLightingToSkybox;
    [SerializeField]
    private bool m_ShowTemporalRejection;
    [SerializeField]
    private bool m_ShowUpsampleThreshold;
    private Camera m_Camera;
    private RenderTexture m_PerFrameRadianceTarget;
    private RenderTexture m_RadianceTarget_01;
    private RenderTexture m_RadianceTarget_02;
    private RenderTexture m_CurrentRadianceTarget;
    private RenderTexture m_PreviousRadianceTarget;
    private RenderTexture m_PreviousDepthTarget;
    private CommandBuffer m_ShadowCascadesCmdBuffer;
    private CommandBuffer m_DirectionalLightCmdBuffer;
    private CommandBuffer m_ClearRadianceCmdBuffer;
    private CommandBuffer m_RenderNonShadowVolumes;
    private Material m_Material;
    private Matrix4x4 m_PreviousViewProjMatrix;
    private Matrix4x4 m_PreviousInvViewProjMatrix;
    private float m_InterleavedOffsetIndex;
    private int m_X;
    private int m_Y;
    private RenderingPath m_PreviousRenderPath;
    private ColorSpace m_ColourSpace;
    private List<DS_HazeLightVolume> m_PerFrameLightVolumes;
    private List<DS_HazeLightVolume> m_PerFrameShadowLightVolumes;
    private Dictionary<Light, CommandBuffer> m_LightVolumeCmdBuffers;

    public DS_HazeView()
    {
      base.\u002Ector();
    }

    public bool OverrideTime
    {
      get
      {
        return this.m_OverrideTime;
      }
      set
      {
        this.m_OverrideTime = value;
        if (!value || !this.m_OverrideContextVariant)
          return;
        this.m_OverrideContextVariant = false;
      }
    }

    public float Time
    {
      get
      {
        return this.m_Time;
      }
      set
      {
        this.m_Time = value;
      }
    }

    public bool OverrideContextAsset
    {
      get
      {
        return this.m_OverrideContextAsset;
      }
      set
      {
        this.m_OverrideContextAsset = value;
      }
    }

    public DS_HazeContextAsset ContextAsset
    {
      get
      {
        return this.m_Context;
      }
      set
      {
        this.m_Context = value;
      }
    }

    public bool OverrideContextVariant
    {
      get
      {
        return this.m_OverrideContextVariant;
      }
      set
      {
        this.m_OverrideContextVariant = value;
        if (!value || !this.m_OverrideTime)
          return;
        this.m_OverrideTime = false;
      }
    }

    public int ContextItemIndex
    {
      get
      {
        return this.m_ContextItemIndex;
      }
      set
      {
        this.m_ContextItemIndex = value <= 0 ? 0 : value;
      }
    }

    public Light DirectLight
    {
      get
      {
        return this.m_DirectLight;
      }
      set
      {
        this.m_DirectLight = value;
      }
    }

    public Vector2 RadianceTargetSize
    {
      get
      {
        return new Vector2((float) this.m_X, (float) this.m_Y);
      }
    }

    public int SampleCount
    {
      get
      {
        switch (this.m_VolumeSamples)
        {
          case DS_HazeView.VolumeSamples.x16:
            return 16;
          case DS_HazeView.VolumeSamples.x24:
            return 24;
          case DS_HazeView.VolumeSamples.x32:
            return 32;
          default:
            return 16;
        }
      }
    }

    public int DownSampleFactor
    {
      get
      {
        return this.m_DownsampleFactor == DS_HazeView.SizeFactor.Half ? 2 : 4;
      }
    }

    public bool RenderAtmosphereVolumetrics
    {
      get
      {
        return this.m_RenderAtmosphereVolumetrics;
      }
      set
      {
        this.m_RenderAtmosphereVolumetrics = value;
        this.SetTemporalKeywords();
      }
    }

    public bool RenderLocalVolumetrics
    {
      get
      {
        return this.m_RenderLocalVolumetrics;
      }
      set
      {
        this.m_RenderLocalVolumetrics = value;
        this.SetTemporalKeywords();
      }
    }

    public bool TemporalReprojection
    {
      get
      {
        return this.m_TemporalReprojection;
      }
      set
      {
        this.m_TemporalReprojection = value;
        this.SetTemporalKeywords();
      }
    }

    public bool WillRenderWithTemporalReprojection
    {
      get
      {
        return this.m_TemporalReprojection & (this.m_RenderAtmosphereVolumetrics | this.m_RenderLocalVolumetrics);
      }
    }

    public int AntiAliasingLevel()
    {
      int num = 1;
      if (this.m_Camera.get_actualRenderingPath() == 1 && this.m_Camera.get_allowMSAA() && QualitySettings.get_antiAliasing() > 0)
        num = QualitySettings.get_antiAliasing();
      return num;
    }

    private bool CheckHasSystemSupport()
    {
      if (!SystemInfo.get_supportsImageEffects())
      {
        Debug.LogError((object) "DeepSky::DS_HazeView: Image effects are not supported on this platform.");
        ((Behaviour) this).set_enabled(false);
        return false;
      }
      if (SystemInfo.get_graphicsShaderLevel() < 30)
      {
        Debug.LogError((object) "DeepSky::DS_HazeView: Minimum required shader model (3.0) is not supported on this platform.");
        ((Behaviour) this).set_enabled(false);
        return false;
      }
      if (this.m_Camera.get_allowHDR() && !SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 2))
      {
        Debug.LogError((object) "DeepSky::DS_HazeView: ARGBHalf render texture format is not supported on this platform.");
        ((Behaviour) this).set_enabled(false);
        return false;
      }
      if (SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 14))
        return true;
      Debug.LogError((object) "DeepSky::DS_HazeView: RFloat render texture format is not supported on this platform.");
      ((Behaviour) this).set_enabled(false);
      return false;
    }

    private void SetMaterialFromContext(DS_HazeContextItem ctx)
    {
      if (this.WillRenderWithTemporalReprojection)
      {
        this.m_InterleavedOffsetIndex += 1f / 16f;
        if (Mathf.Approximately(this.m_InterleavedOffsetIndex, 1f))
          this.m_InterleavedOffsetIndex = 0.0f;
      }
      float num1 = 1f;
      float num2 = 1f;
      float num3 = 1f;
      switch (DS_HazeCore.Instance.HeightFalloff)
      {
        case DS_HazeCore.HeightFalloffType.Exponential:
          float num4 = Mathf.Abs((float) ((Component) this).get_transform().get_position().y);
          num1 = Mathf.Exp(-ctx.m_AirDensityHeightFalloff * num4);
          num2 = Mathf.Exp(-ctx.m_HazeDensityHeightFalloff * num4);
          num3 = Mathf.Exp((float) (-(double) ctx.m_FogDensityHeightFalloff * ((double) num4 - (double) ctx.m_FogStartHeight)));
          break;
        case DS_HazeCore.HeightFalloffType.None:
          num1 = 1f;
          num2 = 1f;
          num3 = 1f;
          break;
      }
      Vector3 vector3_1 = Vector3.op_Multiply(ctx.m_AirScatteringScale, new Vector3(0.00116f, 0.0027f, 0.00662f));
      float num5 = ctx.m_HazeScatteringScale * 0.0021f;
      float fogScatteringScale = ctx.m_FogScatteringScale;
      float num6 = ctx.m_FogExtinctionScale * 0.01f;
      Vector4 vector4_1;
      ((Vector4) ref vector4_1).\u002Ector(ctx.m_AirDensityHeightFalloff, ctx.m_HazeDensityHeightFalloff, 0.0f, ctx.m_HazeScatteringDirection);
      Vector4 vector4_2;
      ((Vector4) ref vector4_2).\u002Ector(num5, !this.m_RenderAtmosphereVolumetrics ? 0.0f : ctx.m_HazeSecondaryScatteringRatio, fogScatteringScale, num6);
      Vector4 vector4_3;
      ((Vector4) ref vector4_3).\u002Ector(num1, num2, num3, 0.0f);
      Vector4 vector4_4;
      ((Vector4) ref vector4_4).\u002Ector(ctx.m_FogStartDistance, ctx.m_FogDensityHeightFalloff, ctx.m_FogOpacity, ctx.m_FogScatteringDirection);
      Vector4 vector4_5;
      ((Vector4) ref vector4_5).\u002Ector((float) this.m_GaussianDepthFalloff, this.m_UpsampleDepthThreshold * 0.01f, this.m_TemporalRejectionScale, this.m_TemporalBlendFactor);
      this.m_Material.SetVector("_SamplingParams", vector4_5);
      this.m_Material.SetVector("_InterleavedOffset", new Vector4(this.m_InterleavedOffsetIndex, 0.0f, 0.0f, 0.0f));
      this.m_Material.SetMatrix("_PreviousViewProjMatrix", this.m_PreviousViewProjMatrix);
      this.m_Material.SetMatrix("_PreviousInvViewProjMatrix", this.m_PreviousInvViewProjMatrix);
      Shader.SetGlobalVector("_DS_BetaParams", vector4_2);
      Shader.SetGlobalVector("_DS_RBetaS", Vector4.op_Implicit(vector3_1));
      Shader.SetGlobalVector("_DS_AirHazeParams", vector4_1);
      Shader.SetGlobalVector("_DS_FogParams", vector4_4);
      Shader.SetGlobalVector("_DS_InitialDensityParams", vector4_3);
      Vector3 vector3_2;
      Color color1;
      if (Object.op_Implicit((Object) this.m_DirectLight))
      {
        vector3_2 = Vector3.op_UnaryNegation(((Component) this.m_DirectLight).get_transform().get_forward());
        Color color2 = this.m_DirectLight.get_color();
        color1 = Color.op_Multiply(((Color) ref color2).get_linear(), this.m_DirectLight.get_intensity());
        Shader.SetGlobalColor("_DS_FogAmbientLight", Color.op_Multiply(((Color) ref ctx.m_FogAmbientColour).get_linear(), this.m_DirectLight.get_intensity()));
        Shader.SetGlobalColor("_DS_FogDirectLight", Color.op_Multiply(((Color) ref ctx.m_FogLightColour).get_linear(), this.m_DirectLight.get_intensity()));
      }
      else
      {
        vector3_2 = Vector3.get_up();
        color1 = Color.get_white();
        Shader.SetGlobalColor("_DS_FogAmbientLight", ((Color) ref ctx.m_FogAmbientColour).get_linear());
        Shader.SetGlobalColor("_DS_FogDirectLight", ((Color) ref ctx.m_FogLightColour).get_linear());
      }
      Shader.SetGlobalVector("_DS_LightDirection", Vector4.op_Implicit(vector3_2));
      Shader.SetGlobalVector("_DS_LightColour", Color.op_Implicit(color1));
    }

    private void SetGlobalParamsToNull()
    {
      Shader.SetGlobalVector("_DS_BetaParams", Vector4.get_zero());
      Shader.SetGlobalVector("_DS_RBetaS", Vector4.get_zero());
    }

    public void SetDebugKeywords()
    {
      if (this.m_ShowTemporalRejection)
        this.m_Material.EnableKeyword("SHOW_TEMPORAL_REJECTION");
      else
        this.m_Material.DisableKeyword("SHOW_TEMPORAL_REJECTION");
      if (this.m_ShowUpsampleThreshold)
        this.m_Material.EnableKeyword("SHOW_UPSAMPLE_THRESHOLD");
      else
        this.m_Material.DisableKeyword("SHOW_UPSAMPLE_THRESHOLD");
    }

    public void SetSkyboxKeywords()
    {
      if (this.m_ApplyAirToSkybox)
        this.m_Material.EnableKeyword("DS_HAZE_APPLY_RAYLEIGH");
      else
        this.m_Material.DisableKeyword("DS_HAZE_APPLY_RAYLEIGH");
      if (this.m_ApplyHazeToSkybox)
        this.m_Material.EnableKeyword("DS_HAZE_APPLY_MIE");
      else
        this.m_Material.DisableKeyword("DS_HAZE_APPLY_MIE");
      if (this.m_ApplyFogExtinctionToSkybox)
        this.m_Material.EnableKeyword("DS_HAZE_APPLY_FOG_EXTINCTION");
      else
        this.m_Material.DisableKeyword("DS_HAZE_APPLY_FOG_EXTINCTION");
      if (this.m_ApplyFogLightingToSkybox)
        this.m_Material.EnableKeyword("DS_HAZE_APPLY_FOG_RADIANCE");
      else
        this.m_Material.DisableKeyword("DS_HAZE_APPLY_FOG_RADIANCE");
    }

    public void SetTemporalKeywords()
    {
      if (this.WillRenderWithTemporalReprojection)
      {
        this.m_Material.EnableKeyword("DS_HAZE_TEMPORAL");
      }
      else
      {
        this.m_Material.DisableKeyword("DS_HAZE_TEMPORAL");
        if (this.m_ShowTemporalRejection)
        {
          this.m_ShowTemporalRejection = false;
          this.m_Material.DisableKeyword("SHOW_TEMPORAL_REJECTION");
        }
        if (Object.op_Implicit((Object) this.m_RadianceTarget_01))
        {
          this.m_RadianceTarget_01.Release();
          Object.DestroyImmediate((Object) this.m_RadianceTarget_01);
          this.m_RadianceTarget_01 = (RenderTexture) null;
        }
        if (Object.op_Implicit((Object) this.m_RadianceTarget_02))
        {
          this.m_RadianceTarget_02.Release();
          Object.DestroyImmediate((Object) this.m_RadianceTarget_02);
          this.m_RadianceTarget_02 = (RenderTexture) null;
        }
        if (!Object.op_Implicit((Object) this.m_PreviousDepthTarget))
          return;
        this.m_PreviousDepthTarget.Release();
        Object.DestroyImmediate((Object) this.m_PreviousDepthTarget);
        this.m_PreviousDepthTarget = (RenderTexture) null;
      }
    }

    private void SetShaderKeyWords()
    {
      if (this.m_ShadowProjectionType == null)
        this.m_Material.EnableKeyword("SHADOW_PROJ_CLOSE");
      else if (this.m_ShadowProjectionType == 1)
        this.m_Material.DisableKeyword("SHADOW_PROJ_CLOSE");
      if (!Object.op_Inequality((Object) DS_HazeCore.Instance, (Object) null))
        return;
      switch (DS_HazeCore.Instance.HeightFalloff)
      {
        case DS_HazeCore.HeightFalloffType.Exponential:
          this.m_Material.DisableKeyword("DS_HAZE_HEIGHT_FALLOFF_NONE");
          break;
        case DS_HazeCore.HeightFalloffType.None:
          this.m_Material.EnableKeyword("DS_HAZE_HEIGHT_FALLOFF_NONE");
          break;
        default:
          this.m_Material.EnableKeyword("DS_HAZE_HEIGHT_FALLOFF_NONE");
          break;
      }
    }

    private void OnEnable()
    {
      this.SetGlobalParamsToNull();
      this.m_Camera = (Camera) ((Component) this).GetComponent<Camera>();
      if (!Object.op_Implicit((Object) this.m_Camera))
      {
        Debug.LogError((object) ("DeepSky::DS_HazeView: GameObject '" + ((Object) ((Component) this).get_gameObject()).get_name() + "' does not have a camera component!"));
        ((Behaviour) this).set_enabled(false);
      }
      else if (!this.CheckHasSystemSupport())
      {
        ((Behaviour) this).set_enabled(false);
      }
      else
      {
        if (Object.op_Equality((Object) DS_HazeView.kShader, (Object) null))
          DS_HazeView.kShader = (Shader) Resources.Load<Shader>("DS_Haze");
        if (Object.op_Equality((Object) this.m_Material, (Object) null))
        {
          this.m_Material = new Material(DS_HazeView.kShader);
          ((Object) this.m_Material).set_hideFlags((HideFlags) 61);
        }
        if (this.m_Camera.get_actualRenderingPath() == 1 && (this.m_Camera.get_depthTextureMode() & 1) != 1)
          this.m_Camera.set_depthTextureMode((DepthTextureMode) (this.m_Camera.get_depthTextureMode() | 1));
        if (this.m_RenderNonShadowVolumes == null)
        {
          CommandBuffer[] commandBuffers = this.m_Camera.GetCommandBuffers((CameraEvent) 12);
          bool flag = false;
          foreach (CommandBuffer commandBuffer in commandBuffers)
          {
            if (commandBuffer.get_name() == DS_HazeView.kRenderLightVolumeCmdBufferName)
            {
              this.m_RenderNonShadowVolumes = commandBuffer;
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            this.m_RenderNonShadowVolumes = new CommandBuffer();
            this.m_RenderNonShadowVolumes.set_name(DS_HazeView.kRenderLightVolumeCmdBufferName);
            this.m_Camera.AddCommandBuffer((CameraEvent) 12, this.m_RenderNonShadowVolumes);
          }
        }
        this.m_CurrentRadianceTarget = this.m_RadianceTarget_01;
        this.m_PreviousRadianceTarget = this.m_RadianceTarget_02;
        this.SetSkyboxKeywords();
        this.SetDebugKeywords();
        this.m_ColourSpace = QualitySettings.get_activeColorSpace();
        this.m_PreviousRenderPath = this.m_Camera.get_actualRenderingPath();
      }
    }

    private void CreateRadianceTarget(string name, out RenderTexture radianceTarget)
    {
      radianceTarget = !this.m_Camera.get_allowHDR() ? new RenderTexture(this.m_Camera.get_pixelWidth(), this.m_Camera.get_pixelHeight(), 0, (RenderTextureFormat) 0) : new RenderTexture(this.m_Camera.get_pixelWidth(), this.m_Camera.get_pixelHeight(), 0, (RenderTextureFormat) 2);
      ((Object) radianceTarget).set_name(name);
      radianceTarget.set_antiAliasing(this.AntiAliasingLevel());
      radianceTarget.set_useMipMap(false);
      ((Object) radianceTarget).set_hideFlags((HideFlags) 61);
      ((Texture) radianceTarget).set_filterMode((FilterMode) 0);
    }

    private void CreateDepthTarget(string name, out RenderTexture depthTarget, bool downsample = false)
    {
      depthTarget = new RenderTexture(!downsample ? this.m_Camera.get_pixelWidth() : this.m_X, !downsample ? this.m_Camera.get_pixelHeight() : this.m_Y, 0, (RenderTextureFormat) 14, (RenderTextureReadWrite) 1);
      ((Object) depthTarget).set_name(name);
      depthTarget.set_antiAliasing(1);
      depthTarget.set_useMipMap(false);
      ((Object) depthTarget).set_hideFlags((HideFlags) 61);
      ((Texture) depthTarget).set_filterMode((FilterMode) 0);
    }

    private bool CameraHasClearRadianceCmdBuffer(out CommandBuffer foundCmd)
    {
      foreach (CommandBuffer commandBuffer in this.m_Camera.get_actualRenderingPath() != 3 ? this.m_Camera.GetCommandBuffers((this.m_Camera.get_depthTextureMode() & 2) != 2 ? (CameraEvent) 0 : (CameraEvent) 2) : this.m_Camera.GetCommandBuffers((CameraEvent) 4))
      {
        if (commandBuffer.get_name() == DS_HazeView.kClearRadianceCmdBufferName)
        {
          foundCmd = commandBuffer;
          return true;
        }
      }
      foundCmd = (CommandBuffer) null;
      return false;
    }

    private CommandBuffer LightHasCascadesCopyCmdBuffer()
    {
      foreach (CommandBuffer commandBuffer in this.m_DirectLight.GetCommandBuffers((LightEvent) 1))
      {
        if (commandBuffer.get_name() == DS_HazeView.kShadowCascadesCmdBufferName)
          return commandBuffer;
      }
      return (CommandBuffer) null;
    }

    private CommandBuffer LightHasRenderCmdBuffer()
    {
      foreach (CommandBuffer commandBuffer in this.m_DirectLight.GetCommandBuffers((LightEvent) 3))
      {
        if (commandBuffer.get_name() == DS_HazeView.kDirectionalLightCmdBufferName)
          return commandBuffer;
      }
      return (CommandBuffer) null;
    }

    public void RemoveCommandBufferFromLight(Light light)
    {
      CommandBuffer[] commandBuffers1 = light.GetCommandBuffers((LightEvent) 1);
      for (int index = 0; index < commandBuffers1.Length; ++index)
      {
        if (commandBuffers1[index].get_name() == DS_HazeView.kShadowCascadesCmdBufferName)
        {
          light.RemoveCommandBuffer((LightEvent) 1, commandBuffers1[index]);
          break;
        }
      }
      CommandBuffer[] commandBuffers2 = light.GetCommandBuffers((LightEvent) 3);
      for (int index = 0; index < commandBuffers2.Length; ++index)
      {
        if (commandBuffers2[index].get_name() == DS_HazeView.kDirectionalLightCmdBufferName)
        {
          light.RemoveCommandBuffer((LightEvent) 3, commandBuffers2[index]);
          break;
        }
      }
    }

    private void RenderPathChanged()
    {
      if (this.m_Camera.get_actualRenderingPath() == 1 && (this.m_Camera.get_depthTextureMode() & 1) != 1)
        this.m_Camera.set_depthTextureMode((DepthTextureMode) (this.m_Camera.get_depthTextureMode() | 1));
      if (this.m_ClearRadianceCmdBuffer != null)
      {
        CameraEvent cameraEvent = this.m_PreviousRenderPath != 3 ? ((this.m_Camera.get_depthTextureMode() & 2) != 2 ? (CameraEvent) 0 : (CameraEvent) 2) : (CameraEvent) 4;
        foreach (CommandBuffer commandBuffer in this.m_Camera.GetCommandBuffers(cameraEvent))
        {
          if (commandBuffer.get_name() == DS_HazeView.kClearRadianceCmdBufferName)
          {
            this.m_Camera.RemoveCommandBuffer(cameraEvent, commandBuffer);
            break;
          }
        }
      }
      this.m_PreviousRenderPath = this.m_Camera.get_actualRenderingPath();
    }

    private void UpdateResources()
    {
      this.m_X = this.m_Camera.get_pixelWidth() / (int) this.m_DownsampleFactor;
      this.m_Y = this.m_Camera.get_pixelHeight() / (int) this.m_DownsampleFactor;
      if (this.m_Camera.get_actualRenderingPath() != this.m_PreviousRenderPath)
        this.RenderPathChanged();
      RenderTextureFormat renderTextureFormat = !this.m_Camera.get_allowHDR() ? (RenderTextureFormat) 0 : (RenderTextureFormat) 2;
      bool flag = this.m_ColourSpace != QualitySettings.get_activeColorSpace();
      this.m_ColourSpace = QualitySettings.get_activeColorSpace();
      if (this.WillRenderWithTemporalReprojection)
      {
        if (Object.op_Equality((Object) this.m_RadianceTarget_01, (Object) null))
        {
          this.CreateRadianceTarget(DS_HazeView.kRadianceTarget01Name, out this.m_RadianceTarget_01);
          this.m_CurrentRadianceTarget = this.m_RadianceTarget_01;
        }
        else if (flag || ((Texture) this.m_RadianceTarget_01).get_width() != this.m_Camera.get_pixelWidth() || (((Texture) this.m_RadianceTarget_01).get_height() != this.m_Camera.get_pixelHeight() || this.m_RadianceTarget_01.get_format() != renderTextureFormat))
        {
          Object.DestroyImmediate((Object) this.m_RadianceTarget_01);
          this.CreateRadianceTarget(DS_HazeView.kRadianceTarget01Name, out this.m_RadianceTarget_01);
          this.m_CurrentRadianceTarget = this.m_RadianceTarget_01;
        }
        if (Object.op_Equality((Object) this.m_RadianceTarget_02, (Object) null))
        {
          this.CreateRadianceTarget(DS_HazeView.kRadianceTarget02Name, out this.m_RadianceTarget_02);
          this.m_PreviousRadianceTarget = this.m_RadianceTarget_02;
        }
        else if (flag || ((Texture) this.m_RadianceTarget_02).get_width() != this.m_Camera.get_pixelWidth() || (((Texture) this.m_RadianceTarget_02).get_height() != this.m_Camera.get_pixelHeight() || this.m_RadianceTarget_02.get_format() != renderTextureFormat))
        {
          Object.DestroyImmediate((Object) this.m_RadianceTarget_02);
          this.CreateRadianceTarget(DS_HazeView.kRadianceTarget02Name, out this.m_RadianceTarget_02);
          this.m_PreviousRadianceTarget = this.m_RadianceTarget_02;
        }
        if (Object.op_Equality((Object) this.m_PreviousDepthTarget, (Object) null))
          this.CreateDepthTarget(DS_HazeView.kPreviousDepthTargetName, out this.m_PreviousDepthTarget, false);
        else if (((Texture) this.m_PreviousDepthTarget).get_width() != this.m_Camera.get_pixelWidth() || ((Texture) this.m_PreviousDepthTarget).get_height() != this.m_Camera.get_pixelHeight())
        {
          Object.DestroyImmediate((Object) this.m_PreviousDepthTarget);
          this.CreateDepthTarget(DS_HazeView.kPreviousDepthTargetName, out this.m_PreviousDepthTarget, false);
        }
      }
      if (this.m_ClearRadianceCmdBuffer == null)
      {
        this.m_ClearRadianceCmdBuffer = new CommandBuffer();
        this.m_ClearRadianceCmdBuffer.set_name(DS_HazeView.kClearRadianceCmdBufferName);
      }
      CameraEvent cameraEvent = this.m_Camera.get_actualRenderingPath() != 3 ? ((this.m_Camera.get_depthTextureMode() & 2) != 2 ? (CameraEvent) 0 : (CameraEvent) 2) : (CameraEvent) 4;
      CommandBuffer foundCmd;
      if (!this.CameraHasClearRadianceCmdBuffer(out foundCmd))
        this.m_Camera.AddCommandBuffer(cameraEvent, this.m_ClearRadianceCmdBuffer);
      else if (foundCmd != this.m_ClearRadianceCmdBuffer)
      {
        this.m_Camera.RemoveCommandBuffer(cameraEvent, foundCmd);
        foundCmd.Dispose();
        this.m_Camera.AddCommandBuffer(cameraEvent, this.m_ClearRadianceCmdBuffer);
      }
      if (!Object.op_Implicit((Object) this.m_DirectLight))
        return;
      this.m_ShadowCascadesCmdBuffer = this.LightHasCascadesCopyCmdBuffer();
      if (this.m_ShadowCascadesCmdBuffer == null)
      {
        this.m_ShadowCascadesCmdBuffer = new CommandBuffer();
        this.m_ShadowCascadesCmdBuffer.set_name(DS_HazeView.kShadowCascadesCmdBufferName);
        this.m_ShadowCascadesCmdBuffer.SetGlobalTexture("_ShadowCascades", new RenderTargetIdentifier((BuiltinRenderTextureType) 1));
        this.m_DirectLight.AddCommandBuffer((LightEvent) 1, this.m_ShadowCascadesCmdBuffer);
      }
      this.m_DirectionalLightCmdBuffer = this.LightHasRenderCmdBuffer();
      if (this.m_DirectionalLightCmdBuffer == null)
      {
        this.m_DirectionalLightCmdBuffer = new CommandBuffer();
        this.m_DirectionalLightCmdBuffer.set_name(DS_HazeView.kDirectionalLightCmdBufferName);
        this.m_DirectLight.AddCommandBuffer((LightEvent) 3, this.m_DirectionalLightCmdBuffer);
      }
      if (this.m_ShadowProjectionType == QualitySettings.get_shadowProjection())
        return;
      this.m_ShadowProjectionType = QualitySettings.get_shadowProjection();
    }

    private void OnDisable()
    {
      this.SetGlobalParamsToNull();
      this.m_Camera.GetCommandBuffers((CameraEvent) 15);
      CameraEvent cameraEvent = this.m_Camera.get_actualRenderingPath() != 3 ? ((this.m_Camera.get_depthTextureMode() & 2) != 2 ? (CameraEvent) 0 : (CameraEvent) 2) : (CameraEvent) 4;
      foreach (CommandBuffer commandBuffer in this.m_Camera.GetCommandBuffers(cameraEvent))
      {
        if (commandBuffer.get_name() == DS_HazeView.kClearRadianceCmdBufferName)
        {
          this.m_Camera.RemoveCommandBuffer(cameraEvent, commandBuffer);
          break;
        }
      }
      if (Object.op_Implicit((Object) this.m_DirectLight))
      {
        foreach (CommandBuffer commandBuffer in this.m_DirectLight.GetCommandBuffers((LightEvent) 1))
        {
          if (commandBuffer.get_name() == DS_HazeView.kShadowCascadesCmdBufferName)
          {
            this.m_DirectLight.RemoveCommandBuffer((LightEvent) 1, commandBuffer);
            break;
          }
        }
        foreach (CommandBuffer commandBuffer in this.m_DirectLight.GetCommandBuffers((LightEvent) 3))
        {
          if (commandBuffer.get_name() == DS_HazeView.kDirectionalLightCmdBufferName)
          {
            this.m_DirectLight.RemoveCommandBuffer((LightEvent) 3, commandBuffer);
            break;
          }
        }
      }
      if (this.m_LightVolumeCmdBuffers.Count > 0)
      {
        using (Dictionary<Light, CommandBuffer>.Enumerator enumerator = this.m_LightVolumeCmdBuffers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<Light, CommandBuffer> current = enumerator.Current;
            current.Key.RemoveCommandBuffer((LightEvent) 1, current.Value);
            current.Value.Dispose();
          }
        }
        this.m_LightVolumeCmdBuffers.Clear();
      }
      if (this.m_RenderNonShadowVolumes == null)
        return;
      this.m_RenderNonShadowVolumes.Clear();
    }

    private void OnDestroy()
    {
      if (Object.op_Implicit((Object) this.m_RadianceTarget_01))
      {
        this.m_RadianceTarget_01.Release();
        Object.DestroyImmediate((Object) this.m_RadianceTarget_01);
        this.m_RadianceTarget_01 = (RenderTexture) null;
      }
      if (Object.op_Implicit((Object) this.m_RadianceTarget_02))
      {
        this.m_RadianceTarget_02.Release();
        Object.DestroyImmediate((Object) this.m_RadianceTarget_02);
        this.m_RadianceTarget_02 = (RenderTexture) null;
      }
      if (Object.op_Implicit((Object) this.m_PreviousDepthTarget))
      {
        this.m_PreviousDepthTarget.Release();
        Object.DestroyImmediate((Object) this.m_PreviousDepthTarget);
        this.m_PreviousDepthTarget = (RenderTexture) null;
      }
      if (this.m_ClearRadianceCmdBuffer != null)
      {
        if (this.m_Camera.get_actualRenderingPath() == 3)
          this.m_Camera.RemoveCommandBuffer((CameraEvent) 4, this.m_ClearRadianceCmdBuffer);
        else
          this.m_Camera.RemoveCommandBuffer((this.m_Camera.get_depthTextureMode() & 2) != 2 ? (CameraEvent) 0 : (CameraEvent) 2, this.m_ClearRadianceCmdBuffer);
        this.m_ClearRadianceCmdBuffer.Dispose();
        this.m_ClearRadianceCmdBuffer = (CommandBuffer) null;
      }
      if (this.m_ShadowCascadesCmdBuffer != null)
      {
        if (Object.op_Inequality((Object) this.m_DirectLight, (Object) null))
          this.m_DirectLight.RemoveCommandBuffer((LightEvent) 1, this.m_ShadowCascadesCmdBuffer);
        this.m_ShadowCascadesCmdBuffer.Dispose();
        this.m_ShadowCascadesCmdBuffer = (CommandBuffer) null;
      }
      if (this.m_DirectionalLightCmdBuffer != null)
      {
        if (Object.op_Inequality((Object) this.m_DirectLight, (Object) null))
          this.m_DirectLight.RemoveCommandBuffer((LightEvent) 3, this.m_DirectionalLightCmdBuffer);
        this.m_DirectionalLightCmdBuffer.Dispose();
        this.m_DirectionalLightCmdBuffer = (CommandBuffer) null;
      }
      if (this.m_LightVolumeCmdBuffers.Count > 0)
      {
        using (Dictionary<Light, CommandBuffer>.Enumerator enumerator = this.m_LightVolumeCmdBuffers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<Light, CommandBuffer> current = enumerator.Current;
            current.Key.RemoveCommandBuffer((LightEvent) 1, current.Value);
            current.Value.Dispose();
          }
        }
        this.m_LightVolumeCmdBuffers.Clear();
      }
      if (this.m_RenderNonShadowVolumes == null)
        return;
      this.m_Camera.RemoveCommandBuffer((CameraEvent) 12, this.m_RenderNonShadowVolumes);
      this.m_RenderNonShadowVolumes.Dispose();
      this.m_RenderNonShadowVolumes = (CommandBuffer) null;
    }

    private void OnPreRender()
    {
      if (!this.CheckHasSystemSupport())
        ((Behaviour) this).set_enabled(false);
      this.UpdateResources();
      this.SetShaderKeyWords();
      this.m_PerFrameRadianceTarget = RenderTexture.GetTemporary(this.m_X, this.m_Y, 0, !this.m_Camera.get_allowHDR() ? (RenderTextureFormat) 0 : (RenderTextureFormat) 2, (RenderTextureReadWrite) 1, this.AntiAliasingLevel());
      ((Object) this.m_PerFrameRadianceTarget).set_name("_DS_Haze_PerFrameRadiance");
      ((Texture) this.m_PerFrameRadianceTarget).set_filterMode((FilterMode) 0);
      this.m_ClearRadianceCmdBuffer.Clear();
      this.m_ClearRadianceCmdBuffer.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.m_PerFrameRadianceTarget));
      this.m_ClearRadianceCmdBuffer.ClearRenderTarget(false, true, Color.get_clear());
      DS_HazeCore instance = DS_HazeCore.Instance;
      DS_HazeContextItem ctx;
      if (this.m_OverrideContextAsset && Object.op_Inequality((Object) this.m_Context, (Object) null))
      {
        ctx = !this.m_OverrideContextVariant ? this.m_Context.Context.GetContextItemBlended(this.m_Time) : this.m_Context.Context.GetItemAtIndex(this.m_ContextItemIndex);
      }
      else
      {
        if (Object.op_Equality((Object) instance, (Object) null))
        {
          this.SetGlobalParamsToNull();
          return;
        }
        ctx = instance.GetRenderContextAtPosition(((Component) this).get_transform().get_position());
      }
      if (ctx == null)
      {
        this.SetGlobalParamsToNull();
      }
      else
      {
        this.SetMaterialFromContext(ctx);
        float farClipPlane = this.m_Camera.get_farClipPlane();
        float num1 = Mathf.Tan(this.m_Camera.get_fieldOfView() * 0.5f * ((float) Math.PI / 180f));
        float num2 = num1 * this.m_Camera.get_aspect();
        Vector3 vector3_1 = Vector3.op_Multiply(((Component) this).get_transform().get_forward(), farClipPlane);
        Vector3 vector3_2 = Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), num2), farClipPlane);
        Vector3 vector3_3 = Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), num1), farClipPlane);
        this.m_Material.SetVector("_ViewportCorner", Vector4.op_Implicit(Vector3.op_Subtraction(Vector3.op_Subtraction(vector3_1, vector3_2), vector3_3)));
        this.m_Material.SetVector("_ViewportRight", Vector4.op_Implicit(Vector3.op_Multiply(vector3_2, 2f)));
        this.m_Material.SetVector("_ViewportUp", Vector4.op_Implicit(Vector3.op_Multiply(vector3_3, 2f)));
        if (Object.op_Implicit((Object) this.m_DirectLight) && this.m_RenderAtmosphereVolumetrics)
          this.m_DirectionalLightCmdBuffer.Blit(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 0), RenderTargetIdentifier.op_Implicit((Texture) this.m_PerFrameRadianceTarget), this.m_Material, (int) (this.m_VolumeSamples + (this.m_DownsampleFactor != DS_HazeView.SizeFactor.Half ? 3 : 0)));
      }
      if (!this.m_RenderLocalVolumetrics)
        return;
      Matrix4x4 viewProjMtx = Matrix4x4.op_Multiply(GL.GetGPUProjectionMatrix(this.m_Camera.get_projectionMatrix(), true), this.m_Camera.get_worldToCameraMatrix());
      instance.GetRenderLightVolumes(((Component) this).get_transform().get_position(), this.m_PerFrameLightVolumes, this.m_PerFrameShadowLightVolumes);
      if (this.m_PerFrameLightVolumes.Count > 0)
        this.m_RenderNonShadowVolumes.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.m_PerFrameRadianceTarget));
      foreach (DS_HazeLightVolume frameLightVolume in this.m_PerFrameLightVolumes)
      {
        frameLightVolume.SetupMaterialPerFrame(viewProjMtx, this.m_Camera.get_worldToCameraMatrix(), ((Component) this).get_transform(), !this.WillRenderWithTemporalReprojection ? 0.0f : this.m_InterleavedOffsetIndex);
        frameLightVolume.AddLightRenderCommand(((Component) this).get_transform(), this.m_RenderNonShadowVolumes, (int) this.m_DownsampleFactor);
      }
      foreach (DS_HazeLightVolume shadowLightVolume in this.m_PerFrameShadowLightVolumes)
      {
        shadowLightVolume.SetupMaterialPerFrame(viewProjMtx, this.m_Camera.get_worldToCameraMatrix(), ((Component) this).get_transform(), !this.WillRenderWithTemporalReprojection ? 0.0f : this.m_InterleavedOffsetIndex);
        shadowLightVolume.FillLightCommandBuffer(this.m_PerFrameRadianceTarget, ((Component) this).get_transform(), (int) this.m_DownsampleFactor);
        this.m_LightVolumeCmdBuffers.Add(shadowLightVolume.LightSource, shadowLightVolume.RenderCommandBuffer);
      }
    }

    private void BlitToMRT(
      RenderTexture source,
      RenderTexture[] destination,
      Material mat,
      int pass)
    {
      RenderBuffer[] renderBufferArray = new RenderBuffer[destination.Length];
      for (int index = 0; index < destination.Length; ++index)
        renderBufferArray[index] = destination[index].get_colorBuffer();
      Graphics.SetRenderTarget(renderBufferArray, destination[0].get_depthBuffer());
      mat.SetTexture("_MainTex", (Texture) source);
      mat.SetPass(pass);
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(7);
      GL.MultiTexCoord2(0, 0.0f, 0.0f);
      GL.Vertex3(0.0f, 0.0f, 0.1f);
      GL.MultiTexCoord2(0, 1f, 0.0f);
      GL.Vertex3(1f, 0.0f, 0.1f);
      GL.MultiTexCoord2(0, 1f, 1f);
      GL.Vertex3(1f, 1f, 0.1f);
      GL.MultiTexCoord2(0, 0.0f, 1f);
      GL.Vertex3(0.0f, 1f, 0.1f);
      GL.End();
      GL.PopMatrix();
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
      RenderTexture renderTexture1 = (RenderTexture) null;
      RenderTexture renderTexture2 = (RenderTexture) null;
      if (this.m_RenderAtmosphereVolumetrics || this.m_RenderLocalVolumetrics)
      {
        renderTexture1 = RenderTexture.GetTemporary(this.m_X, this.m_Y, 0, !this.m_Camera.get_allowHDR() ? (RenderTextureFormat) 0 : (RenderTextureFormat) 2);
        renderTexture2 = RenderTexture.GetTemporary(this.m_X, this.m_Y, 0, (RenderTextureFormat) 14, (RenderTextureReadWrite) 1, 1);
        Graphics.Blit((Texture) null, renderTexture2, this.m_Material, this.m_DownsampleFactor != DS_HazeView.SizeFactor.Half ? 11 : 10);
        this.m_Material.SetTexture("_HalfResDepth", (Texture) renderTexture2);
        Graphics.Blit((Texture) this.m_PerFrameRadianceTarget, renderTexture1, this.m_Material, 6);
        Graphics.Blit((Texture) renderTexture1, this.m_PerFrameRadianceTarget, this.m_Material, 7);
        if (this.m_TemporalReprojection)
        {
          this.m_Material.SetTexture("_PrevAccumBuffer", (Texture) this.m_PreviousRadianceTarget);
          this.m_Material.SetTexture("_PrevDepthBuffer", (Texture) this.m_PreviousDepthTarget);
        }
      }
      ((Texture) this.m_PerFrameRadianceTarget).set_filterMode((FilterMode) 1);
      this.m_Material.SetTexture("_RadianceBuffer", (Texture) this.m_PerFrameRadianceTarget);
      if (Object.op_Equality((Object) dest, (Object) null))
      {
        RenderTexture temporary = RenderTexture.GetTemporary(((Texture) src).get_width(), ((Texture) src).get_height(), src.get_depth(), src.get_format());
        if (this.WillRenderWithTemporalReprojection)
        {
          RenderTexture[] destination = new RenderTexture[2]
          {
            temporary,
            this.m_CurrentRadianceTarget
          };
          this.BlitToMRT(src, destination, this.m_Material, 8);
        }
        else
          Graphics.Blit((Texture) src, temporary, this.m_Material, 8);
        Graphics.Blit((Texture) temporary, (RenderTexture) null);
        RenderTexture.ReleaseTemporary(temporary);
      }
      else if (this.WillRenderWithTemporalReprojection)
      {
        RenderTexture[] destination = new RenderTexture[2]
        {
          dest,
          this.m_CurrentRadianceTarget
        };
        this.BlitToMRT(src, destination, this.m_Material, 8);
      }
      else
        Graphics.Blit((Texture) src, dest, this.m_Material, 8);
      if (this.WillRenderWithTemporalReprojection)
      {
        Graphics.Blit((Texture) src, this.m_PreviousDepthTarget, this.m_Material, 9);
        Graphics.SetRenderTarget(dest);
        Shader.SetGlobalTexture("_DS_RadianceBuffer", (Texture) this.m_CurrentRadianceTarget);
        RenderTexture.ReleaseTemporary(this.m_PerFrameRadianceTarget);
      }
      else
        Shader.SetGlobalTexture("_DS_RadianceBuffer", (Texture) this.m_PerFrameRadianceTarget);
      if (Object.op_Inequality((Object) renderTexture1, (Object) null))
        RenderTexture.ReleaseTemporary(renderTexture1);
      if (!Object.op_Inequality((Object) renderTexture2, (Object) null))
        return;
      RenderTexture.ReleaseTemporary(renderTexture2);
    }

    private void OnPostRender()
    {
      if (this.WillRenderWithTemporalReprojection)
      {
        RenderTexture currentRadianceTarget = this.m_CurrentRadianceTarget;
        this.m_CurrentRadianceTarget = this.m_PreviousRadianceTarget;
        this.m_PreviousRadianceTarget = currentRadianceTarget;
        this.m_PreviousViewProjMatrix = Matrix4x4.op_Multiply(GL.GetGPUProjectionMatrix(this.m_Camera.get_projectionMatrix(), true), this.m_Camera.get_worldToCameraMatrix());
        this.m_PreviousInvViewProjMatrix = ((Matrix4x4) ref this.m_PreviousViewProjMatrix).get_inverse();
      }
      else
        RenderTexture.ReleaseTemporary(this.m_PerFrameRadianceTarget);
      if (this.m_LightVolumeCmdBuffers.Count > 0)
      {
        using (Dictionary<Light, CommandBuffer>.Enumerator enumerator = this.m_LightVolumeCmdBuffers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.Value.Clear();
        }
        this.m_LightVolumeCmdBuffers.Clear();
      }
      if (Object.op_Implicit((Object) this.m_DirectLight))
        this.m_DirectionalLightCmdBuffer.Clear();
      this.m_RenderNonShadowVolumes.Clear();
      this.m_PerFrameLightVolumes.Clear();
      this.m_PerFrameShadowLightVolumes.Clear();
    }

    private enum SizeFactor
    {
      Half = 2,
      Quarter = 4,
    }

    private enum VolumeSamples
    {
      x16,
      x24,
      x32,
    }
  }
}
