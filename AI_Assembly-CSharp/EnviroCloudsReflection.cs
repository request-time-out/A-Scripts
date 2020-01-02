// Decompiled with JetBrains decompiler
// Type: EnviroCloudsReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroCloudsReflection : MonoBehaviour
{
  public bool resetCameraProjection;
  public bool tonemapping;
  private Camera myCam;
  private Material mat;
  private Material blitMat;
  private Material weatherMapMat;
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

  public EnviroCloudsReflection()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.myCam = (Camera) ((Component) this).GetComponent<Camera>();
    this.CreateMaterialsAndTextures();
    if (!Object.op_Inequality((Object) EnviroSky.instance, (Object) null))
      return;
    this.SetReprojectionPixelSize(EnviroSky.instance.cloudsSettings.reprojectionPixelSize);
  }

  private void CreateMaterialsAndTextures()
  {
    if (Object.op_Equality((Object) this.mat, (Object) null))
      this.mat = new Material(Shader.Find("Enviro/RaymarchClouds"));
    if (Object.op_Equality((Object) this.blitMat, (Object) null))
      this.blitMat = new Material(Shader.Find("Enviro/Blit"));
    if (Object.op_Equality((Object) this.curlMap, (Object) null))
      this.curlMap = Resources.Load("tex_enviro_curl") as Texture2D;
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
    this.mat.SetFloat("_Tonemapping", !this.tonemapping ? 1f : 0.0f);
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
    if (EnviroSky.instance.singlePassVR)
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
      if (EnviroSky.instance.singlePassVR)
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
    if (EnviroSky.instance.singlePassVR)
      ((RenderTextureDescriptor) ref textureDescriptor1).set_vrUsage((VRTextureUsage) 2);
    this.prevFrameTex = new RenderTexture(textureDescriptor1);
    ((Texture) this.prevFrameTex).set_filterMode((FilterMode) 1);
    ((Object) this.prevFrameTex).set_hideFlags((HideFlags) 61);
    this.isFirstFrame = true;
  }

  private void Update()
  {
    if (this.currentReprojectionPixelSize != EnviroSky.instance.cloudsSettings.reprojectionPixelSize)
    {
      this.currentReprojectionPixelSize = EnviroSky.instance.cloudsSettings.reprojectionPixelSize;
      this.SetReprojectionPixelSize(EnviroSky.instance.cloudsSettings.reprojectionPixelSize);
    }
    this.mat.SetTexture("_WeatherMap", (Texture) EnviroSky.instance.weatherMap);
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      Graphics.Blit((Texture) source, destination);
    else if (EnviroSky.instance.cloudsMode == EnviroSky.EnviroCloudsMode.Volume || EnviroSky.instance.cloudsMode == EnviroSky.EnviroCloudsMode.Both)
    {
      this.StartFrame();
      if (Object.op_Equality((Object) this.subFrameTex, (Object) null) || Object.op_Equality((Object) this.prevFrameTex, (Object) null) || this.textureDimensionChanged)
        this.CreateCloudsRenderTextures(source);
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
      Graphics.Blit((Texture) source, destination, this.blitMat);
      Graphics.Blit((Texture) this.subFrameTex, this.prevFrameTex);
      this.FinalizeFrame();
    }
    else
      Graphics.Blit((Texture) source, destination);
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

  public void StartFrame()
  {
    this.textureDimensionChanged = this.UpdateFrameDimensions();
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
      if (this.resetCameraProjection)
        this.myCam.ResetProjectionMatrix();
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

  private bool UpdateFrameDimensions()
  {
    int num1 = this.myCam.get_pixelWidth() / EnviroSky.instance.cloudsSettings.cloudsRenderResolution;
    int num2 = this.myCam.get_pixelHeight() / EnviroSky.instance.cloudsSettings.cloudsRenderResolution;
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
}
