// Decompiled with JetBrains decompiler
// Type: AmplifyOcclusionBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

[AddComponentMenu("")]
public class AmplifyOcclusionBase : MonoBehaviour
{
  private static int m_nextID = 0;
  private static Mesh m_quadMesh = (Mesh) null;
  private static Material m_occlusionMat = (Material) null;
  private static Material m_blurMat = (Material) null;
  private static Material m_applyOcclusionMat = (Material) null;
  private static readonly int PerPixelNormalSourceCount = 4;
  private static readonly float[] m_temporalRotations = new float[6]
  {
    60f,
    300f,
    180f,
    240f,
    120f,
    0.0f
  };
  private static readonly float[] m_spatialOffsets = new float[4]
  {
    0.0f,
    0.5f,
    0.25f,
    0.75f
  };
  private int m_myID;
  private string m_myIDstring;
  [Header("Ambient Occlusion")]
  [Tooltip("How to inject the occlusion: Post Effect = Overlay, Deferred = Deferred Injection, Debug - Vizualize.")]
  public AmplifyOcclusionBase.ApplicationMethod ApplyMethod;
  [Tooltip("Number of samples per pass.")]
  public AmplifyOcclusionBase.SampleCountLevel SampleCount;
  [Tooltip("Source of per-pixel normals: None = All, Camera = Forward, GBuffer = Deferred.")]
  public AmplifyOcclusionBase.PerPixelNormalSource PerPixelNormals;
  [Tooltip("Final applied intensity of the occlusion effect.")]
  [Range(0.0f, 1f)]
  public float Intensity;
  [Tooltip("Color tint for occlusion.")]
  public Color Tint;
  [Tooltip("Radius spread of the occlusion.")]
  [Range(0.0f, 32f)]
  public float Radius;
  [Tooltip("Power exponent attenuation of the occlusion.")]
  [Range(0.0f, 16f)]
  public float PowerExponent;
  [Tooltip("Controls the initial occlusion contribution offset.")]
  [Range(0.0f, 0.99f)]
  public float Bias;
  [Tooltip("Controls the thickness occlusion contribution.")]
  [Range(0.0f, 1f)]
  public float Thickness;
  [Tooltip("Compute the Occlusion and Blur at half of the resolution.")]
  public bool Downsample;
  [Tooltip("Cache optimization for best performance / quality tradeoff.")]
  public bool CacheAware;
  [Header("Distance Fade")]
  [Tooltip("Control parameters at faraway.")]
  public bool FadeEnabled;
  [Tooltip("Distance in Unity unities that start to fade.")]
  public float FadeStart;
  [Tooltip("Length distance to performe the transition.")]
  public float FadeLength;
  [Tooltip("Final Intensity parameter.")]
  [Range(0.0f, 1f)]
  public float FadeToIntensity;
  public Color FadeToTint;
  [Tooltip("Final Radius parameter.")]
  [Range(0.0f, 32f)]
  public float FadeToRadius;
  [Tooltip("Final PowerExponent parameter.")]
  [Range(0.0f, 16f)]
  public float FadeToPowerExponent;
  [Tooltip("Final Thickness parameter.")]
  [Range(0.0f, 1f)]
  public float FadeToThickness;
  [Header("Bilateral Blur")]
  public bool BlurEnabled;
  [Tooltip("Radius in screen pixels.")]
  [Range(1f, 4f)]
  public int BlurRadius;
  [Tooltip("Number of times that the Blur will repeat.")]
  [Range(1f, 4f)]
  public int BlurPasses;
  [Tooltip("Sharpness of blur edge-detection: 0 = Softer Edges, 20 = Sharper Edges.")]
  [Range(0.0f, 20f)]
  public float BlurSharpness;
  [Header("Temporal Filter")]
  [Tooltip("Accumulates the effect over the time.")]
  public bool FilterEnabled;
  [Tooltip("Controls the accumulation decayment: 0 = More flicker with less ghosting, 1 = Less flicker with more ghosting.")]
  [Range(0.0f, 1f)]
  public float FilterBlending;
  [Tooltip("Controls the discard sensitivity based on the motion of the scene and objects.")]
  [Range(0.0f, 1f)]
  public float FilterResponse;
  private bool m_HDR;
  private bool m_MSAA;
  private AmplifyOcclusionBase.PerPixelNormalSource m_prevPerPixelNormals;
  private AmplifyOcclusionBase.ApplicationMethod m_prevApplyMethod;
  private bool m_prevDeferredReflections;
  private AmplifyOcclusionBase.SampleCountLevel m_prevSampleCount;
  private bool m_prevDownsample;
  private bool m_prevCacheAware;
  private bool m_prevBlurEnabled;
  private int m_prevBlurRadius;
  private int m_prevBlurPasses;
  private bool m_prevFilterEnabled;
  private bool m_prevHDR;
  private bool m_prevMSAA;
  private Camera m_targetCamera;
  private RenderTargetIdentifier[] applyDebugTargetsTemporal;
  private RenderTargetIdentifier[] applyDeferredTargets_Log_Temporal;
  private RenderTargetIdentifier[] applyDeferredTargetsTemporal;
  private RenderTargetIdentifier[] applyOcclusionTemporal;
  private RenderTargetIdentifier[] applyPostEffectTargetsTemporal;
  private bool useMRTBlendingFallback;
  private AmplifyOcclusionBase.CmdBuffer m_commandBuffer_Parameters;
  private AmplifyOcclusionBase.CmdBuffer m_commandBuffer_Occlusion;
  private AmplifyOcclusionBase.CmdBuffer m_commandBuffer_Apply;
  private RenderTextureFormat m_occlusionRTFormat;
  private RenderTextureFormat m_accumTemporalRTFormat;
  private RenderTextureFormat m_temporaryEmissionRTFormat;
  private RenderTextureFormat m_motionIntensityRTFormat;
  private bool m_paramsChanged;
  private bool m_clearHistory;
  private RenderTexture m_occlusionDepthRT;
  private RenderTexture[] m_temporalAccumRT;
  private RenderTexture m_depthMipmap;
  private uint m_sampleStep;
  private uint m_curTemporalIdx;
  private uint m_prevTemporalIdx;
  private Matrix4x4 m_prevViewProjMatrixLeft;
  private Matrix4x4 m_prevInvViewProjMatrixLeft;
  private Matrix4x4 m_prevViewProjMatrixRight;
  private Matrix4x4 m_prevInvViewProjMatrixRight;
  private string[] m_tmpMipString;
  private int m_numberMips;
  private readonly RenderTargetIdentifier[] m_applyDeferredTargets;
  private readonly RenderTargetIdentifier[] m_applyDeferredTargets_Log;
  private AmplifyOcclusionBase.TargetDesc m_target;

  public AmplifyOcclusionBase()
  {
    base.\u002Ector();
  }

  private bool UsingTemporalFilter
  {
    get
    {
      return this.m_sampleStep > 0U && this.FilterEnabled && this.m_targetCamera.get_cameraType() != 2;
    }
  }

  private bool UsingMotionVectors
  {
    get
    {
      return this.UsingTemporalFilter && this.ApplyMethod != AmplifyOcclusionBase.ApplicationMethod.Deferred;
    }
  }

  private void createCommandBuffer(
    ref AmplifyOcclusionBase.CmdBuffer aCmdBuffer,
    string aCmdBufferName,
    CameraEvent aCameraEvent)
  {
    if (aCmdBuffer.cmdBuffer != null)
      this.cleanupCommandBuffer(ref aCmdBuffer);
    aCmdBuffer.cmdBufferName = aCmdBufferName;
    aCmdBuffer.cmdBuffer = new CommandBuffer();
    aCmdBuffer.cmdBuffer.set_name(aCmdBufferName);
    aCmdBuffer.cmdBufferEvent = aCameraEvent;
    this.m_targetCamera.AddCommandBuffer(aCameraEvent, aCmdBuffer.cmdBuffer);
  }

  private void cleanupCommandBuffer(ref AmplifyOcclusionBase.CmdBuffer aCmdBuffer)
  {
    CommandBuffer[] commandBuffers = this.m_targetCamera.GetCommandBuffers(aCmdBuffer.cmdBufferEvent);
    for (int index = 0; index < commandBuffers.Length; ++index)
    {
      if (commandBuffers[index].get_name() == aCmdBuffer.cmdBufferName)
        this.m_targetCamera.RemoveCommandBuffer(aCmdBuffer.cmdBufferEvent, commandBuffers[index]);
    }
    aCmdBuffer.cmdBufferName = (string) null;
    aCmdBuffer.cmdBufferEvent = (CameraEvent) 0;
    aCmdBuffer.cmdBuffer = (CommandBuffer) null;
  }

  private void createQuadMesh()
  {
    if (!Object.op_Equality((Object) AmplifyOcclusionBase.m_quadMesh, (Object) null))
      return;
    AmplifyOcclusionBase.m_quadMesh = new Mesh();
    AmplifyOcclusionBase.m_quadMesh.set_vertices(new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(1f, 1f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f)
    });
    AmplifyOcclusionBase.m_quadMesh.set_uv(new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f),
      new Vector2(1f, 0.0f)
    });
    AmplifyOcclusionBase.m_quadMesh.set_triangles(new int[6]
    {
      0,
      1,
      2,
      0,
      2,
      3
    });
    AmplifyOcclusionBase.m_quadMesh.set_normals(new Vector3[0]);
    AmplifyOcclusionBase.m_quadMesh.set_tangents(new Vector4[0]);
    AmplifyOcclusionBase.m_quadMesh.set_colors32(new Color32[0]);
    AmplifyOcclusionBase.m_quadMesh.set_colors(new Color[0]);
  }

  private void PerformBlit(CommandBuffer cb, Material mat, int pass)
  {
    cb.DrawMesh(AmplifyOcclusionBase.m_quadMesh, Matrix4x4.get_identity(), mat, 0, pass);
  }

  private Material createMaterialWithShaderName(string aShaderName, bool aThroughErrorMsg)
  {
    Shader shader = Shader.Find(aShaderName);
    if (Object.op_Equality((Object) shader, (Object) null))
    {
      if (aThroughErrorMsg)
        Debug.LogErrorFormat("[AmplifyOcclusion] Cannot find shader: \"{0}\" Please contact support@amplify.pt", new object[1]
        {
          (object) aShaderName
        });
      return (Material) null;
    }
    Material material = new Material(shader);
    ((Object) material).set_hideFlags((HideFlags) 52);
    return material;
  }

  private void checkMaterials(bool aThroughErrorMsg)
  {
    if (Object.op_Equality((Object) AmplifyOcclusionBase.m_occlusionMat, (Object) null))
      AmplifyOcclusionBase.m_occlusionMat = this.createMaterialWithShaderName("Hidden/Amplify Occlusion/Occlusion", aThroughErrorMsg);
    if (Object.op_Equality((Object) AmplifyOcclusionBase.m_blurMat, (Object) null))
      AmplifyOcclusionBase.m_blurMat = this.createMaterialWithShaderName("Hidden/Amplify Occlusion/Blur", aThroughErrorMsg);
    if (!Object.op_Equality((Object) AmplifyOcclusionBase.m_applyOcclusionMat, (Object) null))
      return;
    AmplifyOcclusionBase.m_applyOcclusionMat = this.createMaterialWithShaderName("Hidden/Amplify Occlusion/Apply", aThroughErrorMsg);
    if (!Object.op_Inequality((Object) AmplifyOcclusionBase.m_applyOcclusionMat, (Object) null))
      return;
    this.useMRTBlendingFallback = AmplifyOcclusionBase.m_applyOcclusionMat.GetTag("MRTBlending", false).ToUpper() != "TRUE";
  }

  private bool checkRenderTextureFormats()
  {
    if (!SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 0) || !SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 2))
      return false;
    this.m_occlusionRTFormat = (RenderTextureFormat) 13;
    if (!SystemInfo.SupportsRenderTextureFormat(this.m_occlusionRTFormat))
    {
      this.m_occlusionRTFormat = (RenderTextureFormat) 12;
      if (!SystemInfo.SupportsRenderTextureFormat(this.m_occlusionRTFormat))
        this.m_occlusionRTFormat = (RenderTextureFormat) 2;
    }
    return true;
  }

  private void OnEnable()
  {
    this.m_myID = AmplifyOcclusionBase.m_nextID;
    this.m_myIDstring = this.m_myID.ToString();
    ++AmplifyOcclusionBase.m_nextID;
    if (!this.checkRenderTextureFormats())
    {
      Debug.LogError((object) "[AmplifyOcclusion] Target platform does not meet the minimum requirements for this effect to work properly.");
      ((Behaviour) this).set_enabled(false);
    }
    else
    {
      this.checkMaterials(false);
      this.createQuadMesh();
    }
  }

  private void Reset()
  {
    if (this.m_commandBuffer_Parameters.cmdBuffer != null)
      this.cleanupCommandBuffer(ref this.m_commandBuffer_Parameters);
    if (this.m_commandBuffer_Occlusion.cmdBuffer != null)
      this.cleanupCommandBuffer(ref this.m_commandBuffer_Occlusion);
    if (this.m_commandBuffer_Apply.cmdBuffer != null)
      this.cleanupCommandBuffer(ref this.m_commandBuffer_Apply);
    this.safeReleaseRT(ref this.m_occlusionDepthRT);
    this.safeReleaseRT(ref this.m_depthMipmap);
    this.releaseTemporalRT();
    this.m_tmpMipString = (string[]) null;
  }

  private void OnDisable()
  {
    this.Reset();
  }

  private void releaseTemporalRT()
  {
    if (this.m_temporalAccumRT != null)
    {
      for (int index = 0; index < this.m_temporalAccumRT.Length; ++index)
        this.safeReleaseRT(ref this.m_temporalAccumRT[index]);
    }
    this.m_temporalAccumRT = (RenderTexture[]) null;
  }

  private void ClearHistory(CommandBuffer cb)
  {
    this.m_clearHistory = false;
    if (this.m_temporalAccumRT == null || !Object.op_Inequality((Object) this.m_occlusionDepthRT, (Object) null))
      return;
    for (int index = 0; index < this.m_temporalAccumRT.Length; ++index)
    {
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.m_temporalAccumRT[index]));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_occlusionMat, 34);
    }
  }

  private void checkParamsChanged()
  {
    bool allowHdr = this.m_targetCamera.get_allowHDR();
    bool flag = this.m_targetCamera.get_allowMSAA() && this.m_targetCamera.get_actualRenderingPath() != 2 && this.m_targetCamera.get_actualRenderingPath() != 3 && QualitySettings.get_antiAliasing() >= 1;
    int antiAliasing = !flag ? 1 : QualitySettings.get_antiAliasing();
    if (Object.op_Inequality((Object) this.m_occlusionDepthRT, (Object) null))
    {
      if (((Texture) this.m_occlusionDepthRT).get_width() != this.m_target.width || ((Texture) this.m_occlusionDepthRT).get_height() != this.m_target.height || (this.m_prevMSAA != flag || !this.m_occlusionDepthRT.IsCreated()) || this.m_temporalAccumRT != null && (!this.m_temporalAccumRT[0].IsCreated() || !this.m_temporalAccumRT[1].IsCreated()))
      {
        this.safeReleaseRT(ref this.m_occlusionDepthRT);
        this.safeReleaseRT(ref this.m_depthMipmap);
        this.releaseTemporalRT();
        this.m_paramsChanged = true;
      }
      else if (this.m_prevFilterEnabled != this.FilterEnabled)
        this.releaseTemporalRT();
    }
    if (this.m_temporalAccumRT != null)
    {
      if (this.isStereoMultiPassEnabled())
      {
        if (this.m_temporalAccumRT.Length != 4)
          this.m_temporalAccumRT = (RenderTexture[]) null;
      }
      else if (this.m_temporalAccumRT.Length != 2)
        this.m_temporalAccumRT = (RenderTexture[]) null;
    }
    if (Object.op_Equality((Object) this.m_occlusionDepthRT, (Object) null))
      this.m_occlusionDepthRT = this.safeAllocateRT("_AO_OcclusionDepthTexture", this.m_target.width, this.m_target.height, this.m_occlusionRTFormat, (RenderTextureReadWrite) 1, (FilterMode) 1, 1, false);
    if (this.m_temporalAccumRT == null && this.FilterEnabled)
    {
      this.m_temporalAccumRT = !this.isStereoMultiPassEnabled() ? new RenderTexture[2] : new RenderTexture[4];
      for (int index = 0; index < this.m_temporalAccumRT.Length; ++index)
        this.m_temporalAccumRT[index] = this.safeAllocateRT("_AO_TemporalAccum_" + index.ToString(), this.m_target.width, this.m_target.height, this.m_accumTemporalRTFormat, (RenderTextureReadWrite) 1, (FilterMode) 1, antiAliasing, false);
      this.m_clearHistory = true;
    }
    if (this.CacheAware && Object.op_Equality((Object) this.m_depthMipmap, (Object) null))
    {
      this.m_depthMipmap = this.safeAllocateRT("_AO_DepthMipmap", this.m_target.width >> 1, this.m_target.height >> 1, (RenderTextureFormat) 14, (RenderTextureReadWrite) 1, (FilterMode) 0, 1, true);
      this.m_numberMips = (int) ((double) Mathf.Log((float) Mathf.Min(this.m_target.width, this.m_target.height), 2f) + 1.0) - 1;
      this.m_tmpMipString = new string[this.m_numberMips];
      for (int index = 0; index < this.m_numberMips; ++index)
        this.m_tmpMipString[index] = "_AO_TmpMip_" + index.ToString();
    }
    else if (!this.CacheAware && Object.op_Inequality((Object) this.m_depthMipmap, (Object) null))
    {
      this.safeReleaseRT(ref this.m_depthMipmap);
      this.m_tmpMipString = (string[]) null;
    }
    if (this.m_prevSampleCount == this.SampleCount && this.m_prevDownsample == this.Downsample && (this.m_prevCacheAware == this.CacheAware && this.m_prevBlurEnabled == this.BlurEnabled) && (this.m_prevBlurPasses == this.BlurPasses && this.m_prevBlurRadius == this.BlurRadius || !this.BlurEnabled) && (this.m_prevFilterEnabled == this.FilterEnabled && this.m_prevHDR == allowHdr && this.m_prevMSAA == flag))
      return;
    this.m_clearHistory |= this.m_prevHDR != allowHdr;
    this.m_clearHistory |= this.m_prevMSAA != flag;
    this.m_HDR = allowHdr;
    this.m_MSAA = flag;
    this.m_paramsChanged = true;
  }

  private void updateParams()
  {
    this.m_prevSampleCount = this.SampleCount;
    this.m_prevDownsample = this.Downsample;
    this.m_prevCacheAware = this.CacheAware;
    this.m_prevBlurEnabled = this.BlurEnabled;
    this.m_prevBlurPasses = this.BlurPasses;
    this.m_prevBlurRadius = this.BlurRadius;
    this.m_prevFilterEnabled = this.FilterEnabled;
    this.m_prevHDR = this.m_HDR;
    this.m_prevMSAA = this.m_MSAA;
    this.m_paramsChanged = false;
  }

  private void Update()
  {
    if (Object.op_Inequality((Object) this.m_targetCamera, (Object) null))
    {
      if (this.m_targetCamera.get_actualRenderingPath() != 3)
      {
        if (this.PerPixelNormals != AmplifyOcclusionBase.PerPixelNormalSource.None && this.PerPixelNormals != AmplifyOcclusionBase.PerPixelNormalSource.Camera)
        {
          this.m_paramsChanged = true;
          this.PerPixelNormals = AmplifyOcclusionBase.PerPixelNormalSource.Camera;
          Debug.LogWarning((object) "[AmplifyOcclusion] GBuffer Normals only available in Camera Deferred Shading mode. Switched to Camera source.");
        }
        if (this.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.Deferred)
        {
          this.m_paramsChanged = true;
          this.ApplyMethod = AmplifyOcclusionBase.ApplicationMethod.PostEffect;
          Debug.LogWarning((object) "[AmplifyOcclusion] Deferred Method requires a Deferred Shading path. Switching to Post Effect Method.");
        }
      }
      else if (this.PerPixelNormals == AmplifyOcclusionBase.PerPixelNormalSource.Camera)
      {
        this.m_paramsChanged = true;
        this.PerPixelNormals = AmplifyOcclusionBase.PerPixelNormalSource.GBuffer;
        Debug.LogWarning((object) "[AmplifyOcclusion] Camera Normals not supported for Deferred Method. Switching to GBuffer Normals.");
      }
      if ((this.m_targetCamera.get_depthTextureMode() & 1) == null)
      {
        Camera targetCamera = this.m_targetCamera;
        targetCamera.set_depthTextureMode((DepthTextureMode) (targetCamera.get_depthTextureMode() | 1));
      }
      if (this.PerPixelNormals == AmplifyOcclusionBase.PerPixelNormalSource.Camera && (this.m_targetCamera.get_depthTextureMode() & 2) == null)
      {
        Camera targetCamera = this.m_targetCamera;
        targetCamera.set_depthTextureMode((DepthTextureMode) (targetCamera.get_depthTextureMode() | 2));
      }
      if (!this.UsingMotionVectors || (this.m_targetCamera.get_depthTextureMode() & 4) != null)
        return;
      Camera targetCamera1 = this.m_targetCamera;
      targetCamera1.set_depthTextureMode((DepthTextureMode) (targetCamera1.get_depthTextureMode() | 4));
    }
    else
      this.m_targetCamera = (Camera) ((Component) this).GetComponent<Camera>();
  }

  private void OnPreRender()
  {
    this.checkMaterials(true);
    if (Object.op_Inequality((Object) this.m_targetCamera, (Object) null))
    {
      bool flag = GraphicsSettings.GetShaderMode((BuiltinShaderType) 1) != 0;
      if (this.m_prevPerPixelNormals != this.PerPixelNormals || this.m_prevApplyMethod != this.ApplyMethod || (this.m_prevDeferredReflections != flag || this.m_commandBuffer_Parameters.cmdBuffer == null) || (this.m_commandBuffer_Occlusion.cmdBuffer == null || this.m_commandBuffer_Apply.cmdBuffer == null))
      {
        CameraEvent aCameraEvent = (CameraEvent) 12;
        if (this.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.Deferred)
          aCameraEvent = !flag ? (CameraEvent) 6 : (CameraEvent) 21;
        this.createCommandBuffer(ref this.m_commandBuffer_Parameters, "AmplifyOcclusion_Parameters_" + this.m_myIDstring, aCameraEvent);
        this.createCommandBuffer(ref this.m_commandBuffer_Occlusion, "AmplifyOcclusion_Compute_" + this.m_myIDstring, aCameraEvent);
        this.createCommandBuffer(ref this.m_commandBuffer_Apply, "AmplifyOcclusion_Apply_" + this.m_myIDstring, aCameraEvent);
        this.m_prevPerPixelNormals = this.PerPixelNormals;
        this.m_prevApplyMethod = this.ApplyMethod;
        this.m_prevDeferredReflections = flag;
        this.m_paramsChanged = true;
      }
      if (this.m_commandBuffer_Parameters.cmdBuffer == null || this.m_commandBuffer_Occlusion.cmdBuffer == null || this.m_commandBuffer_Apply.cmdBuffer == null)
        return;
      if (this.isStereoMultiPassEnabled())
      {
        uint num1 = this.m_sampleStep >> 1 & 1U;
        uint num2 = this.m_sampleStep & 1U;
        this.m_curTemporalIdx = num2 * 2U + num1;
        this.m_prevTemporalIdx = (uint) ((int) num2 * 2 + (1 - (int) num1));
      }
      else
      {
        uint num = this.m_sampleStep & 1U;
        this.m_curTemporalIdx = num;
        this.m_prevTemporalIdx = 1U - num;
      }
      this.m_commandBuffer_Parameters.cmdBuffer.Clear();
      this.UpdateGlobalShaderConstants(this.m_commandBuffer_Parameters.cmdBuffer);
      this.UpdateGlobalShaderConstants_Matrices(this.m_commandBuffer_Parameters.cmdBuffer);
      this.UpdateGlobalShaderConstants_AmbientOcclusion(this.m_commandBuffer_Parameters.cmdBuffer);
      this.checkParamsChanged();
      if (this.m_paramsChanged)
      {
        this.m_commandBuffer_Occlusion.cmdBuffer.Clear();
        this.commandBuffer_FillComputeOcclusion(this.m_commandBuffer_Occlusion.cmdBuffer);
      }
      this.m_commandBuffer_Apply.cmdBuffer.Clear();
      if (this.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.Debug)
        this.commandBuffer_FillApplyDebug(this.m_commandBuffer_Apply.cmdBuffer);
      else if (this.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.PostEffect)
        this.commandBuffer_FillApplyPostEffect(this.m_commandBuffer_Apply.cmdBuffer);
      else
        this.commandBuffer_FillApplyDeferred(this.m_commandBuffer_Apply.cmdBuffer, !this.m_HDR);
      this.updateParams();
      ++this.m_sampleStep;
    }
    else
    {
      this.m_targetCamera = (Camera) ((Component) this).GetComponent<Camera>();
      this.Update();
    }
  }

  private void OnPostRender()
  {
    if (Object.op_Inequality((Object) this.m_occlusionDepthRT, (Object) null))
      this.m_occlusionDepthRT.MarkRestoreExpected();
    if (this.m_temporalAccumRT == null)
      return;
    foreach (RenderTexture renderTexture in this.m_temporalAccumRT)
      renderTexture.MarkRestoreExpected();
  }

  private int safeAllocateTemporaryRT(
    CommandBuffer cb,
    string propertyName,
    int width,
    int height,
    RenderTextureFormat format = 7,
    RenderTextureReadWrite readWrite = 0,
    FilterMode filterMode = 0)
  {
    int id = Shader.PropertyToID(propertyName);
    cb.GetTemporaryRT(id, width, height, 0, filterMode, format, readWrite);
    return id;
  }

  private void safeReleaseTemporaryRT(CommandBuffer cb, int id)
  {
    cb.ReleaseTemporaryRT(id);
  }

  private RenderTexture safeAllocateRT(
    string name,
    int width,
    int height,
    RenderTextureFormat format,
    RenderTextureReadWrite readWrite,
    FilterMode filterMode = 0,
    int antiAliasing = 1,
    bool aUseMipMap = false)
  {
    width = Mathf.Clamp(width, 1, 65536);
    height = Mathf.Clamp(height, 1, 65536);
    RenderTexture renderTexture1 = new RenderTexture(width, height, 0, format, readWrite);
    ((Object) renderTexture1).set_hideFlags((HideFlags) 52);
    RenderTexture renderTexture2 = renderTexture1;
    ((Object) renderTexture2).set_name(name);
    ((Texture) renderTexture2).set_filterMode(filterMode);
    ((Texture) renderTexture2).set_wrapMode((TextureWrapMode) 1);
    renderTexture2.set_antiAliasing(Mathf.Max(antiAliasing, 1));
    renderTexture2.set_useMipMap(aUseMipMap);
    renderTexture2.Create();
    return renderTexture2;
  }

  private void safeReleaseRT(ref RenderTexture rt)
  {
    if (!Object.op_Inequality((Object) rt, (Object) null))
      return;
    RenderTexture.set_active((RenderTexture) null);
    rt.Release();
    Object.DestroyImmediate((Object) rt);
    rt = (RenderTexture) null;
  }

  private void BeginSample(CommandBuffer cb, string name)
  {
    cb.BeginSample(name);
  }

  private void EndSample(CommandBuffer cb, string name)
  {
    cb.EndSample(name);
  }

  private void commandBuffer_FillComputeOcclusion(CommandBuffer cb)
  {
    this.BeginSample(cb, "AO 1 - ComputeOcclusion");
    if (this.PerPixelNormals == AmplifyOcclusionBase.PerPixelNormalSource.GBuffer || this.PerPixelNormals == AmplifyOcclusionBase.PerPixelNormalSource.GBufferOctaEncoded)
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_GBufferNormals, RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 12));
    Vector4 vector4;
    ((Vector4) ref vector4).\u002Ector(this.m_target.oneOverWidth, this.m_target.oneOverHeight, (float) this.m_target.width, (float) this.m_target.height);
    int pass = (int) ((int) this.SampleCount * AmplifyOcclusionBase.PerPixelNormalSourceCount + this.PerPixelNormals);
    if (this.CacheAware)
    {
      pass += 16;
      int id = 0;
      for (int index = 0; index < this.m_numberMips; ++index)
      {
        int width = this.m_target.width >> index + 1;
        int height = this.m_target.height >> index + 1;
        int num = this.safeAllocateTemporaryRT(cb, this.m_tmpMipString[index], width, height, (RenderTextureFormat) 14, (RenderTextureReadWrite) 1, (FilterMode) 1);
        cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit(num));
        this.PerformBlit(cb, AmplifyOcclusionBase.m_occlusionMat, index != 0 ? 35 : 36);
        cb.CopyTexture(RenderTargetIdentifier.op_Implicit(num), 0, 0, RenderTargetIdentifier.op_Implicit((Texture) this.m_depthMipmap), 0, index);
        if (id != 0)
          this.safeReleaseTemporaryRT(cb, id);
        id = num;
        cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_Source, RenderTargetIdentifier.op_Implicit(num));
      }
      this.safeReleaseTemporaryRT(cb, id);
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_SourceDepthMipmap, RenderTargetIdentifier.op_Implicit((Texture) this.m_depthMipmap));
    }
    if (this.Downsample)
    {
      int id = this.safeAllocateTemporaryRT(cb, "_AO_SmallOcclusionTexture", this.m_target.width / 2, this.m_target.height / 2, this.m_occlusionRTFormat, (RenderTextureReadWrite) 1, (FilterMode) 1);
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_Source_TexelSize, vector4);
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_Target_TexelSize, new Vector4((float) (1.0 / ((double) this.m_target.width / 2.0)), (float) (1.0 / ((double) this.m_target.height / 2.0)), (float) this.m_target.width / 2f, (float) this.m_target.height / 2f));
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit(id));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_occlusionMat, pass);
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
      this.EndSample(cb, "AO 1 - ComputeOcclusion");
      if (this.BlurEnabled)
        this.commandBuffer_Blur(cb, RenderTargetIdentifier.op_Implicit(id), this.m_target.width / 2, this.m_target.height / 2);
      this.BeginSample(cb, "AO 2b - Combine");
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_Source, RenderTargetIdentifier.op_Implicit(id));
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_Target_TexelSize, vector4);
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_occlusionMat, 32);
      this.safeReleaseTemporaryRT(cb, id);
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
      this.EndSample(cb, "AO 2b - Combine");
    }
    else
    {
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_Source_TexelSize, vector4);
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_Target_TexelSize, vector4);
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_occlusionMat, pass);
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
      this.EndSample(cb, "AO 1 - ComputeOcclusion");
      if (!this.BlurEnabled)
        return;
      this.commandBuffer_Blur(cb, RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT), this.m_target.width, this.m_target.height);
    }
  }

  private int commandBuffer_NeighborMotionIntensity(
    CommandBuffer cb,
    int aSourceWidth,
    int aSourceHeight)
  {
    int num = this.safeAllocateTemporaryRT(cb, "_AO_IntensityTmp", aSourceWidth / 4, aSourceHeight / 4, this.m_motionIntensityRTFormat, (RenderTextureReadWrite) 1, (FilterMode) 1);
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit(num));
    cb.SetGlobalVector("_AO_Target_TexelSize", new Vector4((float) (1.0 / ((double) aSourceWidth / 4.0)), (float) (1.0 / ((double) aSourceHeight / 4.0)), (float) aSourceWidth / 4f, (float) aSourceHeight / 4f));
    this.PerformBlit(cb, AmplifyOcclusionBase.m_occlusionMat, 33);
    int id = this.safeAllocateTemporaryRT(cb, "_AO_BlurIntensityTmp", aSourceWidth / 4, aSourceHeight / 4, this.m_motionIntensityRTFormat, (RenderTextureReadWrite) 1, (FilterMode) 1);
    cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_Source, RenderTargetIdentifier.op_Implicit(num));
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit(id));
    this.PerformBlit(cb, AmplifyOcclusionBase.m_blurMat, 8);
    cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_Source, RenderTargetIdentifier.op_Implicit(id));
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit(num));
    this.PerformBlit(cb, AmplifyOcclusionBase.m_blurMat, 9);
    this.safeReleaseTemporaryRT(cb, id);
    cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_CurrMotionIntensity, RenderTargetIdentifier.op_Implicit(num));
    return num;
  }

  private void commandBuffer_Blur(
    CommandBuffer cb,
    RenderTargetIdentifier aSourceRT,
    int aSourceWidth,
    int aSourceHeight)
  {
    this.BeginSample(cb, "AO 2 - Blur");
    int id = this.safeAllocateTemporaryRT(cb, "_AO_BlurTmp", aSourceWidth, aSourceHeight, this.m_occlusionRTFormat, (RenderTextureReadWrite) 1, (FilterMode) 1);
    for (int index = 0; index < this.BlurPasses; ++index)
    {
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_Source, aSourceRT);
      int pass1 = (this.BlurRadius - 1) * 2;
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit(id));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_blurMat, pass1);
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_Source, RenderTargetIdentifier.op_Implicit(id));
      int pass2 = 1 + (this.BlurRadius - 1) * 2;
      cb.SetRenderTarget(aSourceRT);
      this.PerformBlit(cb, AmplifyOcclusionBase.m_blurMat, pass2);
    }
    this.safeReleaseTemporaryRT(cb, id);
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
    this.EndSample(cb, "AO 2 - Blur");
  }

  private int getTemporalPass()
  {
    return this.UsingMotionVectors && this.m_sampleStep > 1U ? 1 : 0;
  }

  private void commandBuffer_TemporalFilter(CommandBuffer cb)
  {
    if (this.m_clearHistory)
      this.ClearHistory(cb);
    float num = Mathf.Lerp(0.01f, 0.99f, this.FilterBlending);
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_TemporalCurveAdj, num);
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_TemporalMotionSensibility, (float) ((double) this.FilterResponse * (double) this.FilterResponse + 0.00999999977648258));
    cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_CurrOcclusionDepth, RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
    cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_TemporalAccumm, RenderTargetIdentifier.op_Implicit((Texture) this.m_temporalAccumRT[(IntPtr) this.m_prevTemporalIdx]));
  }

  private void commandBuffer_TemporalFilterDirectionsOffsets(CommandBuffer cb)
  {
    float temporalRotation = AmplifyOcclusionBase.m_temporalRotations[(IntPtr) (this.m_sampleStep % 6U)];
    float spatialOffset = AmplifyOcclusionBase.m_spatialOffsets[(IntPtr) (this.m_sampleStep / 6U % 4U)];
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_TemporalDirections, temporalRotation / 360f);
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_TemporalOffsets, spatialOffset);
  }

  private void commandBuffer_FillApplyDeferred(CommandBuffer cb, bool logTarget)
  {
    this.BeginSample(cb, "AO 3 - ApplyDeferred");
    if (!logTarget)
    {
      if (this.UsingTemporalFilter)
      {
        this.commandBuffer_TemporalFilter(cb);
        int id1 = 0;
        if (this.UsingMotionVectors)
          id1 = this.commandBuffer_NeighborMotionIntensity(cb, this.m_target.width, this.m_target.height);
        int id2 = 0;
        if (this.useMRTBlendingFallback)
        {
          id2 = this.safeAllocateTemporaryRT(cb, "_AO_ApplyOcclusionTexture", this.m_target.fullWidth, this.m_target.fullHeight, (RenderTextureFormat) 0, (RenderTextureReadWrite) 0, (FilterMode) 0);
          this.applyOcclusionTemporal[0] = RenderTargetIdentifier.op_Implicit(id2);
          this.applyOcclusionTemporal[1] = new RenderTargetIdentifier((Texture) this.m_temporalAccumRT[(IntPtr) this.m_curTemporalIdx]);
          cb.SetRenderTarget(this.applyOcclusionTemporal, this.applyOcclusionTemporal[0]);
          this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 10 + this.getTemporalPass());
        }
        else
        {
          this.applyDeferredTargetsTemporal[0] = this.m_applyDeferredTargets[0];
          this.applyDeferredTargetsTemporal[1] = this.m_applyDeferredTargets[1];
          this.applyDeferredTargetsTemporal[2] = new RenderTargetIdentifier((Texture) this.m_temporalAccumRT[(IntPtr) this.m_curTemporalIdx]);
          cb.SetRenderTarget(this.applyDeferredTargetsTemporal, this.applyDeferredTargetsTemporal[0]);
          this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 4 + this.getTemporalPass());
        }
        if (this.useMRTBlendingFallback)
        {
          cb.SetGlobalTexture("_AO_ApplyOcclusionTexture", RenderTargetIdentifier.op_Implicit(id2));
          this.applyOcclusionTemporal[0] = this.m_applyDeferredTargets[0];
          this.applyOcclusionTemporal[1] = this.m_applyDeferredTargets[1];
          cb.SetRenderTarget(this.applyOcclusionTemporal, this.applyOcclusionTemporal[0]);
          this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 13);
          this.safeReleaseTemporaryRT(cb, id2);
        }
        if (this.UsingMotionVectors)
          this.safeReleaseTemporaryRT(cb, id1);
      }
      else
      {
        cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_OcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
        cb.SetRenderTarget(this.m_applyDeferredTargets, this.m_applyDeferredTargets[0]);
        this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 3);
      }
    }
    else
    {
      int id1 = this.safeAllocateTemporaryRT(cb, "_AO_tmpAlbedo", this.m_target.fullWidth, this.m_target.fullHeight, (RenderTextureFormat) 0, (RenderTextureReadWrite) 0, (FilterMode) 0);
      int id2 = this.safeAllocateTemporaryRT(cb, "_AO_tmpEmission", this.m_target.fullWidth, this.m_target.fullHeight, this.m_temporaryEmissionRTFormat, (RenderTextureReadWrite) 0, (FilterMode) 0);
      cb.Blit(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 10), RenderTargetIdentifier.op_Implicit(id1));
      cb.Blit(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 13), RenderTargetIdentifier.op_Implicit(id2));
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_GBufferAlbedo, RenderTargetIdentifier.op_Implicit(id1));
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_GBufferEmission, RenderTargetIdentifier.op_Implicit(id2));
      if (this.UsingTemporalFilter)
      {
        this.commandBuffer_TemporalFilter(cb);
        int id3 = 0;
        if (this.UsingMotionVectors)
          id3 = this.commandBuffer_NeighborMotionIntensity(cb, this.m_target.width, this.m_target.height);
        this.applyDeferredTargets_Log_Temporal[0] = this.m_applyDeferredTargets_Log[0];
        this.applyDeferredTargets_Log_Temporal[1] = this.m_applyDeferredTargets_Log[1];
        this.applyDeferredTargets_Log_Temporal[2] = new RenderTargetIdentifier((Texture) this.m_temporalAccumRT[(IntPtr) this.m_curTemporalIdx]);
        cb.SetRenderTarget(this.applyDeferredTargets_Log_Temporal, this.applyDeferredTargets_Log_Temporal[0]);
        this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 7 + this.getTemporalPass());
        if (this.UsingMotionVectors)
          this.safeReleaseTemporaryRT(cb, id3);
      }
      else
      {
        cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_OcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
        cb.SetRenderTarget(this.m_applyDeferredTargets_Log, this.m_applyDeferredTargets_Log[0]);
        this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 6);
      }
      this.safeReleaseTemporaryRT(cb, id1);
      this.safeReleaseTemporaryRT(cb, id2);
    }
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
    this.EndSample(cb, "AO 3 - ApplyDeferred");
  }

  private void commandBuffer_FillApplyPostEffect(CommandBuffer cb)
  {
    this.BeginSample(cb, "AO 3 - ApplyPostEffect");
    if (this.UsingTemporalFilter)
    {
      this.commandBuffer_TemporalFilter(cb);
      int id1 = 0;
      if (this.UsingMotionVectors)
        id1 = this.commandBuffer_NeighborMotionIntensity(cb, this.m_target.width, this.m_target.height);
      int id2 = 0;
      if (this.useMRTBlendingFallback)
      {
        id2 = this.safeAllocateTemporaryRT(cb, "_AO_ApplyOcclusionTexture", this.m_target.fullWidth, this.m_target.fullHeight, (RenderTextureFormat) 0, (RenderTextureReadWrite) 0, (FilterMode) 0);
        this.applyPostEffectTargetsTemporal[0] = RenderTargetIdentifier.op_Implicit(id2);
      }
      else
        this.applyPostEffectTargetsTemporal[0] = RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 2);
      this.applyPostEffectTargetsTemporal[1] = new RenderTargetIdentifier((Texture) this.m_temporalAccumRT[(IntPtr) this.m_curTemporalIdx]);
      cb.SetRenderTarget(this.applyPostEffectTargetsTemporal, this.applyPostEffectTargetsTemporal[0]);
      this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 10 + this.getTemporalPass());
      if (this.useMRTBlendingFallback)
      {
        cb.SetGlobalTexture("_AO_ApplyOcclusionTexture", RenderTargetIdentifier.op_Implicit(id2));
        cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 2));
        this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 12);
        this.safeReleaseTemporaryRT(cb, id2);
      }
      if (this.UsingMotionVectors)
        this.safeReleaseTemporaryRT(cb, id1);
    }
    else
    {
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_OcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 2));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 9);
    }
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
    this.EndSample(cb, "AO 3 - ApplyPostEffect");
  }

  private void commandBuffer_FillApplyDebug(CommandBuffer cb)
  {
    this.BeginSample(cb, "AO 3 - ApplyDebug");
    if (this.UsingTemporalFilter)
    {
      this.commandBuffer_TemporalFilter(cb);
      int id = 0;
      if (this.UsingMotionVectors)
        id = this.commandBuffer_NeighborMotionIntensity(cb, this.m_target.width, this.m_target.height);
      this.applyDebugTargetsTemporal[0] = RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 2);
      this.applyDebugTargetsTemporal[1] = new RenderTargetIdentifier((Texture) this.m_temporalAccumRT[(IntPtr) this.m_curTemporalIdx]);
      cb.SetRenderTarget(this.applyDebugTargetsTemporal, this.applyDebugTargetsTemporal[0]);
      this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 1 + this.getTemporalPass());
      if (this.UsingMotionVectors)
        this.safeReleaseTemporaryRT(cb, id);
    }
    else
    {
      cb.SetGlobalTexture(AmplifyOcclusionBase.PropertyID._AO_OcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture) this.m_occlusionDepthRT));
      cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 2));
      this.PerformBlit(cb, AmplifyOcclusionBase.m_applyOcclusionMat, 0);
    }
    cb.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) null));
    this.EndSample(cb, "AO 3 - ApplyDebug");
  }

  private bool isStereoSinglePassEnabled()
  {
    if (!this.m_targetCamera.get_stereoEnabled())
      return false;
    RenderTextureDescriptor eyeTextureDesc = XRSettings.get_eyeTextureDesc();
    return ((RenderTextureDescriptor) ref eyeTextureDesc).get_vrUsage() == 2;
  }

  private bool isStereoMultiPassEnabled()
  {
    if (!this.m_targetCamera.get_stereoEnabled())
      return false;
    RenderTextureDescriptor eyeTextureDesc = XRSettings.get_eyeTextureDesc();
    return ((RenderTextureDescriptor) ref eyeTextureDesc).get_vrUsage() == 1;
  }

  private void UpdateGlobalShaderConstants(CommandBuffer cb)
  {
    if (XRSettings.get_enabled())
    {
      ref AmplifyOcclusionBase.TargetDesc local1 = ref this.m_target;
      RenderTextureDescriptor eyeTextureDesc1 = XRSettings.get_eyeTextureDesc();
      int num1 = (int) ((double) ((RenderTextureDescriptor) ref eyeTextureDesc1).get_width() * (double) XRSettings.get_eyeTextureResolutionScale());
      local1.fullWidth = num1;
      ref AmplifyOcclusionBase.TargetDesc local2 = ref this.m_target;
      RenderTextureDescriptor eyeTextureDesc2 = XRSettings.get_eyeTextureDesc();
      int num2 = (int) ((double) ((RenderTextureDescriptor) ref eyeTextureDesc2).get_height() * (double) XRSettings.get_eyeTextureResolutionScale());
      local2.fullHeight = num2;
    }
    else
    {
      this.m_target.fullWidth = this.m_targetCamera.get_pixelWidth();
      this.m_target.fullHeight = this.m_targetCamera.get_pixelHeight();
    }
    this.m_target.width = this.m_target.fullWidth;
    this.m_target.height = this.m_target.fullHeight;
    this.m_target.oneOverWidth = 1f / (float) this.m_target.width;
    this.m_target.oneOverHeight = 1f / (float) this.m_target.height;
    float num3 = this.m_targetCamera.get_fieldOfView() * ((float) Math.PI / 180f);
    float num4 = 1f / Mathf.Tan(num3 * 0.5f);
    Vector2 vector2_1;
    ((Vector2) ref vector2_1).\u002Ector(num4 * ((float) this.m_target.height / (float) this.m_target.width), num4);
    Vector2 vector2_2;
    ((Vector2) ref vector2_2).\u002Ector((float) (1.0 / vector2_1.x), (float) (1.0 / vector2_1.y));
    cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_UVToView, new Vector4((float) (2.0 * vector2_2.x), (float) (2.0 * vector2_2.y), (float) (-1.0 * vector2_2.x), (float) (-1.0 * vector2_2.y)));
    float num5 = !this.m_targetCamera.get_orthographic() ? (float) this.m_target.height / (Mathf.Tan(num3 * 0.5f) * 2f) : (float) this.m_target.height / this.m_targetCamera.get_orthographicSize();
    float num6 = !this.Downsample ? num5 * 0.5f : (float) ((double) num5 * 0.5 * 0.5);
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_HalfProjScale, num6);
  }

  private void UpdateGlobalShaderConstants_AmbientOcclusion(CommandBuffer cb)
  {
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_Radius, this.Radius);
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_PowExponent, this.PowerExponent);
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_Bias, this.Bias * this.Bias);
    cb.SetGlobalColor(AmplifyOcclusionBase.PropertyID._AO_Levels, new Color((float) this.Tint.r, (float) this.Tint.g, (float) this.Tint.b, this.Intensity));
    float num1 = 1f - this.Thickness;
    cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_ThicknessDecay, (float) ((1.0 - (double) num1 * (double) num1) * 0.980000019073486));
    if (this.BlurEnabled)
      cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_BlurSharpness, this.BlurSharpness * 100f);
    if (this.FadeEnabled)
    {
      this.FadeStart = Mathf.Max(0.0f, this.FadeStart);
      this.FadeLength = Mathf.Max(0.01f, this.FadeLength);
      float num2 = 1f / this.FadeLength;
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_FadeParams, Vector4.op_Implicit(new Vector2(this.FadeStart, num2)));
      float num3 = 1f - this.FadeToThickness;
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_FadeValues, new Vector4(this.FadeToIntensity, this.FadeToRadius, this.FadeToPowerExponent, (float) ((1.0 - (double) num3 * (double) num3) * 0.980000019073486)));
      cb.SetGlobalColor(AmplifyOcclusionBase.PropertyID._AO_FadeToTint, new Color((float) this.FadeToTint.r, (float) this.FadeToTint.g, (float) this.FadeToTint.b, 0.0f));
    }
    else
      cb.SetGlobalVector(AmplifyOcclusionBase.PropertyID._AO_FadeParams, Vector4.op_Implicit(new Vector2(0.0f, 0.0f)));
    if (this.FilterEnabled)
    {
      this.commandBuffer_TemporalFilterDirectionsOffsets(cb);
    }
    else
    {
      cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_TemporalDirections, 0.0f);
      cb.SetGlobalFloat(AmplifyOcclusionBase.PropertyID._AO_TemporalOffsets, 0.0f);
    }
  }

  private void UpdateGlobalShaderConstants_Matrices(CommandBuffer cb)
  {
    if (this.isStereoSinglePassEnabled())
    {
      Matrix4x4 stereoViewMatrix1 = this.m_targetCamera.GetStereoViewMatrix((Camera.StereoscopicEye) 0);
      Matrix4x4 stereoViewMatrix2 = this.m_targetCamera.GetStereoViewMatrix((Camera.StereoscopicEye) 1);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_CameraViewLeft, stereoViewMatrix1);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_CameraViewRight, stereoViewMatrix2);
      Matrix4x4 projectionMatrix1 = this.m_targetCamera.GetStereoProjectionMatrix((Camera.StereoscopicEye) 0);
      Matrix4x4 projectionMatrix2 = this.m_targetCamera.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1);
      Matrix4x4 projectionMatrix3 = GL.GetGPUProjectionMatrix(projectionMatrix1, false);
      Matrix4x4 projectionMatrix4 = GL.GetGPUProjectionMatrix(projectionMatrix2, false);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_ProjMatrixLeft, projectionMatrix3);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_ProjMatrixRight, projectionMatrix4);
      if (!this.UsingTemporalFilter)
        return;
      Matrix4x4 matrix4x4_1 = Matrix4x4.op_Multiply(projectionMatrix3, stereoViewMatrix1);
      Matrix4x4 matrix4x4_2 = Matrix4x4.op_Multiply(projectionMatrix4, stereoViewMatrix2);
      Matrix4x4 matrix4x4_3 = Matrix4x4.Inverse(matrix4x4_1);
      Matrix4x4 matrix4x4_4 = Matrix4x4.Inverse(matrix4x4_2);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_InvViewProjMatrixLeft, matrix4x4_3);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_PrevViewProjMatrixLeft, this.m_prevViewProjMatrixLeft);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_PrevInvViewProjMatrixLeft, this.m_prevInvViewProjMatrixLeft);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_InvViewProjMatrixRight, matrix4x4_4);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_PrevViewProjMatrixRight, this.m_prevViewProjMatrixRight);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_PrevInvViewProjMatrixRight, this.m_prevInvViewProjMatrixRight);
      this.m_prevViewProjMatrixLeft = matrix4x4_1;
      this.m_prevInvViewProjMatrixLeft = matrix4x4_3;
      this.m_prevViewProjMatrixRight = matrix4x4_2;
      this.m_prevInvViewProjMatrixRight = matrix4x4_4;
    }
    else
    {
      Matrix4x4 worldToCameraMatrix = this.m_targetCamera.get_worldToCameraMatrix();
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_CameraViewLeft, worldToCameraMatrix);
      if (!this.UsingTemporalFilter)
        return;
      Matrix4x4 matrix4x4_1 = Matrix4x4.op_Multiply(GL.GetGPUProjectionMatrix(this.m_targetCamera.get_projectionMatrix(), false), worldToCameraMatrix);
      Matrix4x4 matrix4x4_2 = Matrix4x4.Inverse(matrix4x4_1);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_InvViewProjMatrixLeft, matrix4x4_2);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_PrevViewProjMatrixLeft, this.m_prevViewProjMatrixLeft);
      cb.SetGlobalMatrix(AmplifyOcclusionBase.PropertyID._AO_PrevInvViewProjMatrixLeft, this.m_prevInvViewProjMatrixLeft);
      this.m_prevViewProjMatrixLeft = matrix4x4_1;
      this.m_prevInvViewProjMatrixLeft = matrix4x4_2;
    }
  }

  public enum ApplicationMethod
  {
    PostEffect,
    Deferred,
    Debug,
  }

  public enum PerPixelNormalSource
  {
    None,
    Camera,
    GBuffer,
    GBufferOctaEncoded,
  }

  public enum SampleCountLevel
  {
    Low,
    Medium,
    High,
    VeryHigh,
  }

  private struct CmdBuffer
  {
    public CommandBuffer cmdBuffer;
    public CameraEvent cmdBufferEvent;
    public string cmdBufferName;
  }

  private struct TargetDesc
  {
    public int fullWidth;
    public int fullHeight;
    public int width;
    public int height;
    public float oneOverWidth;
    public float oneOverHeight;
  }

  private static class ShaderPass
  {
    public const int CombineDownsampledOcclusionDepth = 32;
    public const int NeighborMotionIntensity = 33;
    public const int ClearTemporal = 34;
    public const int ScaleDownCloserDepthEven = 35;
    public const int ScaleDownCloserDepthEven_CameraDepthTexture = 36;
    public const int BlurHorizontal1 = 0;
    public const int BlurVertical1 = 1;
    public const int BlurHorizontal2 = 2;
    public const int BlurVertical2 = 3;
    public const int BlurHorizontal3 = 4;
    public const int BlurVertical3 = 5;
    public const int BlurHorizontal4 = 6;
    public const int BlurVertical4 = 7;
    public const int BlurHorizontalIntensity = 8;
    public const int BlurVerticalIntensity = 9;
    public const int ApplyDebug = 0;
    public const int ApplyDebugTemporal = 1;
    public const int ApplyDeferred = 3;
    public const int ApplyDeferredTemporal = 4;
    public const int ApplyDeferredLog = 6;
    public const int ApplyDeferredLogTemporal = 7;
    public const int ApplyPostEffect = 9;
    public const int ApplyPostEffectTemporal = 10;
    public const int ApplyPostEffectTemporalMultiply = 12;
    public const int ApplyDeferredTemporalMultiply = 13;
    public const int OcclusionLow_None = 0;
    public const int OcclusionLow_Camera = 1;
    public const int OcclusionLow_GBuffer = 2;
    public const int OcclusionLow_GBufferOctaEncoded = 3;
    public const int OcclusionLow_None_UseDynamicDepthMips = 16;
  }

  private static class PropertyID
  {
    public static readonly int _AO_Radius = Shader.PropertyToID(nameof (_AO_Radius));
    public static readonly int _AO_PowExponent = Shader.PropertyToID(nameof (_AO_PowExponent));
    public static readonly int _AO_Bias = Shader.PropertyToID(nameof (_AO_Bias));
    public static readonly int _AO_Levels = Shader.PropertyToID(nameof (_AO_Levels));
    public static readonly int _AO_ThicknessDecay = Shader.PropertyToID(nameof (_AO_ThicknessDecay));
    public static readonly int _AO_BlurSharpness = Shader.PropertyToID(nameof (_AO_BlurSharpness));
    public static readonly int _AO_CameraViewLeft = Shader.PropertyToID(nameof (_AO_CameraViewLeft));
    public static readonly int _AO_CameraViewRight = Shader.PropertyToID(nameof (_AO_CameraViewRight));
    public static readonly int _AO_ProjMatrixLeft = Shader.PropertyToID(nameof (_AO_ProjMatrixLeft));
    public static readonly int _AO_ProjMatrixRight = Shader.PropertyToID(nameof (_AO_ProjMatrixRight));
    public static readonly int _AO_InvViewProjMatrixLeft = Shader.PropertyToID(nameof (_AO_InvViewProjMatrixLeft));
    public static readonly int _AO_PrevViewProjMatrixLeft = Shader.PropertyToID(nameof (_AO_PrevViewProjMatrixLeft));
    public static readonly int _AO_PrevInvViewProjMatrixLeft = Shader.PropertyToID(nameof (_AO_PrevInvViewProjMatrixLeft));
    public static readonly int _AO_InvViewProjMatrixRight = Shader.PropertyToID(nameof (_AO_InvViewProjMatrixRight));
    public static readonly int _AO_PrevViewProjMatrixRight = Shader.PropertyToID(nameof (_AO_PrevViewProjMatrixRight));
    public static readonly int _AO_PrevInvViewProjMatrixRight = Shader.PropertyToID(nameof (_AO_PrevInvViewProjMatrixRight));
    public static readonly int _AO_GBufferNormals = Shader.PropertyToID(nameof (_AO_GBufferNormals));
    public static readonly int _AO_Target_TexelSize = Shader.PropertyToID(nameof (_AO_Target_TexelSize));
    public static readonly int _AO_TemporalCurveAdj = Shader.PropertyToID(nameof (_AO_TemporalCurveAdj));
    public static readonly int _AO_TemporalMotionSensibility = Shader.PropertyToID(nameof (_AO_TemporalMotionSensibility));
    public static readonly int _AO_CurrOcclusionDepth = Shader.PropertyToID(nameof (_AO_CurrOcclusionDepth));
    public static readonly int _AO_TemporalAccumm = Shader.PropertyToID(nameof (_AO_TemporalAccumm));
    public static readonly int _AO_TemporalDirections = Shader.PropertyToID(nameof (_AO_TemporalDirections));
    public static readonly int _AO_TemporalOffsets = Shader.PropertyToID(nameof (_AO_TemporalOffsets));
    public static readonly int _AO_OcclusionTexture = Shader.PropertyToID(nameof (_AO_OcclusionTexture));
    public static readonly int _AO_GBufferAlbedo = Shader.PropertyToID(nameof (_AO_GBufferAlbedo));
    public static readonly int _AO_GBufferEmission = Shader.PropertyToID(nameof (_AO_GBufferEmission));
    public static readonly int _AO_UVToView = Shader.PropertyToID(nameof (_AO_UVToView));
    public static readonly int _AO_HalfProjScale = Shader.PropertyToID(nameof (_AO_HalfProjScale));
    public static readonly int _AO_FadeParams = Shader.PropertyToID(nameof (_AO_FadeParams));
    public static readonly int _AO_FadeValues = Shader.PropertyToID(nameof (_AO_FadeValues));
    public static readonly int _AO_FadeToTint = Shader.PropertyToID(nameof (_AO_FadeToTint));
    public static readonly int _AO_Source_TexelSize = Shader.PropertyToID(nameof (_AO_Source_TexelSize));
    public static readonly int _AO_Source = Shader.PropertyToID(nameof (_AO_Source));
    public static readonly int _AO_CurrMotionIntensity = Shader.PropertyToID(nameof (_AO_CurrMotionIntensity));
    public static readonly int _AO_SourceDepthMipmap = Shader.PropertyToID(nameof (_AO_SourceDepthMipmap));
  }
}
