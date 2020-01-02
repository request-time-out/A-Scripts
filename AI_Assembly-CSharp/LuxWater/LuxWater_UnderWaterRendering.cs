// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_UnderWaterRendering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LuxWater
{
  [RequireComponent(typeof (Camera))]
  public class LuxWater_UnderWaterRendering : MonoBehaviour
  {
    public static LuxWater_UnderWaterRendering instance;
    [Space(6f)]
    [LuxWater_HelpBtn("h.d0q6uguuxpy")]
    public Transform Sun;
    [Space(4f)]
    public bool FindSunOnEnable;
    public string SunGoName;
    public string SunTagName;
    private Light SunLight;
    [Space(2f)]
    [Header("Deep Water Lighting")]
    [Space(4f)]
    public bool EnableDeepwaterLighting;
    public float DefaultWaterSurfacePosition;
    public float DirectionalLightingFadeRange;
    public float FogLightingFadeRange;
    [Space(2f)]
    [Header("Advanced Deferred Fog")]
    [Space(4f)]
    public bool EnableAdvancedDeferredFog;
    public float FogDepthShift;
    public float FogEdgeBlending;
    [Space(8f)]
    [NonSerialized]
    public int activeWaterVolume;
    [NonSerialized]
    public List<Camera> activeWaterVolumeCameras;
    [NonSerialized]
    public float activeGridSize;
    [NonSerialized]
    public float WaterSurfacePos;
    [Space(8f)]
    [NonSerialized]
    public List<int> RegisteredWaterVolumesIDs;
    [NonSerialized]
    public List<LuxWater_WaterVolume> RegisteredWaterVolumes;
    private List<Mesh> WaterMeshes;
    private List<Transform> WaterTransforms;
    private List<Material> WaterMaterials;
    private List<bool> WaterIsOnScreen;
    private List<bool> WaterUsesSlidingVolume;
    private RenderTexture UnderWaterMask;
    [Space(2f)]
    [Header("Managed transparent Materials")]
    [Space(4f)]
    public List<Material> m_aboveWatersurface;
    public List<Material> m_belowWatersurface;
    [Space(2f)]
    [Header("Optimize")]
    [Space(4f)]
    public ShaderVariantCollection PrewarmedShaders;
    public int ListCapacity;
    [Space(2f)]
    [Header("Debug")]
    [Space(4f)]
    public bool enableDebug;
    [Space(8f)]
    private Material mat;
    private Material blurMaterial;
    private Material blitMaterial;
    private Camera cam;
    private bool UnderwaterIsSetUp;
    private Transform camTransform;
    private Matrix4x4 frustumCornersArray;
    private SphericalHarmonicsL2 ambientProbe;
    private Vector3[] directions;
    private Color[] AmbientLightingSamples;
    private bool DoUnderWaterRendering;
    private Matrix4x4 camProj;
    private Vector3[] frustumCorners;
    private float Projection;
    private bool islinear;
    private Matrix4x4 WatervolumeMatrix;
    private int UnderWaterMaskPID;
    private int Lux_FrustumCornersWSPID;
    private int Lux_CameraWSPID;
    private int GerstnerEnabledPID;
    private int LuxWaterMask_GerstnerVertexIntensityPID;
    private int GerstnerVertexIntensityPID;
    private int LuxWaterMask_GAmplitudePID;
    private int GAmplitudePID;
    private int LuxWaterMask_GFinalFrequencyPID;
    private int GFinalFrequencyPID;
    private int LuxWaterMask_GSteepnessPID;
    private int GSteepnessPID;
    private int LuxWaterMask_GFinalSpeedPID;
    private int GFinalSpeedPID;
    private int LuxWaterMask_GDirectionABPID;
    private int GDirectionABPID;
    private int LuxWaterMask_GDirectionCDPID;
    private int GDirectionCDPID;
    private int LuxWaterMask_GerstnerSecondaryWaves;
    private int GerstnerSecondaryWaves;
    private int Lux_UnderWaterAmbientSkyLightPID;
    private int Lux_UnderWaterSunColorPID;
    private int Lux_UnderWaterSunDirPID;
    private int Lux_UnderWaterSunDirViewSpacePID;
    private int Lux_EdgeLengthPID;
    private int Lux_CausticsEnabledPID;
    private int Lux_CausticModePID;
    private int Lux_UnderWaterFogColorPID;
    private int Lux_UnderWaterFogDensityPID;
    private int Lux_UnderWaterFogAbsorptionCancellationPID;
    private int Lux_UnderWaterAbsorptionHeightPID;
    private int Lux_UnderWaterAbsorptionMaxHeightPID;
    private int Lux_MaxDirLightDepthPID;
    private int Lux_MaxFogLightDepthPID;
    private int Lux_UnderWaterAbsorptionDepthPID;
    private int Lux_UnderWaterAbsorptionColorStrengthPID;
    private int Lux_UnderWaterAbsorptionStrengthPID;
    private int Lux_UnderWaterUnderwaterScatteringPowerPID;
    private int Lux_UnderWaterUnderwaterScatteringIntensityPID;
    private int Lux_UnderWaterUnderwaterScatteringColorPID;
    private int Lux_UnderWaterUnderwaterScatteringOcclusionPID;
    private int Lux_UnderWaterCausticsPID;
    private int Lux_UnderWaterDeferredFogParams;
    private int CausticTexPID;

    public LuxWater_UnderWaterRendering()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      if (Object.op_Inequality((Object) LuxWater_UnderWaterRendering.instance, (Object) null))
        Object.Destroy((Object) this);
      else
        LuxWater_UnderWaterRendering.instance = this;
      this.mat = new Material(Shader.Find("Hidden/Lux Water/WaterMask"));
      this.blurMaterial = new Material(Shader.Find("Hidden/Lux Water/BlurEffectConeTap"));
      this.blitMaterial = new Material(Shader.Find("Hidden/Lux Water/UnderWaterPost"));
      if (Object.op_Equality((Object) this.cam, (Object) null))
        this.cam = (Camera) ((Component) this).GetComponent<Camera>();
      Camera cam = this.cam;
      cam.set_depthTextureMode((DepthTextureMode) (cam.get_depthTextureMode() | 1));
      this.camTransform = ((Component) this.cam).get_transform();
      if (this.FindSunOnEnable)
      {
        if (this.SunGoName != string.Empty)
          this.Sun = GameObject.Find(this.SunGoName).get_transform();
        else if (this.SunTagName != string.Empty)
          this.Sun = GameObject.FindWithTag(this.SunTagName).get_transform();
      }
      this.Projection = !SystemInfo.get_usesReversedZBuffer() ? 1f : -1f;
      this.UnderWaterMaskPID = Shader.PropertyToID("_UnderWaterMask");
      this.Lux_FrustumCornersWSPID = Shader.PropertyToID("_Lux_FrustumCornersWS");
      this.Lux_CameraWSPID = Shader.PropertyToID("_Lux_CameraWS");
      this.GerstnerEnabledPID = Shader.PropertyToID("_GerstnerEnabled");
      this.LuxWaterMask_GerstnerVertexIntensityPID = Shader.PropertyToID("_LuxWaterMask_GerstnerVertexIntensity");
      this.GerstnerVertexIntensityPID = Shader.PropertyToID("_GerstnerVertexIntensity");
      this.LuxWaterMask_GAmplitudePID = Shader.PropertyToID("_LuxWaterMask_GAmplitude");
      this.GAmplitudePID = Shader.PropertyToID("_GAmplitude");
      this.LuxWaterMask_GFinalFrequencyPID = Shader.PropertyToID("_LuxWaterMask_GFinalFrequency");
      this.GFinalFrequencyPID = Shader.PropertyToID("_GFinalFrequency");
      this.LuxWaterMask_GSteepnessPID = Shader.PropertyToID("_LuxWaterMask_GSteepness");
      this.GSteepnessPID = Shader.PropertyToID("_GSteepness");
      this.LuxWaterMask_GFinalSpeedPID = Shader.PropertyToID("_LuxWaterMask_GFinalSpeed");
      this.GFinalSpeedPID = Shader.PropertyToID("_GFinalSpeed");
      this.LuxWaterMask_GDirectionABPID = Shader.PropertyToID("_LuxWaterMask_GDirectionAB");
      this.GDirectionABPID = Shader.PropertyToID("_GDirectionAB");
      this.LuxWaterMask_GDirectionCDPID = Shader.PropertyToID("_LuxWaterMask_GDirectionCD");
      this.GDirectionCDPID = Shader.PropertyToID("_GDirectionCD");
      this.LuxWaterMask_GerstnerSecondaryWaves = Shader.PropertyToID("_LuxWaterMask_GerstnerSecondaryWaves");
      this.GerstnerSecondaryWaves = Shader.PropertyToID("_GerstnerSecondaryWaves");
      this.Lux_UnderWaterAmbientSkyLightPID = Shader.PropertyToID("_Lux_UnderWaterAmbientSkyLight");
      this.Lux_UnderWaterSunColorPID = Shader.PropertyToID("_Lux_UnderWaterSunColor");
      this.Lux_UnderWaterSunDirPID = Shader.PropertyToID("_Lux_UnderWaterSunDir");
      this.Lux_UnderWaterSunDirViewSpacePID = Shader.PropertyToID("_Lux_UnderWaterSunDirViewSpace");
      this.Lux_EdgeLengthPID = Shader.PropertyToID("_LuxWater_EdgeLength");
      this.Lux_MaxDirLightDepthPID = Shader.PropertyToID("_MaxDirLightDepth");
      this.Lux_MaxFogLightDepthPID = Shader.PropertyToID("_MaxFogLightDepth");
      this.Lux_CausticsEnabledPID = Shader.PropertyToID("_CausticsEnabled");
      this.Lux_CausticModePID = Shader.PropertyToID("_CausticMode");
      this.Lux_UnderWaterFogColorPID = Shader.PropertyToID("_Lux_UnderWaterFogColor");
      this.Lux_UnderWaterFogDensityPID = Shader.PropertyToID("_Lux_UnderWaterFogDensity");
      this.Lux_UnderWaterFogAbsorptionCancellationPID = Shader.PropertyToID("_Lux_UnderWaterFogAbsorptionCancellation");
      this.Lux_UnderWaterAbsorptionHeightPID = Shader.PropertyToID("_Lux_UnderWaterAbsorptionHeight");
      this.Lux_UnderWaterAbsorptionMaxHeightPID = Shader.PropertyToID("_Lux_UnderWaterAbsorptionMaxHeight");
      this.Lux_UnderWaterAbsorptionDepthPID = Shader.PropertyToID("_Lux_UnderWaterAbsorptionDepth");
      this.Lux_UnderWaterAbsorptionColorStrengthPID = Shader.PropertyToID("_Lux_UnderWaterAbsorptionColorStrength");
      this.Lux_UnderWaterAbsorptionStrengthPID = Shader.PropertyToID("_Lux_UnderWaterAbsorptionStrength");
      this.Lux_UnderWaterUnderwaterScatteringPowerPID = Shader.PropertyToID("_Lux_UnderWaterUnderwaterScatteringPower");
      this.Lux_UnderWaterUnderwaterScatteringIntensityPID = Shader.PropertyToID("_Lux_UnderWaterUnderwaterScatteringIntensity");
      this.Lux_UnderWaterUnderwaterScatteringColorPID = Shader.PropertyToID("_Lux_UnderWaterUnderwaterScatteringColor");
      this.Lux_UnderWaterUnderwaterScatteringOcclusionPID = Shader.PropertyToID("_Lux_UnderwaterScatteringOcclusion");
      this.Lux_UnderWaterCausticsPID = Shader.PropertyToID("_Lux_UnderWaterCaustics");
      this.Lux_UnderWaterDeferredFogParams = Shader.PropertyToID("_LuxUnderWaterDeferredFogParams");
      this.CausticTexPID = Shader.PropertyToID("_CausticTex");
      this.islinear = QualitySettings.get_desiredColorSpace() == 1;
      if (Object.op_Inequality((Object) this.PrewarmedShaders, (Object) null) && !this.PrewarmedShaders.get_isWarmedUp())
        this.PrewarmedShaders.WarmUp();
      if (Object.op_Inequality((Object) this.Sun, (Object) null))
        this.SunLight = (Light) ((Component) this.Sun).GetComponent<Light>();
      this.RegisteredWaterVolumesIDs.Capacity = this.ListCapacity;
      this.RegisteredWaterVolumes.Capacity = this.ListCapacity;
      this.WaterMeshes.Capacity = this.ListCapacity;
      this.WaterTransforms.Capacity = this.ListCapacity;
      this.WaterMaterials.Capacity = this.ListCapacity;
      this.WaterIsOnScreen.Capacity = this.ListCapacity;
      this.WaterUsesSlidingVolume.Capacity = this.ListCapacity;
      this.activeWaterVolumeCameras.Capacity = 2;
      this.SetDeepwaterLighting();
      this.SetDeferredFogParams();
    }

    private void CleanUp()
    {
      LuxWater_UnderWaterRendering.instance = (LuxWater_UnderWaterRendering) null;
      if (Object.op_Inequality((Object) this.UnderWaterMask, (Object) null))
      {
        this.UnderWaterMask.Release();
        Object.Destroy((Object) this.UnderWaterMask);
      }
      if (Object.op_Implicit((Object) this.mat))
        Object.Destroy((Object) this.mat);
      if (Object.op_Implicit((Object) this.blurMaterial))
        Object.Destroy((Object) this.blurMaterial);
      if (Object.op_Implicit((Object) this.blitMaterial))
        Object.Destroy((Object) this.blitMaterial);
      Shader.DisableKeyword("LUXWATER_DEEPWATERLIGHTING");
      Shader.DisableKeyword("LUXWATER_DEFERREDFOG");
      this.RegisteredWaterVolumesIDs.Clear();
      this.RegisteredWaterVolumes.Clear();
      this.WaterMeshes.Clear();
      this.WaterTransforms.Clear();
      this.WaterMaterials.Clear();
      this.WaterIsOnScreen.Clear();
      this.WaterUsesSlidingVolume.Clear();
      this.activeWaterVolumeCameras.Clear();
    }

    private void OnDisable()
    {
      this.CleanUp();
    }

    private void OnDestroy()
    {
      this.CleanUp();
    }

    private void OnValidate()
    {
      this.SetDeepwaterLighting();
      this.SetDeferredFogParams();
    }

    public void SetDeferredFogParams()
    {
      if (this.EnableAdvancedDeferredFog)
      {
        Shader.EnableKeyword("LUXWATER_DEFERREDFOG");
        Vector4 vector4;
        ((Vector4) ref vector4).\u002Ector(!this.DoUnderWaterRendering ? 0.0f : 1f, this.FogDepthShift, this.FogEdgeBlending, 0.0f);
        Shader.SetGlobalVector(this.Lux_UnderWaterDeferredFogParams, vector4);
      }
      else
        Shader.DisableKeyword("LUXWATER_DEFERREDFOG");
    }

    public void SetDeepwaterLighting()
    {
      if (this.EnableDeepwaterLighting)
      {
        Shader.EnableKeyword("LUXWATER_DEEPWATERLIGHTING");
        if (this.activeWaterVolume != -1)
          Shader.SetGlobalFloat("_Lux_UnderWaterWaterSurfacePos", this.WaterSurfacePos);
        else
          Shader.SetGlobalFloat("_Lux_UnderWaterWaterSurfacePos", this.DefaultWaterSurfacePosition);
        Shader.SetGlobalFloat("_Lux_UnderWaterDirLightingDepth", this.DirectionalLightingFadeRange);
        Shader.SetGlobalFloat("_Lux_UnderWaterFogLightingDepth", this.FogLightingFadeRange);
      }
      else
        Shader.DisableKeyword("LUXWATER_DEEPWATERLIGHTING");
    }

    public void RegisterWaterVolume(
      LuxWater_WaterVolume item,
      int ID,
      bool visible,
      bool SlidingVolume)
    {
      this.RegisteredWaterVolumesIDs.Add(ID);
      this.RegisteredWaterVolumes.Add(item);
      this.WaterMeshes.Add(item.WaterVolumeMesh);
      this.WaterMaterials.Add(((Renderer) ((Component) ((Component) item).get_transform()).GetComponent<Renderer>()).get_sharedMaterial());
      this.WaterTransforms.Add(((Component) item).get_transform());
      this.WaterIsOnScreen.Add(visible);
      this.WaterUsesSlidingVolume.Add(SlidingVolume);
      int index = this.WaterMaterials.Count - 1;
      Shader.SetGlobalTexture(this.Lux_UnderWaterCausticsPID, this.WaterMaterials[index].GetTexture(this.CausticTexPID));
      this.SetGerstnerWaves(index);
    }

    public void DeRegisterWaterVolume(LuxWater_WaterVolume item, int ID)
    {
      int index = this.RegisteredWaterVolumesIDs.IndexOf(ID);
      if (this.activeWaterVolume == index)
        this.activeWaterVolume = -1;
      if (index == -1)
        return;
      this.RegisteredWaterVolumesIDs.RemoveAt(index);
      this.RegisteredWaterVolumes.RemoveAt(index);
      this.WaterMeshes.RemoveAt(index);
      this.WaterMaterials.RemoveAt(index);
      this.WaterTransforms.RemoveAt(index);
      this.WaterIsOnScreen.RemoveAt(index);
      this.WaterUsesSlidingVolume.RemoveAt(index);
    }

    public void SetWaterVisible(int ID)
    {
      int index = this.RegisteredWaterVolumesIDs.IndexOf(ID);
      if (index < 0 || index >= this.WaterIsOnScreen.Count)
        return;
      this.WaterIsOnScreen[index] = true;
    }

    public void SetWaterInvisible(int ID)
    {
      int index = this.RegisteredWaterVolumesIDs.IndexOf(ID);
      if (index < 0 || index >= this.WaterIsOnScreen.Count)
        return;
      this.WaterIsOnScreen[index] = false;
    }

    public void EnteredWaterVolume(
      LuxWater_WaterVolume item,
      int ID,
      Camera triggerCam,
      float GridSize)
    {
      this.DoUnderWaterRendering = true;
      int num = this.RegisteredWaterVolumesIDs.IndexOf(ID);
      if (num != this.activeWaterVolume)
      {
        this.activeWaterVolume = num;
        this.activeGridSize = GridSize;
        this.WaterSurfacePos = (float) this.WaterTransforms[this.activeWaterVolume].get_position().y;
        for (int index = 0; index < this.m_aboveWatersurface.Count; ++index)
          this.m_aboveWatersurface[index].set_renderQueue(2997);
        for (int index = 0; index < this.m_belowWatersurface.Count; ++index)
          this.m_belowWatersurface[index].set_renderQueue(3001);
      }
      if (this.activeWaterVolumeCameras.Contains(triggerCam))
        return;
      this.activeWaterVolumeCameras.Add(triggerCam);
    }

    public void LeftWaterVolume(LuxWater_WaterVolume item, int ID, Camera triggerCam)
    {
      this.DoUnderWaterRendering = false;
      if (this.activeWaterVolume == this.RegisteredWaterVolumesIDs.IndexOf(ID))
      {
        this.activeWaterVolume = -1;
        for (int index = 0; index < this.m_aboveWatersurface.Count; ++index)
          this.m_aboveWatersurface[index].set_renderQueue(3000);
        for (int index = 0; index < this.m_belowWatersurface.Count; ++index)
          this.m_belowWatersurface[index].set_renderQueue(2997);
      }
      if (!this.activeWaterVolumeCameras.Contains(triggerCam))
        return;
      this.activeWaterVolumeCameras.Remove(triggerCam);
    }

    private void OnPreCull()
    {
      this.SetDeferredFogParams();
      this.RenderWaterMask(this.cam, false);
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
      this.RenderUnderWater(src, dest, this.cam, false);
    }

    public void SetGerstnerWaves(int index)
    {
      if ((double) this.WaterMaterials[index].GetFloat(this.GerstnerEnabledPID) == 1.0)
      {
        this.mat.EnableKeyword("GERSTNERENABLED");
        this.mat.SetVector(this.LuxWaterMask_GerstnerVertexIntensityPID, this.WaterMaterials[index].GetVector(this.GerstnerVertexIntensityPID));
        this.mat.SetVector(this.LuxWaterMask_GAmplitudePID, this.WaterMaterials[index].GetVector(this.GAmplitudePID));
        this.mat.SetVector(this.LuxWaterMask_GFinalFrequencyPID, this.WaterMaterials[index].GetVector(this.GFinalFrequencyPID));
        this.mat.SetVector(this.LuxWaterMask_GSteepnessPID, this.WaterMaterials[index].GetVector(this.GSteepnessPID));
        this.mat.SetVector(this.LuxWaterMask_GFinalSpeedPID, this.WaterMaterials[index].GetVector(this.GFinalSpeedPID));
        this.mat.SetVector(this.LuxWaterMask_GDirectionABPID, this.WaterMaterials[index].GetVector(this.GDirectionABPID));
        this.mat.SetVector(this.LuxWaterMask_GDirectionCDPID, this.WaterMaterials[index].GetVector(this.GDirectionCDPID));
        this.mat.SetVector(this.LuxWaterMask_GerstnerSecondaryWaves, this.WaterMaterials[index].GetVector(this.GerstnerSecondaryWaves));
      }
      else
        this.mat.DisableKeyword("GERSTNERENABLED");
    }

    public void RenderWaterMask(Camera currentCamera, bool SecondaryCameraRendering)
    {
      Shader.SetGlobalFloat("_Lux_Time", Time.get_timeSinceLevelLoad());
      Camera camera = currentCamera;
      camera.set_depthTextureMode((DepthTextureMode) (camera.get_depthTextureMode() | 1));
      this.camTransform = ((Component) currentCamera).get_transform();
      if (!Object.op_Implicit((Object) this.UnderWaterMask))
        this.UnderWaterMask = new RenderTexture(currentCamera.get_pixelWidth(), currentCamera.get_pixelHeight(), 24, (RenderTextureFormat) 0, (RenderTextureReadWrite) 1);
      else if (((Texture) this.UnderWaterMask).get_width() != currentCamera.get_pixelWidth() && !SecondaryCameraRendering)
        this.UnderWaterMask = new RenderTexture(currentCamera.get_pixelWidth(), currentCamera.get_pixelHeight(), 24, (RenderTextureFormat) 0, (RenderTextureReadWrite) 1);
      Shader.SetGlobalTexture(this.UnderWaterMaskPID, (Texture) this.UnderWaterMask);
      Graphics.SetRenderTarget(this.UnderWaterMask);
      currentCamera.CalculateFrustumCorners(new Rect(0.0f, 0.0f, 1f, 1f), currentCamera.get_farClipPlane(), currentCamera.get_stereoActiveEye(), this.frustumCorners);
      Vector3 vector3_1 = this.camTransform.TransformVector(this.frustumCorners[0]);
      Vector3 vector3_2 = this.camTransform.TransformVector(this.frustumCorners[1]);
      Vector3 vector3_3 = this.camTransform.TransformVector(this.frustumCorners[2]);
      Vector3 vector3_4 = this.camTransform.TransformVector(this.frustumCorners[3]);
      ((Matrix4x4) ref this.frustumCornersArray).SetRow(0, Vector4.op_Implicit(vector3_1));
      ((Matrix4x4) ref this.frustumCornersArray).SetRow(1, Vector4.op_Implicit(vector3_4));
      ((Matrix4x4) ref this.frustumCornersArray).SetRow(2, Vector4.op_Implicit(vector3_2));
      ((Matrix4x4) ref this.frustumCornersArray).SetRow(3, Vector4.op_Implicit(vector3_3));
      Shader.SetGlobalMatrix(this.Lux_FrustumCornersWSPID, this.frustumCornersArray);
      Shader.SetGlobalVector(this.Lux_CameraWSPID, Vector4.op_Implicit(this.camTransform.get_position()));
      this.ambientProbe = RenderSettings.get_ambientProbe();
      ((SphericalHarmonicsL2) ref this.ambientProbe).Evaluate(this.directions, this.AmbientLightingSamples);
      if (this.islinear)
      {
        int ambientSkyLightPid = this.Lux_UnderWaterAmbientSkyLightPID;
        Color color = Color.op_Multiply(this.AmbientLightingSamples[0], RenderSettings.get_ambientIntensity());
        Color linear = ((Color) ref color).get_linear();
        Shader.SetGlobalColor(ambientSkyLightPid, linear);
      }
      else
        Shader.SetGlobalColor(this.Lux_UnderWaterAmbientSkyLightPID, Color.op_Multiply(this.AmbientLightingSamples[0], RenderSettings.get_ambientIntensity()));
      if (this.activeWaterVolumeCameras.Contains(currentCamera) || this.EnableAdvancedDeferredFog)
        ;
      if (this.activeWaterVolume > -1)
      {
        Shader.EnableKeyword("LUXWATERENABLED");
        if (!this.EnableDeepwaterLighting)
        {
          Shader.SetGlobalFloat("_Lux_UnderWaterDirLightingDepth", this.WaterMaterials[this.activeWaterVolume].GetFloat(this.Lux_MaxDirLightDepthPID));
          Shader.SetGlobalFloat("_Lux_UnderWaterFogLightingDepth", this.WaterMaterials[this.activeWaterVolume].GetFloat(this.Lux_MaxFogLightDepthPID));
        }
        Shader.SetGlobalFloat("_Lux_UnderWaterWaterSurfacePos", this.WaterSurfacePos);
      }
      else
        Shader.DisableKeyword("LUXWATERENABLED");
      GL.PushMatrix();
      GL.Clear(true, true, Color.get_black(), 1f);
      this.camProj = currentCamera.get_projectionMatrix();
      GL.LoadProjectionMatrix(this.camProj);
      Shader.SetGlobalVector("_WorldSpaceCameraPos", Vector4.op_Implicit(this.camTransform.get_position()));
      Shader.SetGlobalVector("_ProjectionParams", new Vector4(this.Projection, currentCamera.get_nearClipPlane(), currentCamera.get_farClipPlane(), 1f / currentCamera.get_farClipPlane()));
      Shader.SetGlobalVector("_ScreenParams", new Vector4((float) currentCamera.get_pixelWidth(), (float) currentCamera.get_pixelHeight(), (float) (1.0 + 1.0 / (double) currentCamera.get_pixelWidth()), (float) (1.0 + 1.0 / (double) currentCamera.get_pixelHeight())));
      for (int index = 0; index < this.RegisteredWaterVolumes.Count; ++index)
      {
        if ((this.WaterIsOnScreen[index] || index == this.activeWaterVolume) && (this.EnableAdvancedDeferredFog || index == this.activeWaterVolume))
        {
          this.WatervolumeMatrix = this.WaterTransforms[index].get_localToWorldMatrix();
          if (this.WaterUsesSlidingVolume[index])
          {
            Vector3 position = this.camTransform.get_position();
            Vector4 column = ((Matrix4x4) ref this.WatervolumeMatrix).GetColumn(3);
            Vector3 lossyScale = this.WaterTransforms[index].get_lossyScale();
            Vector2 vector2;
            ((Vector2) ref vector2).\u002Ector(this.activeGridSize * (float) lossyScale.x, this.activeGridSize * (float) lossyScale.z);
            float num1 = (float) Math.Round((double) (position.x / vector2.x));
            float num2 = (float) vector2.x * num1;
            float num3 = (float) Math.Round((double) (position.z / vector2.y));
            float num4 = (float) vector2.y * num3;
            column.x = (__Null) ((double) num2 + column.x % vector2.x);
            column.z = (__Null) ((double) num4 + column.z % vector2.y);
            ((Matrix4x4) ref this.WatervolumeMatrix).SetColumn(3, column);
          }
          Material waterMaterial = this.WaterMaterials[index];
          if ((double) waterMaterial.GetFloat(this.GerstnerEnabledPID) == 1.0)
          {
            this.mat.EnableKeyword("GERSTNERENABLED");
            this.mat.SetVector(this.LuxWaterMask_GerstnerVertexIntensityPID, waterMaterial.GetVector(this.GerstnerVertexIntensityPID));
            this.mat.SetVector(this.LuxWaterMask_GAmplitudePID, waterMaterial.GetVector(this.GAmplitudePID));
            this.mat.SetVector(this.LuxWaterMask_GFinalFrequencyPID, waterMaterial.GetVector(this.GFinalFrequencyPID));
            this.mat.SetVector(this.LuxWaterMask_GSteepnessPID, waterMaterial.GetVector(this.GSteepnessPID));
            this.mat.SetVector(this.LuxWaterMask_GFinalSpeedPID, waterMaterial.GetVector(this.GFinalSpeedPID));
            this.mat.SetVector(this.LuxWaterMask_GDirectionABPID, waterMaterial.GetVector(this.GDirectionABPID));
            this.mat.SetVector(this.LuxWaterMask_GDirectionCDPID, waterMaterial.GetVector(this.GDirectionCDPID));
            this.mat.SetVector(this.LuxWaterMask_GerstnerSecondaryWaves, waterMaterial.GetVector(this.GerstnerSecondaryWaves));
          }
          else
            this.mat.DisableKeyword("GERSTNERENABLED");
          bool flag = waterMaterial.HasProperty(this.Lux_EdgeLengthPID) && SystemInfo.get_graphicsShaderLevel() >= 46;
          if (flag)
            this.mat.SetFloat(this.Lux_EdgeLengthPID, waterMaterial.GetFloat(this.Lux_EdgeLengthPID));
          if (index == this.activeWaterVolume && this.activeWaterVolumeCameras.Contains(currentCamera))
          {
            if (this.WaterUsesSlidingVolume[index] && flag)
              this.mat.SetPass(5);
            else
              this.mat.SetPass(0);
            Graphics.DrawMeshNow(this.WaterMeshes[index], this.WatervolumeMatrix, 0);
          }
          if (index == this.activeWaterVolume && this.activeWaterVolumeCameras.Contains(currentCamera) || this.EnableAdvancedDeferredFog)
          {
            if (flag)
            {
              if (index == this.activeWaterVolume)
                this.mat.SetPass(3);
              else
                this.mat.SetPass(4);
            }
            else if (index == this.activeWaterVolume)
              this.mat.SetPass(1);
            else
              this.mat.SetPass(2);
            Graphics.DrawMeshNow(this.WaterMeshes[index], this.WatervolumeMatrix, 1);
          }
        }
      }
      GL.PopMatrix();
    }

    public void RenderUnderWater(
      RenderTexture src,
      RenderTexture dest,
      Camera currentCamera,
      bool SecondaryCameraRendering)
    {
      if (this.activeWaterVolumeCameras.Contains(currentCamera))
      {
        if (this.DoUnderWaterRendering && this.activeWaterVolume > -1)
        {
          if (!this.UnderwaterIsSetUp)
          {
            if (Object.op_Implicit((Object) this.Sun))
            {
              Vector3 vector3 = Vector3.op_UnaryNegation(this.Sun.get_forward());
              Color color = Color.op_Multiply(this.SunLight.get_color(), this.SunLight.get_intensity());
              if (this.islinear)
                color = ((Color) ref color).get_linear();
              Shader.SetGlobalColor(this.Lux_UnderWaterSunColorPID, Color.op_Multiply(color, Mathf.Clamp01(Vector3.Dot(vector3, Vector3.get_up()))));
              Shader.SetGlobalVector(this.Lux_UnderWaterSunDirPID, Vector4.op_Implicit(Vector3.op_UnaryNegation(vector3)));
              Shader.SetGlobalVector(this.Lux_UnderWaterSunDirViewSpacePID, Vector4.op_Implicit(currentCamera.WorldToViewportPoint(Vector3.op_Subtraction(((Component) currentCamera).get_transform().get_position(), Vector3.op_Multiply(vector3, 1000f)))));
            }
            if ((double) this.WaterMaterials[this.activeWaterVolume].GetFloat(this.Lux_CausticsEnabledPID) == 1.0)
            {
              this.blitMaterial.EnableKeyword("GEOM_TYPE_FROND");
              if ((double) this.WaterMaterials[this.activeWaterVolume].GetFloat(this.Lux_CausticModePID) == 1.0)
                this.blitMaterial.EnableKeyword("GEOM_TYPE_LEAF");
              else
                this.blitMaterial.DisableKeyword("GEOM_TYPE_LEAF");
            }
            else
              this.blitMaterial.DisableKeyword("GEOM_TYPE_FROND");
            if (this.islinear)
            {
              int waterFogColorPid = this.Lux_UnderWaterFogColorPID;
              Color color = this.WaterMaterials[this.activeWaterVolume].GetColor("_Color");
              Color linear = ((Color) ref color).get_linear();
              Shader.SetGlobalColor(waterFogColorPid, linear);
            }
            else
              Shader.SetGlobalColor(this.Lux_UnderWaterFogColorPID, this.WaterMaterials[this.activeWaterVolume].GetColor("_Color"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterFogDensityPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_Density"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterFogAbsorptionCancellationPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_FogAbsorptionCancellation"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterAbsorptionHeightPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_AbsorptionHeight"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterAbsorptionMaxHeightPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_AbsorptionMaxHeight"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterAbsorptionDepthPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_AbsorptionDepth"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterAbsorptionColorStrengthPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_AbsorptionColorStrength"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterAbsorptionStrengthPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_AbsorptionStrength"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterUnderwaterScatteringPowerPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_ScatteringPower"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterUnderwaterScatteringIntensityPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_UnderwaterScatteringIntensity"));
            if (this.islinear)
            {
              int scatteringColorPid = this.Lux_UnderWaterUnderwaterScatteringColorPID;
              Color color = this.WaterMaterials[this.activeWaterVolume].GetColor("_TranslucencyColor");
              Color linear = ((Color) ref color).get_linear();
              Shader.SetGlobalColor(scatteringColorPid, linear);
            }
            else
              Shader.SetGlobalColor(this.Lux_UnderWaterUnderwaterScatteringColorPID, this.WaterMaterials[this.activeWaterVolume].GetColor("_TranslucencyColor"));
            Shader.SetGlobalFloat(this.Lux_UnderWaterUnderwaterScatteringOcclusionPID, this.WaterMaterials[this.activeWaterVolume].GetFloat("_ScatterOcclusion"));
            Shader.SetGlobalTexture(this.Lux_UnderWaterCausticsPID, this.WaterMaterials[this.activeWaterVolume].GetTexture(this.CausticTexPID));
            Shader.SetGlobalFloat("_Lux_UnderWaterCausticsTiling", this.WaterMaterials[this.activeWaterVolume].GetFloat("_CausticsTiling"));
            Shader.SetGlobalFloat("_Lux_UnderWaterCausticsScale", this.WaterMaterials[this.activeWaterVolume].GetFloat("_CausticsScale"));
            Shader.SetGlobalFloat("_Lux_UnderWaterCausticsSpeed", this.WaterMaterials[this.activeWaterVolume].GetFloat("_CausticsSpeed"));
            Shader.SetGlobalFloat("_Lux_UnderWaterCausticsTiling", this.WaterMaterials[this.activeWaterVolume].GetFloat("_CausticsTiling"));
            Shader.SetGlobalFloat("_Lux_UnderWaterCausticsSelfDistortion", this.WaterMaterials[this.activeWaterVolume].GetFloat("_CausticsSelfDistortion"));
            Shader.SetGlobalVector("_Lux_UnderWaterFinalBumpSpeed01", this.WaterMaterials[this.activeWaterVolume].GetVector("_FinalBumpSpeed01"));
            Shader.SetGlobalVector("_Lux_UnderWaterFogDepthAtten", this.WaterMaterials[this.activeWaterVolume].GetVector("_DepthAtten"));
          }
          Graphics.Blit((Texture) src, dest, this.blitMaterial, 0);
        }
        else
          Graphics.Blit((Texture) src, dest);
      }
      else
        Graphics.Blit((Texture) src, dest);
    }
  }
}
