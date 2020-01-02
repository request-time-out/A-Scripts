// Decompiled with JetBrains decompiler
// Type: EnviroSkyRendering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof (Camera))]
public class EnviroSkyRendering : MonoBehaviour
{
  [HideInInspector]
  public bool isAddionalCamera;
  private Camera myCam;
  private RenderTexture spSatTex;
  private Camera spSatCam;
  private Material mat;
  private Material blitMat;
  private RenderTexture subFrameTex;
  private RenderTexture prevFrameTex;
  private Texture2D curlMap;
  private Texture3D noiseTexture;
  private Texture3D noiseTextureHigh;
  private Texture3D detailNoiseTexture;
  private Texture3D detailNoiseTextureHigh;
  private Matrix4x4 projection;
  private Matrix4x4 projectionSPVR;
  private Matrix4x4 inverseRotation;
  private Matrix4x4 inverseRotationSPVR;
  private Matrix4x4 rotation;
  private Matrix4x4 rotationSPVR;
  private Matrix4x4 previousRotation;
  private Matrix4x4 previousRotationSPVR;
  [HideInInspector]
  public EnviroCloudSettings.ReprojectionPixelSize currentReprojectionPixelSize;
  private int reprojectionPixelSize;
  private bool isFirstFrame;
  private int subFrameNumber;
  private int[] frameList;
  private int renderingCounter;
  private int subFrameWidth;
  private int subFrameHeight;
  private int frameWidth;
  private int frameHeight;
  private bool textureDimensionChanged;
  private static Mesh _pointLightMesh;
  private static Mesh _spotLightMesh;
  private static Material _lightMaterial;
  private CommandBuffer _preLightPass;
  public CommandBuffer _afterLightPass;
  private Matrix4x4 _viewProj;
  private Matrix4x4 _viewProjSP;
  [HideInInspector]
  public Material _volumeRenderingMaterial;
  private Material _bilateralBlurMaterial;
  private RenderTexture _volumeLightTexture;
  private RenderTexture _halfVolumeLightTexture;
  private RenderTexture _quarterVolumeLightTexture;
  private static Texture _defaultSpotCookie;
  private RenderTexture _halfDepthBuffer;
  private RenderTexture _quarterDepthBuffer;
  private Texture2D _ditheringTexture;
  private Texture2D blackTexture;
  private Texture3D _noiseTexture;
  [HideInInspector]
  public EnviroSkyRendering.VolumtericResolution Resolution;
  private EnviroSkyRendering.VolumtericResolution _currentResolution;
  [HideInInspector]
  public Texture DefaultSpotCookie;
  private Material _material;
  [HideInInspector]
  public bool simpleFog;
  private bool currentSimpleFog;
  [HideInInspector]
  public bool volumeLighting;
  [HideInInspector]
  public bool dirVolumeLighting;
  [HideInInspector]
  public bool distanceFog;
  [HideInInspector]
  public bool useRadialDistance;
  [HideInInspector]
  public bool heightFog;
  [HideInInspector]
  public float height;
  [Range(0.001f, 10f)]
  [HideInInspector]
  public float heightDensity;
  [HideInInspector]
  public float startDistance;

  public EnviroSkyRendering()
  {
    base.\u002Ector();
  }

  public static event Action<EnviroSkyRendering, Matrix4x4, Matrix4x4> PreRenderEvent;

  public CommandBuffer GlobalCommandBuffer
  {
    get
    {
      return this._preLightPass;
    }
  }

  public CommandBuffer GlobalCommandBufferForward
  {
    get
    {
      return this._afterLightPass;
    }
  }

  public static Material GetLightMaterial()
  {
    return EnviroSkyRendering._lightMaterial;
  }

  public static Mesh GetPointLightMesh()
  {
    return EnviroSkyRendering._pointLightMesh;
  }

  public static Mesh GetSpotLightMesh()
  {
    return EnviroSkyRendering._spotLightMesh;
  }

  public RenderTexture GetVolumeLightBuffer()
  {
    if (EnviroSky.instance.volumeLightSettings.Resolution == EnviroSkyRendering.VolumtericResolution.Quarter)
      return this._quarterVolumeLightTexture;
    return EnviroSky.instance.volumeLightSettings.Resolution == EnviroSkyRendering.VolumtericResolution.Half ? this._halfVolumeLightTexture : this._volumeLightTexture;
  }

  public RenderTexture GetVolumeLightDepthBuffer()
  {
    if (EnviroSky.instance.volumeLightSettings.Resolution == EnviroSkyRendering.VolumtericResolution.Quarter)
      return this._quarterDepthBuffer;
    return EnviroSky.instance.volumeLightSettings.Resolution == EnviroSkyRendering.VolumtericResolution.Half ? this._halfDepthBuffer : (RenderTexture) null;
  }

  public static Texture GetDefaultSpotCookie()
  {
    return EnviroSkyRendering._defaultSpotCookie;
  }

  private void Awake()
  {
    this.myCam = (Camera) ((Component) this).GetComponent<Camera>();
    if (this.myCam.get_actualRenderingPath() == 1)
      this.myCam.set_depthTextureMode((DepthTextureMode) 1);
    this._currentResolution = this.Resolution;
    this._material = new Material(Shader.Find("Enviro/VolumeLight"));
    Shader shader1 = Shader.Find("Hidden/EnviroBilateralBlur");
    if (Object.op_Equality((Object) shader1, (Object) null))
      throw new Exception("Critical Error: \"Hidden/EnviroBilateralBlur\" shader is missing.");
    this._bilateralBlurMaterial = new Material(shader1);
    this._preLightPass = new CommandBuffer();
    this._preLightPass.set_name("PreLight");
    this._afterLightPass = new CommandBuffer();
    this._afterLightPass.set_name("AfterLight");
    this.ChangeResolution();
    if (Object.op_Equality((Object) EnviroSkyRendering._pointLightMesh, (Object) null))
    {
      GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 0);
      EnviroSkyRendering._pointLightMesh = ((MeshFilter) primitive.GetComponent<MeshFilter>()).get_sharedMesh();
      Object.Destroy((Object) primitive);
    }
    if (Object.op_Equality((Object) EnviroSkyRendering._spotLightMesh, (Object) null))
      EnviroSkyRendering._spotLightMesh = this.CreateSpotLightMesh();
    if (Object.op_Equality((Object) EnviroSkyRendering._lightMaterial, (Object) null))
    {
      Shader shader2 = Shader.Find("Enviro/VolumeLight");
      if (Object.op_Equality((Object) shader2, (Object) null))
        throw new Exception("Critical Error: \"Enviro/VolumeLight\" shader is missing.");
      EnviroSkyRendering._lightMaterial = new Material(shader2);
    }
    if (Object.op_Equality((Object) EnviroSkyRendering._defaultSpotCookie, (Object) null))
      EnviroSkyRendering._defaultSpotCookie = this.DefaultSpotCookie;
    this.LoadNoise3dTexture();
    this.GenerateDitherTexture();
  }

  private void Start()
  {
    this.CreateMaterialsAndTextures();
    if (!Object.op_Inequality((Object) EnviroSky.instance, (Object) null))
      return;
    this.SetReprojectionPixelSize(EnviroSky.instance.cloudsSettings.reprojectionPixelSize);
  }

  private void OnEnable()
  {
    if (Object.op_Equality((Object) this.myCam, (Object) null))
      this.myCam = (Camera) ((Component) this).GetComponent<Camera>();
    if (this.myCam.get_actualRenderingPath() == 1)
    {
      this.myCam.AddCommandBuffer((CameraEvent) 1, this._preLightPass);
      this.myCam.AddCommandBuffer((CameraEvent) 11, this._afterLightPass);
    }
    else
      this.myCam.AddCommandBuffer((CameraEvent) 6, this._preLightPass);
    this.CreateFogMaterial();
  }

  private void OnDisable()
  {
    if (this.myCam.get_actualRenderingPath() == 1)
    {
      this.myCam.RemoveCommandBuffer((CameraEvent) 1, this._preLightPass);
      this.myCam.RemoveCommandBuffer((CameraEvent) 11, this._afterLightPass);
    }
    else
      this.myCam.RemoveCommandBuffer((CameraEvent) 6, this._preLightPass);
  }

  private void ChangeResolution()
  {
    int pixelWidth = this.myCam.get_pixelWidth();
    int pixelHeight = this.myCam.get_pixelHeight();
    if (Object.op_Inequality((Object) this._volumeLightTexture, (Object) null))
      Object.Destroy((Object) this._volumeLightTexture);
    this._volumeLightTexture = new RenderTexture(pixelWidth, pixelHeight, 0, (RenderTextureFormat) 2);
    ((Object) this._volumeLightTexture).set_name("VolumeLightBuffer");
    ((Texture) this._volumeLightTexture).set_filterMode((FilterMode) 1);
    if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
    {
      if (this.Resolution == EnviroSkyRendering.VolumtericResolution.Half || this.Resolution == EnviroSkyRendering.VolumtericResolution.Quarter)
        this._volumeLightTexture.set_vrUsage((VRTextureUsage) 0);
      else
        this._volumeLightTexture.set_vrUsage((VRTextureUsage) 2);
    }
    if (Object.op_Inequality((Object) this._halfDepthBuffer, (Object) null))
      Object.Destroy((Object) this._halfDepthBuffer);
    if (Object.op_Inequality((Object) this._halfVolumeLightTexture, (Object) null))
      Object.Destroy((Object) this._halfVolumeLightTexture);
    if (this.Resolution == EnviroSkyRendering.VolumtericResolution.Half || this.Resolution == EnviroSkyRendering.VolumtericResolution.Quarter)
    {
      this._halfVolumeLightTexture = new RenderTexture(pixelWidth / 2, pixelHeight / 2, 0, (RenderTextureFormat) 2);
      ((Object) this._halfVolumeLightTexture).set_name("VolumeLightBufferHalf");
      ((Texture) this._halfVolumeLightTexture).set_filterMode((FilterMode) 1);
      if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
        this._halfVolumeLightTexture.set_vrUsage((VRTextureUsage) 2);
      this._halfDepthBuffer = new RenderTexture(pixelWidth / 2, pixelHeight / 2, 0, (RenderTextureFormat) 14);
      ((Object) this._halfDepthBuffer).set_name("VolumeLightHalfDepth");
      this._halfDepthBuffer.Create();
      ((Texture) this._halfDepthBuffer).set_filterMode((FilterMode) 0);
    }
    if (Object.op_Inequality((Object) this._quarterVolumeLightTexture, (Object) null))
      Object.Destroy((Object) this._quarterVolumeLightTexture);
    if (Object.op_Inequality((Object) this._quarterDepthBuffer, (Object) null))
      Object.Destroy((Object) this._quarterDepthBuffer);
    if (this.Resolution != EnviroSkyRendering.VolumtericResolution.Quarter)
      return;
    this._quarterVolumeLightTexture = new RenderTexture(pixelWidth / 4, pixelHeight / 4, 0, (RenderTextureFormat) 2);
    ((Object) this._quarterVolumeLightTexture).set_name("VolumeLightBufferQuarter");
    ((Texture) this._quarterVolumeLightTexture).set_filterMode((FilterMode) 1);
    if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
      this._quarterVolumeLightTexture.set_vrUsage((VRTextureUsage) 2);
    this._quarterDepthBuffer = new RenderTexture(pixelWidth / 4, pixelHeight / 4, 0, (RenderTextureFormat) 14);
    ((Object) this._quarterDepthBuffer).set_name("VolumeLightQuarterDepth");
    this._quarterDepthBuffer.Create();
    ((Texture) this._quarterDepthBuffer).set_filterMode((FilterMode) 0);
  }

  private void CreateFogMaterial()
  {
    if (Object.op_Inequality((Object) this._volumeRenderingMaterial, (Object) null))
      Object.Destroy((Object) this._volumeRenderingMaterial);
    if (!this.simpleFog)
    {
      Shader shader = Shader.Find("Enviro/EnviroFogRendering");
      if (Object.op_Equality((Object) shader, (Object) null))
        throw new Exception("Critical Error: \"Enviro/EnviroFogRendering\" shader is missing.");
      this._volumeRenderingMaterial = new Material(shader);
    }
    else
    {
      Shader shader = Shader.Find("Enviro/EnviroFogRenderingSimple");
      if (Object.op_Equality((Object) shader, (Object) null))
        throw new Exception("Critical Error: \"Enviro/EnviroFogRendering\" shader is missing.");
      this._volumeRenderingMaterial = new Material(shader);
    }
  }

  private void CreateMaterialsAndTextures()
  {
    if (Object.op_Equality((Object) this.mat, (Object) null))
      this.mat = new Material(Shader.Find("Enviro/RaymarchClouds"));
    if (Object.op_Equality((Object) this.blitMat, (Object) null))
      this.blitMat = new Material(Shader.Find("Enviro/Blit"));
    if (Object.op_Equality((Object) this.curlMap, (Object) null))
      this.curlMap = Resources.Load("tex_enviro_curl") as Texture2D;
    if (Object.op_Equality((Object) this.blackTexture, (Object) null))
      this.blackTexture = Resources.Load("tex_enviro_black") as Texture2D;
    if (Object.op_Equality((Object) this.noiseTextureHigh, (Object) null))
      this.noiseTextureHigh = Resources.Load("enviro_clouds_base") as Texture3D;
    if (Object.op_Equality((Object) this.noiseTexture, (Object) null))
      this.noiseTexture = Resources.Load("enviro_clouds_base_low") as Texture3D;
    if (Object.op_Equality((Object) this.detailNoiseTexture, (Object) null))
      this.detailNoiseTexture = Resources.Load("enviro_clouds_detail_low") as Texture3D;
    if (!Object.op_Equality((Object) this.detailNoiseTextureHigh, (Object) null))
      return;
    this.detailNoiseTextureHigh = Resources.Load("enviro_clouds_detail_high") as Texture3D;
  }

  private void OnPreRender()
  {
    if (this.volumeLighting)
    {
      Matrix4x4.Perspective(this.myCam.get_fieldOfView(), this.myCam.get_aspect(), 0.01f, this.myCam.get_farClipPlane());
      Matrix4x4 matrix4x4 = Matrix4x4.Perspective(this.myCam.get_fieldOfView(), this.myCam.get_aspect(), 0.01f, this.myCam.get_farClipPlane());
      Matrix4x4 projectionMatrix;
      if (this.myCam.get_stereoEnabled())
      {
        projectionMatrix = GL.GetGPUProjectionMatrix(this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 0), true);
        matrix4x4 = GL.GetGPUProjectionMatrix(this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1), true);
      }
      else
        projectionMatrix = GL.GetGPUProjectionMatrix(Matrix4x4.Perspective(this.myCam.get_fieldOfView(), this.myCam.get_aspect(), 0.01f, this.myCam.get_farClipPlane()), true);
      if (this.myCam.get_stereoEnabled())
      {
        this._viewProj = Matrix4x4.op_Multiply(projectionMatrix, this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 0));
        this._viewProjSP = Matrix4x4.op_Multiply(matrix4x4, this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 1));
      }
      else
      {
        this._viewProj = Matrix4x4.op_Multiply(projectionMatrix, this.myCam.get_worldToCameraMatrix());
        this._viewProjSP = Matrix4x4.op_Multiply(matrix4x4, this.myCam.get_worldToCameraMatrix());
      }
      this._preLightPass.Clear();
      this._afterLightPass.Clear();
      bool flag = SystemInfo.get_graphicsShaderLevel() > 40;
      if (this.Resolution == EnviroSkyRendering.VolumtericResolution.Quarter)
      {
        Texture texture = (Texture) null;
        this._preLightPass.Blit(texture, RenderTargetIdentifier.op_Implicit((Texture) this._halfDepthBuffer), this._bilateralBlurMaterial, !flag ? 10 : 4);
        this._preLightPass.Blit(texture, RenderTargetIdentifier.op_Implicit((Texture) this._quarterDepthBuffer), this._bilateralBlurMaterial, !flag ? 11 : 6);
        this._preLightPass.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this._quarterVolumeLightTexture));
      }
      else if (this.Resolution == EnviroSkyRendering.VolumtericResolution.Half)
      {
        this._preLightPass.Blit((Texture) null, RenderTargetIdentifier.op_Implicit((Texture) this._halfDepthBuffer), this._bilateralBlurMaterial, !flag ? 10 : 4);
        this._preLightPass.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this._halfVolumeLightTexture));
      }
      else
        this._preLightPass.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) this._volumeLightTexture));
      this._preLightPass.ClearRenderTarget(false, true, new Color(0.0f, 0.0f, 0.0f, 1f));
      this.UpdateMaterialParameters();
      if (EnviroSkyRendering.PreRenderEvent != null)
        EnviroSkyRendering.PreRenderEvent(this, this._viewProj, this._viewProjSP);
    }
    if (this.myCam.get_stereoEnabled())
    {
      Matrix4x4 stereoViewMatrix1 = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 0);
      Matrix4x4 inverse1 = ((Matrix4x4) ref stereoViewMatrix1).get_inverse();
      Matrix4x4 stereoViewMatrix2 = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 1);
      Matrix4x4 inverse2 = ((Matrix4x4) ref stereoViewMatrix2).get_inverse();
      Matrix4x4 projectionMatrix1 = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 0);
      Matrix4x4 projectionMatrix2 = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1);
      Matrix4x4 projectionMatrix3 = GL.GetGPUProjectionMatrix(projectionMatrix1, true);
      Matrix4x4 inverse3 = ((Matrix4x4) ref projectionMatrix3).get_inverse();
      Matrix4x4 projectionMatrix4 = GL.GetGPUProjectionMatrix(projectionMatrix2, true);
      Matrix4x4 inverse4 = ((Matrix4x4) ref projectionMatrix4).get_inverse();
      if (SystemInfo.get_graphicsDeviceType() != 17 && SystemInfo.get_graphicsDeviceType() != 11)
      {
        // ISSUE: variable of a reference type
        Matrix4x4& local1;
        ((Matrix4x4) (local1 = ref inverse3)).set_Item(1, 1, ((Matrix4x4) ref local1).get_Item(1, 1) * -1f);
        // ISSUE: variable of a reference type
        Matrix4x4& local2;
        ((Matrix4x4) (local2 = ref inverse4)).set_Item(1, 1, ((Matrix4x4) ref local2).get_Item(1, 1) * -1f);
      }
      Shader.SetGlobalMatrix("_LeftWorldFromView", inverse1);
      Shader.SetGlobalMatrix("_RightWorldFromView", inverse2);
      Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse3);
      Shader.SetGlobalMatrix("_RightViewFromScreen", inverse4);
    }
    else
    {
      Matrix4x4 cameraToWorldMatrix = this.myCam.get_cameraToWorldMatrix();
      Matrix4x4 projectionMatrix = GL.GetGPUProjectionMatrix(this.myCam.get_projectionMatrix(), true);
      Matrix4x4 inverse = ((Matrix4x4) ref projectionMatrix).get_inverse();
      if (SystemInfo.get_graphicsDeviceType() != 17 && SystemInfo.get_graphicsDeviceType() != 11)
      {
        // ISSUE: variable of a reference type
        Matrix4x4& local;
        ((Matrix4x4) (local = ref inverse)).set_Item(1, 1, ((Matrix4x4) ref local).get_Item(1, 1) * -1f);
      }
      Shader.SetGlobalMatrix("_LeftWorldFromView", cameraToWorldMatrix);
      Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse);
    }
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null) || !Object.op_Inequality((Object) this.myCam, (Object) null))
      return;
    Camera.MonoOrStereoscopicEye stereoActiveEye = this.myCam.get_stereoActiveEye();
    if (stereoActiveEye != 2)
    {
      if (stereoActiveEye != null)
      {
        if (stereoActiveEye == 1 && Object.op_Inequality((Object) EnviroSky.instance.satCamera, (Object) null))
          this.RenderCamera(EnviroSky.instance.satCamera, (Camera.MonoOrStereoscopicEye) 1);
      }
      else if (Object.op_Inequality((Object) EnviroSky.instance.satCamera, (Object) null))
        this.RenderCamera(EnviroSky.instance.satCamera, (Camera.MonoOrStereoscopicEye) 0);
    }
    else if (Object.op_Inequality((Object) EnviroSky.instance.satCamera, (Object) null))
      this.RenderCamera(EnviroSky.instance.satCamera, (Camera.MonoOrStereoscopicEye) 2);
    if (!Object.op_Inequality((Object) EnviroSky.instance.satCamera, (Object) null))
      return;
    RenderSettings.get_skybox().SetTexture("_SatTex", (Texture) EnviroSky.instance.satCamera.get_targetTexture());
  }

  private void RenderCamera(Camera targetCam, Camera.MonoOrStereoscopicEye eye)
  {
    targetCam.set_fieldOfView(EnviroSky.instance.PlayerCamera.get_fieldOfView());
    targetCam.set_aspect(EnviroSky.instance.PlayerCamera.get_aspect());
    if (eye != 2)
    {
      if (eye != null)
      {
        if (eye != 1)
          return;
        ((Component) targetCam).get_transform().set_position(((Component) EnviroSky.instance.PlayerCamera).get_transform().get_position());
        ((Component) targetCam).get_transform().set_rotation(((Component) EnviroSky.instance.PlayerCamera).get_transform().get_rotation());
        targetCam.set_projectionMatrix(EnviroSky.instance.PlayerCamera.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1));
        targetCam.set_worldToCameraMatrix(EnviroSky.instance.PlayerCamera.GetStereoViewMatrix((Camera.StereoscopicEye) 1));
        targetCam.Render();
      }
      else
      {
        ((Component) targetCam).get_transform().set_position(((Component) EnviroSky.instance.PlayerCamera).get_transform().get_position());
        ((Component) targetCam).get_transform().set_rotation(((Component) EnviroSky.instance.PlayerCamera).get_transform().get_rotation());
        targetCam.set_projectionMatrix(EnviroSky.instance.PlayerCamera.GetStereoProjectionMatrix((Camera.StereoscopicEye) 0));
        targetCam.set_worldToCameraMatrix(EnviroSky.instance.PlayerCamera.GetStereoViewMatrix((Camera.StereoscopicEye) 0));
        targetCam.Render();
      }
    }
    else
    {
      ((Component) targetCam).get_transform().set_position(((Component) EnviroSky.instance.PlayerCamera).get_transform().get_position());
      ((Component) targetCam).get_transform().set_rotation(((Component) EnviroSky.instance.PlayerCamera).get_transform().get_rotation());
      targetCam.set_worldToCameraMatrix(EnviroSky.instance.PlayerCamera.get_worldToCameraMatrix());
      targetCam.Render();
    }
  }

  private void Update()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    if (this._currentResolution != this.Resolution)
    {
      this._currentResolution = this.Resolution;
      this.ChangeResolution();
    }
    if (this.currentReprojectionPixelSize != EnviroSky.instance.cloudsSettings.reprojectionPixelSize)
    {
      this.currentReprojectionPixelSize = EnviroSky.instance.cloudsSettings.reprojectionPixelSize;
      this.SetReprojectionPixelSize(EnviroSky.instance.cloudsSettings.reprojectionPixelSize);
    }
    if (((Texture) this._volumeLightTexture).get_width() != this.myCam.get_pixelWidth() || ((Texture) this._volumeLightTexture).get_height() != this.myCam.get_pixelHeight())
      this.ChangeResolution();
    if (this.currentSimpleFog == this.simpleFog)
      return;
    this.CreateFogMaterial();
    this.currentSimpleFog = this.simpleFog;
  }

  private void SetCloudProperties()
  {
    this.mat.SetTexture("_Noise", (Texture) this.noiseTextureHigh);
    this.mat.SetTexture("_NoiseLow", (Texture) this.noiseTexture);
    if (EnviroSky.instance.cloudsSettings.detailQuality == EnviroCloudSettings.CloudDetailQuality.Low)
      this.mat.SetTexture("_DetailNoise", (Texture) this.detailNoiseTexture);
    else
      this.mat.SetTexture("_DetailNoise", (Texture) this.detailNoiseTextureHigh);
    Camera.MonoOrStereoscopicEye stereoActiveEye = this.myCam.get_stereoActiveEye();
    if (stereoActiveEye != 2)
    {
      if (stereoActiveEye != null)
      {
        if (stereoActiveEye == 1)
        {
          this.projection = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1);
          this.mat.SetMatrix("_InverseProjection", ((Matrix4x4) ref this.projection).get_inverse());
          Matrix4x4 stereoViewMatrix = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 1);
          this.inverseRotation = ((Matrix4x4) ref stereoViewMatrix).get_inverse();
          this.mat.SetMatrix("_InverseRotation", this.inverseRotation);
        }
      }
      else
      {
        this.projection = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 0);
        this.mat.SetMatrix("_InverseProjection", ((Matrix4x4) ref this.projection).get_inverse());
        Matrix4x4 stereoViewMatrix1 = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 0);
        this.inverseRotation = ((Matrix4x4) ref stereoViewMatrix1).get_inverse();
        this.mat.SetMatrix("_InverseRotation", this.inverseRotation);
        if (EnviroSky.instance.singlePassVR)
        {
          Matrix4x4 projectionMatrix = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1);
          this.mat.SetMatrix("_InverseProjection_SP", ((Matrix4x4) ref projectionMatrix).get_inverse());
          Matrix4x4 stereoViewMatrix2 = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 1);
          this.inverseRotationSPVR = ((Matrix4x4) ref stereoViewMatrix2).get_inverse();
          this.mat.SetMatrix("_InverseRotation_SP", this.inverseRotationSPVR);
        }
      }
    }
    else
    {
      this.projection = this.myCam.get_projectionMatrix();
      this.mat.SetMatrix("_InverseProjection", ((Matrix4x4) ref this.projection).get_inverse());
      this.inverseRotation = this.myCam.get_cameraToWorldMatrix();
      this.mat.SetMatrix("_InverseRotation", this.inverseRotation);
    }
    if (Object.op_Equality((Object) EnviroSky.instance.cloudsSettings.customWeatherMap, (Object) null))
      this.mat.SetTexture("_WeatherMap", (Texture) EnviroSky.instance.weatherMap);
    else
      this.mat.SetTexture("_WeatherMap", (Texture) EnviroSky.instance.cloudsSettings.customWeatherMap);
    this.mat.SetTexture("_CurlNoise", (Texture) this.curlMap);
    this.mat.SetVector("_Steps", new Vector4((float) EnviroSky.instance.cloudsSettings.raymarchSteps * EnviroSky.instance.cloudsConfig.raymarchingScale, (float) EnviroSky.instance.cloudsSettings.raymarchSteps * EnviroSky.instance.cloudsConfig.raymarchingScale, 0.0f, 0.0f));
    this.mat.SetFloat("_BaseNoiseUV", EnviroSky.instance.cloudsSettings.baseNoiseUV);
    this.mat.SetFloat("_DetailNoiseUV", EnviroSky.instance.cloudsSettings.detailNoiseUV);
    this.mat.SetFloat("_PrimAtt", EnviroSky.instance.cloudsSettings.primaryAttenuation);
    this.mat.SetFloat("_SecAtt", EnviroSky.instance.cloudsSettings.secondaryAttenuation);
    this.mat.SetFloat("_SkyBlending", EnviroSky.instance.cloudsConfig.skyBlending);
    this.mat.SetFloat("_HgPhaseFactor", EnviroSky.instance.cloudsSettings.hgPhase);
    this.mat.SetVector("_CloudsParameter", new Vector4(EnviroSky.instance.cloudsSettings.bottomCloudHeight, EnviroSky.instance.cloudsSettings.topCloudHeight, EnviroSky.instance.cloudsSettings.topCloudHeight - EnviroSky.instance.cloudsSettings.bottomCloudHeight, EnviroSky.instance.cloudsSettings.cloudsWorldScale * 10f));
    this.mat.SetFloat("_AmbientLightIntensity", EnviroSky.instance.cloudsSettings.ambientLightIntensity.Evaluate(EnviroSky.instance.GameTime.solarTime));
    this.mat.SetFloat("_SunLightIntensity", EnviroSky.instance.cloudsSettings.directLightIntensity.Evaluate(EnviroSky.instance.GameTime.solarTime));
    this.mat.SetFloat("_AlphaCoef", EnviroSky.instance.cloudsConfig.alphaCoef);
    this.mat.SetFloat("_ExtinctionCoef", EnviroSky.instance.cloudsConfig.scatteringCoef);
    this.mat.SetFloat("_CloudDensityScale", EnviroSky.instance.cloudsConfig.density);
    this.mat.SetColor("_CloudBaseColor", EnviroSky.instance.cloudsConfig.bottomColor);
    this.mat.SetColor("_CloudTopColor", EnviroSky.instance.cloudsConfig.topColor);
    this.mat.SetFloat("_CloudsType", EnviroSky.instance.cloudsConfig.cloudType);
    this.mat.SetFloat("_CloudsCoverage", EnviroSky.instance.cloudsConfig.coverageHeight);
    this.mat.SetVector("_CloudsAnimation", new Vector4((float) EnviroSky.instance.cloudAnim.x, (float) EnviroSky.instance.cloudAnim.y, 0.0f, 0.0f));
    this.mat.SetFloat("_CloudsExposure", EnviroSky.instance.cloudsSettings.cloudsExposure);
    this.mat.SetFloat("_GlobalCoverage", EnviroSky.instance.cloudsConfig.coverage * EnviroSky.instance.cloudsSettings.globalCloudCoverage);
    this.mat.SetColor("_LightColor", EnviroSky.instance.cloudsSettings.volumeCloudsColor.Evaluate(EnviroSky.instance.GameTime.solarTime));
    this.mat.SetColor("_MoonLightColor", EnviroSky.instance.cloudsSettings.volumeCloudsMoonColor.Evaluate(EnviroSky.instance.GameTime.lunarTime));
    this.mat.SetFloat("_Tonemapping", !EnviroSky.instance.cloudsSettings.tonemapping ? 1f : 0.0f);
    this.mat.SetFloat("_stepsInDepth", EnviroSky.instance.cloudsSettings.stepsInDepthModificator);
    this.mat.SetFloat("_LODDistance", EnviroSky.instance.cloudsSettings.lodDistance);
  }

  public void SetBlitmaterialProperties()
  {
    Matrix4x4 inverse1 = ((Matrix4x4) ref this.projection).get_inverse();
    this.blitMat.SetMatrix("_PreviousRotation", this.previousRotation);
    this.blitMat.SetMatrix("_Projection", this.projection);
    this.blitMat.SetMatrix("_InverseRotation", this.inverseRotation);
    this.blitMat.SetMatrix("_InverseProjection", inverse1);
    if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
    {
      Matrix4x4 inverse2 = ((Matrix4x4) ref this.projectionSPVR).get_inverse();
      this.blitMat.SetMatrix("_PreviousRotationSPVR", this.previousRotationSPVR);
      this.blitMat.SetMatrix("_ProjectionSPVR", this.projectionSPVR);
      this.blitMat.SetMatrix("_InverseRotationSPVR", this.inverseRotationSPVR);
      this.blitMat.SetMatrix("_InverseProjectionSPVR", inverse2);
    }
    this.blitMat.SetFloat("_FrameNumber", (float) this.subFrameNumber);
    this.blitMat.SetFloat("_ReprojectionPixelSize", (float) this.reprojectionPixelSize);
    this.blitMat.SetVector("_SubFrameDimension", Vector4.op_Implicit(new Vector2((float) this.subFrameWidth, (float) this.subFrameHeight)));
    this.blitMat.SetVector("_FrameDimension", Vector4.op_Implicit(new Vector2((float) this.frameWidth, (float) this.frameHeight)));
  }

  public void RenderClouds(RenderTexture tex)
  {
    this.SetCloudProperties();
    Graphics.Blit((Texture) null, tex, this.mat);
  }

  private void CreateCloudsRenderTextures(RenderTexture source)
  {
    if (Object.op_Inequality((Object) this.subFrameTex, (Object) null))
    {
      Object.DestroyImmediate((Object) this.subFrameTex);
      this.subFrameTex = (RenderTexture) null;
    }
    if (Object.op_Inequality((Object) this.prevFrameTex, (Object) null))
    {
      Object.DestroyImmediate((Object) this.prevFrameTex);
      this.prevFrameTex = (RenderTexture) null;
    }
    RenderTextureFormat renderTextureFormat = !this.myCam.get_allowHDR() ? (RenderTextureFormat) 7 : (RenderTextureFormat) 9;
    if (Object.op_Equality((Object) this.subFrameTex, (Object) null))
    {
      RenderTextureDescriptor textureDescriptor;
      ((RenderTextureDescriptor) ref textureDescriptor).\u002Ector(this.subFrameWidth, this.subFrameHeight, renderTextureFormat, 0);
      if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
        ((RenderTextureDescriptor) ref textureDescriptor).set_vrUsage((VRTextureUsage) 2);
      this.subFrameTex = new RenderTexture(textureDescriptor);
      ((Texture) this.subFrameTex).set_filterMode((FilterMode) 1);
      ((Object) this.subFrameTex).set_hideFlags((HideFlags) 61);
      this.isFirstFrame = true;
    }
    if (!Object.op_Equality((Object) this.prevFrameTex, (Object) null))
      return;
    RenderTextureDescriptor textureDescriptor1;
    ((RenderTextureDescriptor) ref textureDescriptor1).\u002Ector(this.frameWidth, this.frameHeight, renderTextureFormat, 0);
    if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
      ((RenderTextureDescriptor) ref textureDescriptor1).set_vrUsage((VRTextureUsage) 2);
    this.prevFrameTex = new RenderTexture(textureDescriptor1);
    ((Texture) this.prevFrameTex).set_filterMode((FilterMode) 1);
    ((Object) this.prevFrameTex).set_hideFlags((HideFlags) 61);
    this.isFirstFrame = true;
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if (this.myCam.get_actualRenderingPath() == 1)
      {
        Camera cam = this.myCam;
        cam.set_depthTextureMode((DepthTextureMode) (cam.get_depthTextureMode() | 1));
      }
      int num1 = source.get_depth();
      if (SystemInfo.get_graphicsDeviceType() == 17 || SystemInfo.get_graphicsDeviceType() == 16)
        num1 = 0;
      RenderTextureDescriptor textureDescriptor;
      ((RenderTextureDescriptor) ref textureDescriptor).\u002Ector(((Texture) source).get_width(), ((Texture) source).get_height(), source.get_format(), num1);
      ((RenderTextureDescriptor) ref textureDescriptor).set_msaaSamples(source.get_antiAliasing());
      if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
        ((RenderTextureDescriptor) ref textureDescriptor).set_vrUsage((VRTextureUsage) 2);
      RenderTexture temporary1 = RenderTexture.GetTemporary(textureDescriptor);
      if (EnviroSky.instance.cloudsMode == EnviroSky.EnviroCloudsMode.Volume || EnviroSky.instance.cloudsMode == EnviroSky.EnviroCloudsMode.Both)
      {
        this.StartFrame(((Texture) source).get_width(), ((Texture) source).get_height());
        if (Object.op_Equality((Object) this.subFrameTex, (Object) null) || Object.op_Equality((Object) this.prevFrameTex, (Object) null) || this.textureDimensionChanged)
          this.CreateCloudsRenderTextures(source);
        if (!this.isAddionalCamera)
          EnviroSky.instance.cloudsRenderTarget = this.subFrameTex;
        this.RenderClouds(this.subFrameTex);
        if (this.isFirstFrame)
        {
          Graphics.Blit((Texture) this.subFrameTex, this.prevFrameTex);
          this.isFirstFrame = false;
        }
        this.blitMat.SetTexture("_MainTex", (Texture) source);
        this.blitMat.SetTexture("_SubFrame", (Texture) this.subFrameTex);
        this.blitMat.SetTexture("_PrevFrame", (Texture) this.prevFrameTex);
        this.SetBlitmaterialProperties();
        Graphics.Blit((Texture) source, temporary1, this.blitMat);
        Graphics.Blit((Texture) this.subFrameTex, this.prevFrameTex);
        this.FinalizeFrame();
      }
      else
        Graphics.Blit((Texture) source, temporary1);
      float num2 = (float) ((Component) this.myCam).get_transform().get_position().y - this.height;
      float num3 = (double) num2 > 0.0 ? 0.0f : 1f;
      FogMode fogMode = RenderSettings.get_fogMode();
      float fogDensity = RenderSettings.get_fogDensity();
      float fogStartDistance = RenderSettings.get_fogStartDistance();
      float fogEndDistance = RenderSettings.get_fogEndDistance();
      bool flag = fogMode == 1;
      float num4 = !flag ? 0.0f : fogEndDistance - fogStartDistance;
      float num5 = (double) Mathf.Abs(num4) <= 9.99999974737875E-05 ? 0.0f : 1f / num4;
      Vector4 vector4;
      vector4.x = (__Null) ((double) fogDensity * 1.20112240314484);
      vector4.y = (__Null) ((double) fogDensity * 1.44269502162933);
      vector4.z = !flag ? (__Null) 0.0 : (__Null) -(double) num5;
      vector4.w = !flag ? (__Null) 0.0 : (__Null) ((double) fogEndDistance * (double) num5);
      if (!EnviroSky.instance.fogSettings.useSimpleFog)
      {
        Shader.SetGlobalVector("_FogNoiseVelocity", Vector4.op_Multiply(new Vector4((float) EnviroSky.instance.fogSettings.noiseVelocity.x, (float) EnviroSky.instance.fogSettings.noiseVelocity.y), EnviroSky.instance.fogSettings.noiseScale));
        Shader.SetGlobalVector("_FogNoiseData", new Vector4(EnviroSky.instance.fogSettings.noiseScale, EnviroSky.instance.fogSettings.noiseIntensity, EnviroSky.instance.fogSettings.noiseIntensityOffset));
        Shader.SetGlobalTexture("_FogNoiseTexture", (Texture) this._noiseTexture);
      }
      if (this.volumeLighting)
      {
        if (this.dirVolumeLighting)
        {
          Light component = (Light) ((Component) EnviroSky.instance.Components.DirectLight).GetComponent<Light>();
          int num6 = 4;
          this._material.SetPass(num6);
          if (EnviroSky.instance.volumeLightSettings.directLightNoise)
            this._material.EnableKeyword("NOISE");
          else
            this._material.DisableKeyword("NOISE");
          this._material.SetVector("_LightDir", new Vector4((float) ((Component) component).get_transform().get_forward().x, (float) ((Component) component).get_transform().get_forward().y, (float) ((Component) component).get_transform().get_forward().z, (float) (1.0 / ((double) component.get_range() * (double) component.get_range()))));
          this._material.SetVector("_LightColor", Color.op_Implicit(Color.op_Multiply(component.get_color(), component.get_intensity())));
          this._material.SetFloat("_MaxRayLength", EnviroSky.instance.volumeLightSettings.MaxRayLength);
          if (Object.op_Equality((Object) component.get_cookie(), (Object) null))
          {
            this._material.EnableKeyword("DIRECTIONAL");
            this._material.DisableKeyword("DIRECTIONAL_COOKIE");
          }
          else
          {
            this._material.EnableKeyword("DIRECTIONAL_COOKIE");
            this._material.DisableKeyword("DIRECTIONAL");
            this._material.SetTexture("_LightTexture0", component.get_cookie());
          }
          this._material.SetInt("_SampleCount", EnviroSky.instance.volumeLightSettings.SampleCount);
          this._material.SetVector("_NoiseVelocity", Vector4.op_Multiply(new Vector4((float) EnviroSky.instance.volumeLightSettings.noiseVelocity.x, (float) EnviroSky.instance.volumeLightSettings.noiseVelocity.y), EnviroSky.instance.volumeLightSettings.noiseScale));
          this._material.SetVector("_NoiseData", new Vector4(EnviroSky.instance.volumeLightSettings.noiseScale, EnviroSky.instance.volumeLightSettings.noiseIntensity, EnviroSky.instance.volumeLightSettings.noiseIntensityOffset));
          this._material.SetVector("_MieG", new Vector4((float) (1.0 - (double) EnviroSky.instance.volumeLightSettings.Anistropy * (double) EnviroSky.instance.volumeLightSettings.Anistropy), (float) (1.0 + (double) EnviroSky.instance.volumeLightSettings.Anistropy * (double) EnviroSky.instance.volumeLightSettings.Anistropy), 2f * EnviroSky.instance.volumeLightSettings.Anistropy, 0.07957747f));
          this._material.SetVector("_VolumetricLight", new Vector4(EnviroSky.instance.volumeLightSettings.ScatteringCoef, EnviroSky.instance.volumeLightSettings.ExtinctionCoef, component.get_range(), 1f));
          this._material.SetTexture("_CameraDepthTexture", (Texture) this.GetVolumeLightDepthBuffer());
          if (component.get_shadows() != null)
          {
            this._material.EnableKeyword("SHADOWS_DEPTH");
            Graphics.Blit((Texture) null, this.GetVolumeLightBuffer(), this._material, num6);
          }
          else
          {
            this._material.DisableKeyword("SHADOWS_DEPTH");
            Graphics.Blit((Texture) null, this.GetVolumeLightBuffer(), this._material, num6);
          }
        }
        if (this.Resolution == EnviroSkyRendering.VolumtericResolution.Quarter)
        {
          RenderTexture temporary2 = RenderTexture.GetTemporary(((Texture) this._quarterDepthBuffer).get_width(), ((Texture) this._quarterDepthBuffer).get_height(), 0, (RenderTextureFormat) 2);
          ((Texture) temporary2).set_filterMode((FilterMode) 1);
          if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
            temporary2.set_vrUsage((VRTextureUsage) 2);
          Graphics.Blit((Texture) this._quarterVolumeLightTexture, temporary2, this._bilateralBlurMaterial, 8);
          Graphics.Blit((Texture) temporary2, this._quarterVolumeLightTexture, this._bilateralBlurMaterial, 9);
          Graphics.Blit((Texture) this._quarterVolumeLightTexture, this._volumeLightTexture, this._bilateralBlurMaterial, 7);
          RenderTexture.ReleaseTemporary(temporary2);
        }
        else if (this.Resolution == EnviroSkyRendering.VolumtericResolution.Half)
        {
          RenderTexture temporary2 = RenderTexture.GetTemporary(((Texture) this._halfVolumeLightTexture).get_width(), ((Texture) this._halfVolumeLightTexture).get_height(), 0, (RenderTextureFormat) 2);
          ((Texture) temporary2).set_filterMode((FilterMode) 1);
          if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
            temporary2.set_vrUsage((VRTextureUsage) 2);
          Graphics.Blit((Texture) this._halfVolumeLightTexture, temporary2, this._bilateralBlurMaterial, 2);
          Graphics.Blit((Texture) temporary2, this._halfVolumeLightTexture, this._bilateralBlurMaterial, 3);
          Graphics.Blit((Texture) this._halfVolumeLightTexture, this._volumeLightTexture, this._bilateralBlurMaterial, 5);
          RenderTexture.ReleaseTemporary(temporary2);
        }
        else
        {
          RenderTexture temporary2 = RenderTexture.GetTemporary(((Texture) this._volumeLightTexture).get_width(), ((Texture) this._volumeLightTexture).get_height(), 0, (RenderTextureFormat) 2);
          ((Texture) temporary2).set_filterMode((FilterMode) 1);
          if (this.myCam.get_stereoEnabled() && EnviroSky.instance.singlePassVR)
            temporary2.set_vrUsage((VRTextureUsage) 2);
          Graphics.Blit((Texture) this._volumeLightTexture, temporary2, this._bilateralBlurMaterial, 0);
          Graphics.Blit((Texture) temporary2, this._volumeLightTexture, this._bilateralBlurMaterial, 1);
          RenderTexture.ReleaseTemporary(temporary2);
        }
        this._volumeRenderingMaterial.EnableKeyword("ENVIROVOLUMELIGHT");
      }
      else
        this._volumeRenderingMaterial.DisableKeyword("ENVIROVOLUMELIGHT");
      Shader.SetGlobalFloat("_EnviroVolumeDensity", EnviroSky.instance.globalVolumeLightIntensity);
      Shader.SetGlobalVector("_SceneFogParams", vector4);
      Shader.SetGlobalVector("_SceneFogMode", new Vector4((float) fogMode, !this.useRadialDistance ? 0.0f : 1f, 0.0f, 0.0f));
      Shader.SetGlobalVector("_HeightParams", new Vector4(this.height, num2, num3, this.heightDensity * 0.5f));
      Shader.SetGlobalVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0.0f), 0.0f, 0.0f, 0.0f));
      this._volumeRenderingMaterial.SetTexture("_MainTex", (Texture) temporary1);
      if (this.volumeLighting)
        Shader.SetGlobalTexture("_EnviroVolumeLightingTex", (Texture) this._volumeLightTexture);
      else
        Shader.SetGlobalTexture("_EnviroVolumeLightingTex", (Texture) this.blackTexture);
      Graphics.Blit((Texture) temporary1, destination, this._volumeRenderingMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
    }
  }

  private void UpdateMaterialParameters()
  {
    this._bilateralBlurMaterial.SetTexture("_HalfResDepthBuffer", (Texture) this._halfDepthBuffer);
    this._bilateralBlurMaterial.SetTexture("_HalfResColor", (Texture) this._halfVolumeLightTexture);
    this._bilateralBlurMaterial.SetTexture("_QuarterResDepthBuffer", (Texture) this._quarterDepthBuffer);
    this._bilateralBlurMaterial.SetTexture("_QuarterResColor", (Texture) this._quarterVolumeLightTexture);
    Shader.SetGlobalTexture("_DitherTexture", (Texture) this._ditheringTexture);
    Shader.SetGlobalTexture("_NoiseTexture", (Texture) this._noiseTexture);
  }

  private void LoadNoise3dTexture()
  {
    TextAsset textAsset = Resources.Load("NoiseVolume") as TextAsset;
    byte[] bytes = textAsset.get_bytes();
    uint uint32_1 = BitConverter.ToUInt32(textAsset.get_bytes(), 12);
    uint uint32_2 = BitConverter.ToUInt32(textAsset.get_bytes(), 16);
    uint uint32_3 = BitConverter.ToUInt32(textAsset.get_bytes(), 20);
    uint uint32_4 = BitConverter.ToUInt32(textAsset.get_bytes(), 24);
    uint uint32_5 = BitConverter.ToUInt32(textAsset.get_bytes(), 80);
    uint num1 = BitConverter.ToUInt32(textAsset.get_bytes(), 88);
    if (num1 == 0U)
      num1 = uint32_3 / uint32_2 * 8U;
    this._noiseTexture = new Texture3D((int) uint32_2, (int) uint32_1, (int) uint32_4, (TextureFormat) 4, false);
    ((Object) this._noiseTexture).set_name("3D Noise");
    Color[] colorArray = new Color[(IntPtr) (uint32_2 * uint32_1 * uint32_4)];
    uint num2 = 128;
    if (textAsset.get_bytes()[84] == (byte) 68 && textAsset.get_bytes()[85] == (byte) 88 && (textAsset.get_bytes()[86] == (byte) 49 && textAsset.get_bytes()[87] == (byte) 48) && ((int) uint32_5 & 4) != 0)
    {
      uint uint32_6 = BitConverter.ToUInt32(textAsset.get_bytes(), (int) num2);
      if (uint32_6 >= 60U && uint32_6 <= 65U)
        num1 = 8U;
      else if (uint32_6 >= 48U && uint32_6 <= 52U)
        num1 = 16U;
      else if (uint32_6 >= 27U && uint32_6 <= 32U)
        num1 = 32U;
      num2 += 20U;
    }
    uint num3 = num1 / 8U;
    uint num4 = (uint) ((int) uint32_2 * (int) num1 + 7) / 8U;
    for (int index1 = 0; (long) index1 < (long) uint32_4; ++index1)
    {
      for (int index2 = 0; (long) index2 < (long) uint32_1; ++index2)
      {
        for (int index3 = 0; (long) index3 < (long) uint32_2; ++index3)
        {
          float num5 = (float) bytes[(long) num2 + (long) index3 * (long) num3] / (float) byte.MaxValue;
          colorArray[(long) index3 + (long) index2 * (long) uint32_2 + (long) index1 * (long) uint32_2 * (long) uint32_1] = new Color(num5, num5, num5, num5);
        }
        num2 += num4;
      }
    }
    this._noiseTexture.SetPixels(colorArray);
    this._noiseTexture.Apply();
  }

  private void GenerateDitherTexture()
  {
    if (Object.op_Inequality((Object) this._ditheringTexture, (Object) null))
      return;
    int num1 = 8;
    this._ditheringTexture = new Texture2D(num1, num1, (TextureFormat) 1, false, true);
    ((Texture) this._ditheringTexture).set_filterMode((FilterMode) 0);
    Color32[] color32Array1 = new Color32[num1 * num1];
    int num2 = 0;
    byte num3 = 3;
    Color32[] color32Array2 = color32Array1;
    int index1 = num2;
    int num4 = index1 + 1;
    color32Array2[index1] = new Color32(num3, num3, num3, num3);
    byte num5 = 192;
    Color32[] color32Array3 = color32Array1;
    int index2 = num4;
    int num6 = index2 + 1;
    color32Array3[index2] = new Color32(num5, num5, num5, num5);
    byte num7 = 51;
    Color32[] color32Array4 = color32Array1;
    int index3 = num6;
    int num8 = index3 + 1;
    color32Array4[index3] = new Color32(num7, num7, num7, num7);
    byte num9 = 239;
    Color32[] color32Array5 = color32Array1;
    int index4 = num8;
    int num10 = index4 + 1;
    color32Array5[index4] = new Color32(num9, num9, num9, num9);
    byte num11 = 15;
    Color32[] color32Array6 = color32Array1;
    int index5 = num10;
    int num12 = index5 + 1;
    color32Array6[index5] = new Color32(num11, num11, num11, num11);
    byte num13 = 204;
    Color32[] color32Array7 = color32Array1;
    int index6 = num12;
    int num14 = index6 + 1;
    color32Array7[index6] = new Color32(num13, num13, num13, num13);
    byte num15 = 62;
    Color32[] color32Array8 = color32Array1;
    int index7 = num14;
    int num16 = index7 + 1;
    color32Array8[index7] = new Color32(num15, num15, num15, num15);
    byte num17 = 251;
    Color32[] color32Array9 = color32Array1;
    int index8 = num16;
    int num18 = index8 + 1;
    color32Array9[index8] = new Color32(num17, num17, num17, num17);
    byte num19 = 129;
    Color32[] color32Array10 = color32Array1;
    int index9 = num18;
    int num20 = index9 + 1;
    color32Array10[index9] = new Color32(num19, num19, num19, num19);
    byte num21 = 66;
    Color32[] color32Array11 = color32Array1;
    int index10 = num20;
    int num22 = index10 + 1;
    color32Array11[index10] = new Color32(num21, num21, num21, num21);
    byte num23 = 176;
    Color32[] color32Array12 = color32Array1;
    int index11 = num22;
    int num24 = index11 + 1;
    color32Array12[index11] = new Color32(num23, num23, num23, num23);
    byte num25 = 113;
    Color32[] color32Array13 = color32Array1;
    int index12 = num24;
    int num26 = index12 + 1;
    color32Array13[index12] = new Color32(num25, num25, num25, num25);
    byte num27 = 141;
    Color32[] color32Array14 = color32Array1;
    int index13 = num26;
    int num28 = index13 + 1;
    color32Array14[index13] = new Color32(num27, num27, num27, num27);
    byte num29 = 78;
    Color32[] color32Array15 = color32Array1;
    int index14 = num28;
    int num30 = index14 + 1;
    color32Array15[index14] = new Color32(num29, num29, num29, num29);
    byte num31 = 188;
    Color32[] color32Array16 = color32Array1;
    int index15 = num30;
    int num32 = index15 + 1;
    color32Array16[index15] = new Color32(num31, num31, num31, num31);
    byte num33 = 125;
    Color32[] color32Array17 = color32Array1;
    int index16 = num32;
    int num34 = index16 + 1;
    color32Array17[index16] = new Color32(num33, num33, num33, num33);
    byte num35 = 35;
    Color32[] color32Array18 = color32Array1;
    int index17 = num34;
    int num36 = index17 + 1;
    color32Array18[index17] = new Color32(num35, num35, num35, num35);
    byte num37 = 223;
    Color32[] color32Array19 = color32Array1;
    int index18 = num36;
    int num38 = index18 + 1;
    color32Array19[index18] = new Color32(num37, num37, num37, num37);
    byte num39 = 19;
    Color32[] color32Array20 = color32Array1;
    int index19 = num38;
    int num40 = index19 + 1;
    color32Array20[index19] = new Color32(num39, num39, num39, num39);
    byte num41 = 207;
    Color32[] color32Array21 = color32Array1;
    int index20 = num40;
    int num42 = index20 + 1;
    color32Array21[index20] = new Color32(num41, num41, num41, num41);
    byte num43 = 47;
    Color32[] color32Array22 = color32Array1;
    int index21 = num42;
    int num44 = index21 + 1;
    color32Array22[index21] = new Color32(num43, num43, num43, num43);
    byte num45 = 235;
    Color32[] color32Array23 = color32Array1;
    int index22 = num44;
    int num46 = index22 + 1;
    color32Array23[index22] = new Color32(num45, num45, num45, num45);
    byte num47 = 31;
    Color32[] color32Array24 = color32Array1;
    int index23 = num46;
    int num48 = index23 + 1;
    color32Array24[index23] = new Color32(num47, num47, num47, num47);
    byte num49 = 219;
    Color32[] color32Array25 = color32Array1;
    int index24 = num48;
    int num50 = index24 + 1;
    color32Array25[index24] = new Color32(num49, num49, num49, num49);
    byte num51 = 160;
    Color32[] color32Array26 = color32Array1;
    int index25 = num50;
    int num52 = index25 + 1;
    color32Array26[index25] = new Color32(num51, num51, num51, num51);
    byte num53 = 98;
    Color32[] color32Array27 = color32Array1;
    int index26 = num52;
    int num54 = index26 + 1;
    color32Array27[index26] = new Color32(num53, num53, num53, num53);
    byte num55 = 145;
    Color32[] color32Array28 = color32Array1;
    int index27 = num54;
    int num56 = index27 + 1;
    color32Array28[index27] = new Color32(num55, num55, num55, num55);
    byte num57 = 82;
    Color32[] color32Array29 = color32Array1;
    int index28 = num56;
    int num58 = index28 + 1;
    color32Array29[index28] = new Color32(num57, num57, num57, num57);
    byte num59 = 172;
    Color32[] color32Array30 = color32Array1;
    int index29 = num58;
    int num60 = index29 + 1;
    color32Array30[index29] = new Color32(num59, num59, num59, num59);
    byte num61 = 109;
    Color32[] color32Array31 = color32Array1;
    int index30 = num60;
    int num62 = index30 + 1;
    color32Array31[index30] = new Color32(num61, num61, num61, num61);
    byte num63 = 156;
    Color32[] color32Array32 = color32Array1;
    int index31 = num62;
    int num64 = index31 + 1;
    color32Array32[index31] = new Color32(num63, num63, num63, num63);
    byte num65 = 94;
    Color32[] color32Array33 = color32Array1;
    int index32 = num64;
    int num66 = index32 + 1;
    color32Array33[index32] = new Color32(num65, num65, num65, num65);
    byte num67 = 11;
    Color32[] color32Array34 = color32Array1;
    int index33 = num66;
    int num68 = index33 + 1;
    color32Array34[index33] = new Color32(num67, num67, num67, num67);
    byte num69 = 200;
    Color32[] color32Array35 = color32Array1;
    int index34 = num68;
    int num70 = index34 + 1;
    color32Array35[index34] = new Color32(num69, num69, num69, num69);
    byte num71 = 58;
    Color32[] color32Array36 = color32Array1;
    int index35 = num70;
    int num72 = index35 + 1;
    color32Array36[index35] = new Color32(num71, num71, num71, num71);
    byte num73 = 247;
    Color32[] color32Array37 = color32Array1;
    int index36 = num72;
    int num74 = index36 + 1;
    color32Array37[index36] = new Color32(num73, num73, num73, num73);
    byte num75 = 7;
    Color32[] color32Array38 = color32Array1;
    int index37 = num74;
    int num76 = index37 + 1;
    color32Array38[index37] = new Color32(num75, num75, num75, num75);
    byte num77 = 196;
    Color32[] color32Array39 = color32Array1;
    int index38 = num76;
    int num78 = index38 + 1;
    color32Array39[index38] = new Color32(num77, num77, num77, num77);
    byte num79 = 54;
    Color32[] color32Array40 = color32Array1;
    int index39 = num78;
    int num80 = index39 + 1;
    color32Array40[index39] = new Color32(num79, num79, num79, num79);
    byte num81 = 243;
    Color32[] color32Array41 = color32Array1;
    int index40 = num80;
    int num82 = index40 + 1;
    color32Array41[index40] = new Color32(num81, num81, num81, num81);
    byte num83 = 137;
    Color32[] color32Array42 = color32Array1;
    int index41 = num82;
    int num84 = index41 + 1;
    color32Array42[index41] = new Color32(num83, num83, num83, num83);
    byte num85 = 74;
    Color32[] color32Array43 = color32Array1;
    int index42 = num84;
    int num86 = index42 + 1;
    color32Array43[index42] = new Color32(num85, num85, num85, num85);
    byte num87 = 184;
    Color32[] color32Array44 = color32Array1;
    int index43 = num86;
    int num88 = index43 + 1;
    color32Array44[index43] = new Color32(num87, num87, num87, num87);
    byte num89 = 121;
    Color32[] color32Array45 = color32Array1;
    int index44 = num88;
    int num90 = index44 + 1;
    color32Array45[index44] = new Color32(num89, num89, num89, num89);
    byte num91 = 133;
    Color32[] color32Array46 = color32Array1;
    int index45 = num90;
    int num92 = index45 + 1;
    color32Array46[index45] = new Color32(num91, num91, num91, num91);
    byte num93 = 70;
    Color32[] color32Array47 = color32Array1;
    int index46 = num92;
    int num94 = index46 + 1;
    color32Array47[index46] = new Color32(num93, num93, num93, num93);
    byte num95 = 180;
    Color32[] color32Array48 = color32Array1;
    int index47 = num94;
    int num96 = index47 + 1;
    color32Array48[index47] = new Color32(num95, num95, num95, num95);
    byte num97 = 117;
    Color32[] color32Array49 = color32Array1;
    int index48 = num96;
    int num98 = index48 + 1;
    color32Array49[index48] = new Color32(num97, num97, num97, num97);
    byte num99 = 43;
    Color32[] color32Array50 = color32Array1;
    int index49 = num98;
    int num100 = index49 + 1;
    color32Array50[index49] = new Color32(num99, num99, num99, num99);
    byte num101 = 231;
    Color32[] color32Array51 = color32Array1;
    int index50 = num100;
    int num102 = index50 + 1;
    color32Array51[index50] = new Color32(num101, num101, num101, num101);
    byte num103 = 27;
    Color32[] color32Array52 = color32Array1;
    int index51 = num102;
    int num104 = index51 + 1;
    color32Array52[index51] = new Color32(num103, num103, num103, num103);
    byte num105 = 215;
    Color32[] color32Array53 = color32Array1;
    int index52 = num104;
    int num106 = index52 + 1;
    color32Array53[index52] = new Color32(num105, num105, num105, num105);
    byte num107 = 39;
    Color32[] color32Array54 = color32Array1;
    int index53 = num106;
    int num108 = index53 + 1;
    color32Array54[index53] = new Color32(num107, num107, num107, num107);
    byte num109 = 227;
    Color32[] color32Array55 = color32Array1;
    int index54 = num108;
    int num110 = index54 + 1;
    color32Array55[index54] = new Color32(num109, num109, num109, num109);
    byte num111 = 23;
    Color32[] color32Array56 = color32Array1;
    int index55 = num110;
    int num112 = index55 + 1;
    color32Array56[index55] = new Color32(num111, num111, num111, num111);
    byte num113 = 211;
    Color32[] color32Array57 = color32Array1;
    int index56 = num112;
    int num114 = index56 + 1;
    color32Array57[index56] = new Color32(num113, num113, num113, num113);
    byte num115 = 168;
    Color32[] color32Array58 = color32Array1;
    int index57 = num114;
    int num116 = index57 + 1;
    color32Array58[index57] = new Color32(num115, num115, num115, num115);
    byte num117 = 105;
    Color32[] color32Array59 = color32Array1;
    int index58 = num116;
    int num118 = index58 + 1;
    color32Array59[index58] = new Color32(num117, num117, num117, num117);
    byte num119 = 153;
    Color32[] color32Array60 = color32Array1;
    int index59 = num118;
    int num120 = index59 + 1;
    color32Array60[index59] = new Color32(num119, num119, num119, num119);
    byte num121 = 90;
    Color32[] color32Array61 = color32Array1;
    int index60 = num120;
    int num122 = index60 + 1;
    color32Array61[index60] = new Color32(num121, num121, num121, num121);
    byte num123 = 164;
    Color32[] color32Array62 = color32Array1;
    int index61 = num122;
    int num124 = index61 + 1;
    color32Array62[index61] = new Color32(num123, num123, num123, num123);
    byte num125 = 102;
    Color32[] color32Array63 = color32Array1;
    int index62 = num124;
    int num126 = index62 + 1;
    color32Array63[index62] = new Color32(num125, num125, num125, num125);
    byte num127 = 149;
    Color32[] color32Array64 = color32Array1;
    int index63 = num126;
    int num128 = index63 + 1;
    color32Array64[index63] = new Color32(num127, num127, num127, num127);
    byte num129 = 86;
    Color32[] color32Array65 = color32Array1;
    int index64 = num128;
    int num130 = index64 + 1;
    color32Array65[index64] = new Color32(num129, num129, num129, num129);
    this._ditheringTexture.SetPixels32(color32Array1);
    this._ditheringTexture.Apply();
  }

  private Mesh CreateSpotLightMesh()
  {
    Mesh mesh = new Mesh();
    Vector3[] vector3Array = new Vector3[50];
    Color32[] color32Array = new Color32[50];
    vector3Array[0] = new Vector3(0.0f, 0.0f, 0.0f);
    vector3Array[1] = new Vector3(0.0f, 0.0f, 1f);
    float num1 = 0.0f;
    float num2 = 0.3926991f;
    float num3 = 0.9f;
    for (int index = 0; index < 16; ++index)
    {
      vector3Array[index + 2] = new Vector3(-Mathf.Cos(num1) * num3, Mathf.Sin(num1) * num3, num3);
      color32Array[index + 2] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      vector3Array[index + 2 + 16] = new Vector3(-Mathf.Cos(num1), Mathf.Sin(num1), 1f);
      color32Array[index + 2 + 16] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0);
      vector3Array[index + 2 + 32] = new Vector3(-Mathf.Cos(num1) * num3, Mathf.Sin(num1) * num3, 1f);
      color32Array[index + 2 + 32] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      num1 += num2;
    }
    mesh.set_vertices(vector3Array);
    mesh.set_colors32(color32Array);
    int[] numArray1 = new int[288];
    int num4 = 0;
    for (int index1 = 2; index1 < 17; ++index1)
    {
      int[] numArray2 = numArray1;
      int index2 = num4;
      int num5 = index2 + 1;
      numArray2[index2] = 0;
      int[] numArray3 = numArray1;
      int index3 = num5;
      int num6 = index3 + 1;
      int num7 = index1;
      numArray3[index3] = num7;
      int[] numArray4 = numArray1;
      int index4 = num6;
      num4 = index4 + 1;
      int num8 = index1 + 1;
      numArray4[index4] = num8;
    }
    int[] numArray5 = numArray1;
    int index5 = num4;
    int num9 = index5 + 1;
    numArray5[index5] = 0;
    int[] numArray6 = numArray1;
    int index6 = num9;
    int num10 = index6 + 1;
    numArray6[index6] = 17;
    int[] numArray7 = numArray1;
    int index7 = num10;
    int num11 = index7 + 1;
    numArray7[index7] = 2;
    for (int index1 = 2; index1 < 17; ++index1)
    {
      int[] numArray2 = numArray1;
      int index2 = num11;
      int num5 = index2 + 1;
      int num6 = index1;
      numArray2[index2] = num6;
      int[] numArray3 = numArray1;
      int index3 = num5;
      int num7 = index3 + 1;
      int num8 = index1 + 16;
      numArray3[index3] = num8;
      int[] numArray4 = numArray1;
      int index4 = num7;
      int num12 = index4 + 1;
      int num13 = index1 + 1;
      numArray4[index4] = num13;
      int[] numArray8 = numArray1;
      int index8 = num12;
      int num14 = index8 + 1;
      int num15 = index1 + 1;
      numArray8[index8] = num15;
      int[] numArray9 = numArray1;
      int index9 = num14;
      int num16 = index9 + 1;
      int num17 = index1 + 16;
      numArray9[index9] = num17;
      int[] numArray10 = numArray1;
      int index10 = num16;
      num11 = index10 + 1;
      int num18 = index1 + 16 + 1;
      numArray10[index10] = num18;
    }
    int[] numArray11 = numArray1;
    int index11 = num11;
    int num19 = index11 + 1;
    numArray11[index11] = 2;
    int[] numArray12 = numArray1;
    int index12 = num19;
    int num20 = index12 + 1;
    numArray12[index12] = 17;
    int[] numArray13 = numArray1;
    int index13 = num20;
    int num21 = index13 + 1;
    numArray13[index13] = 18;
    int[] numArray14 = numArray1;
    int index14 = num21;
    int num22 = index14 + 1;
    numArray14[index14] = 18;
    int[] numArray15 = numArray1;
    int index15 = num22;
    int num23 = index15 + 1;
    numArray15[index15] = 17;
    int[] numArray16 = numArray1;
    int index16 = num23;
    int num24 = index16 + 1;
    numArray16[index16] = 33;
    for (int index1 = 18; index1 < 33; ++index1)
    {
      int[] numArray2 = numArray1;
      int index2 = num24;
      int num5 = index2 + 1;
      int num6 = index1;
      numArray2[index2] = num6;
      int[] numArray3 = numArray1;
      int index3 = num5;
      int num7 = index3 + 1;
      int num8 = index1 + 16;
      numArray3[index3] = num8;
      int[] numArray4 = numArray1;
      int index4 = num7;
      int num12 = index4 + 1;
      int num13 = index1 + 1;
      numArray4[index4] = num13;
      int[] numArray8 = numArray1;
      int index8 = num12;
      int num14 = index8 + 1;
      int num15 = index1 + 1;
      numArray8[index8] = num15;
      int[] numArray9 = numArray1;
      int index9 = num14;
      int num16 = index9 + 1;
      int num17 = index1 + 16;
      numArray9[index9] = num17;
      int[] numArray10 = numArray1;
      int index10 = num16;
      num24 = index10 + 1;
      int num18 = index1 + 16 + 1;
      numArray10[index10] = num18;
    }
    int[] numArray17 = numArray1;
    int index17 = num24;
    int num25 = index17 + 1;
    numArray17[index17] = 18;
    int[] numArray18 = numArray1;
    int index18 = num25;
    int num26 = index18 + 1;
    numArray18[index18] = 33;
    int[] numArray19 = numArray1;
    int index19 = num26;
    int num27 = index19 + 1;
    numArray19[index19] = 34;
    int[] numArray20 = numArray1;
    int index20 = num27;
    int num28 = index20 + 1;
    numArray20[index20] = 34;
    int[] numArray21 = numArray1;
    int index21 = num28;
    int num29 = index21 + 1;
    numArray21[index21] = 33;
    int[] numArray22 = numArray1;
    int index22 = num29;
    int num30 = index22 + 1;
    numArray22[index22] = 49;
    for (int index1 = 34; index1 < 49; ++index1)
    {
      int[] numArray2 = numArray1;
      int index2 = num30;
      int num5 = index2 + 1;
      numArray2[index2] = 1;
      int[] numArray3 = numArray1;
      int index3 = num5;
      int num6 = index3 + 1;
      int num7 = index1 + 1;
      numArray3[index3] = num7;
      int[] numArray4 = numArray1;
      int index4 = num6;
      num30 = index4 + 1;
      int num8 = index1;
      numArray4[index4] = num8;
    }
    int[] numArray23 = numArray1;
    int index23 = num30;
    int num31 = index23 + 1;
    numArray23[index23] = 1;
    int[] numArray24 = numArray1;
    int index24 = num31;
    int num32 = index24 + 1;
    numArray24[index24] = 34;
    int[] numArray25 = numArray1;
    int index25 = num32;
    int num33 = index25 + 1;
    numArray25[index25] = 49;
    mesh.set_triangles(numArray1);
    mesh.RecalculateBounds();
    return mesh;
  }

  public void SetReprojectionPixelSize(EnviroCloudSettings.ReprojectionPixelSize pSize)
  {
    switch (pSize)
    {
      case EnviroCloudSettings.ReprojectionPixelSize.Off:
        this.reprojectionPixelSize = 1;
        break;
      case EnviroCloudSettings.ReprojectionPixelSize.Low:
        this.reprojectionPixelSize = 2;
        break;
      case EnviroCloudSettings.ReprojectionPixelSize.Medium:
        this.reprojectionPixelSize = 4;
        break;
      case EnviroCloudSettings.ReprojectionPixelSize.High:
        this.reprojectionPixelSize = 8;
        break;
    }
    this.frameList = this.CalculateFrames(this.reprojectionPixelSize);
  }

  public void StartFrame(int width, int height)
  {
    this.textureDimensionChanged = this.UpdateFrameDimensions(width, height);
    Camera.MonoOrStereoscopicEye stereoActiveEye = this.myCam.get_stereoActiveEye();
    if (stereoActiveEye != 2)
    {
      if (stereoActiveEye != null)
      {
        if (stereoActiveEye != 1)
          return;
        this.projection = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1);
        this.rotation = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 1);
        this.inverseRotation = ((Matrix4x4) ref this.rotation).get_inverse();
      }
      else
      {
        this.projection = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 0);
        this.rotation = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 0);
        this.inverseRotation = ((Matrix4x4) ref this.rotation).get_inverse();
        if (!EnviroSky.instance.singlePassVR)
          return;
        this.projectionSPVR = this.myCam.GetStereoProjectionMatrix((Camera.StereoscopicEye) 1);
        this.rotationSPVR = this.myCam.GetStereoViewMatrix((Camera.StereoscopicEye) 1);
        this.inverseRotationSPVR = ((Matrix4x4) ref this.rotationSPVR).get_inverse();
      }
    }
    else
    {
      this.projection = this.myCam.get_projectionMatrix();
      this.rotation = this.myCam.get_worldToCameraMatrix();
      this.inverseRotation = this.myCam.get_cameraToWorldMatrix();
    }
  }

  public void FinalizeFrame()
  {
    ++this.renderingCounter;
    this.previousRotation = this.rotation;
    if (EnviroSky.instance.singlePassVR)
      this.previousRotationSPVR = this.rotationSPVR;
    this.subFrameNumber = this.frameList[this.renderingCounter % (this.reprojectionPixelSize * this.reprojectionPixelSize)];
  }

  private bool UpdateFrameDimensions(int width, int height)
  {
    int num1 = width / EnviroSky.instance.cloudsSettings.cloudsRenderResolution;
    int num2 = height / EnviroSky.instance.cloudsSettings.cloudsRenderResolution;
    while (num1 % this.reprojectionPixelSize != 0)
      ++num1;
    while (num2 % this.reprojectionPixelSize != 0)
      ++num2;
    int num3 = num1 / this.reprojectionPixelSize;
    int num4 = num2 / this.reprojectionPixelSize;
    if (num1 != this.frameWidth || num3 != this.subFrameWidth || (num2 != this.frameHeight || num4 != this.subFrameHeight))
    {
      this.frameWidth = num1;
      this.frameHeight = num2;
      this.subFrameWidth = num3;
      this.subFrameHeight = num4;
      return true;
    }
    this.frameWidth = num1;
    this.frameHeight = num2;
    this.subFrameWidth = num3;
    this.subFrameHeight = num4;
    return false;
  }

  private int[] CalculateFrames(int reproSize)
  {
    this.subFrameNumber = 0;
    int length = reproSize * reproSize;
    int[] numArray = new int[length];
    int index1;
    for (index1 = 0; index1 < length; ++index1)
      numArray[index1] = index1;
    while (index1-- > 0)
    {
      int num = numArray[index1];
      int index2 = (int) ((double) Random.Range(0, 1) * 1000.0) % length;
      numArray[index1] = numArray[index2];
      numArray[index2] = num;
    }
    return numArray;
  }

  public enum VolumtericResolution
  {
    Full,
    Half,
    Quarter,
  }
}
