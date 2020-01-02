// Decompiled with JetBrains decompiler
// Type: EnviroAquasIntegration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Enviro/Integration/AQUAS Integration")]
public class EnviroAquasIntegration : MonoBehaviour
{
  [Header("AQUAS Water Plane")]
  public GameObject waterObject;
  [Header("Setup")]
  public bool deactivateAquasReflectionProbe;
  public bool deactivateEnviroFogUnderwater;
  [Header("Settings")]
  [Range(0.0f, 1f)]
  public float underwaterFogColorInfluence;
  private AQUAS_LensEffects aquasUnderWater;
  private bool isUnderWater;
  private bool defaultDistanceFog;
  private bool defaultHeightFog;

  public EnviroAquasIntegration()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
    {
      Debug.Log((object) "No EnviroSky in scene! This component will be disabled!");
      ((Behaviour) this).set_enabled(false);
    }
    else
    {
      if (Object.op_Inequality((Object) GameObject.Find("UnderWaterCameraEffects"), (Object) null))
        this.aquasUnderWater = (AQUAS_LensEffects) GameObject.Find("UnderWaterCameraEffects").GetComponent<AQUAS_LensEffects>();
      this.defaultDistanceFog = EnviroSky.instance.fogSettings.distanceFog;
      this.defaultHeightFog = EnviroSky.instance.fogSettings.heightFog;
      this.SetupEnviroWithAQUAS();
    }
  }

  private void Update()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null) || !Object.op_Inequality((Object) this.waterObject, (Object) null) || !Object.op_Inequality((Object) this.aquasUnderWater, (Object) null))
      return;
    if (this.aquasUnderWater.underWater && !this.isUnderWater)
    {
      if (this.deactivateEnviroFogUnderwater)
      {
        EnviroSky.instance.fogSettings.distanceFog = false;
        EnviroSky.instance.fogSettings.heightFog = false;
        EnviroSky.instance.customFogIntensity = this.underwaterFogColorInfluence;
      }
      EnviroSky.instance.updateFogDensity = false;
      EnviroSky.instance.Audio.ambientSFXVolumeMod = -1f;
      EnviroSky.instance.Audio.weatherSFXVolumeMod = -1f;
      this.isUnderWater = true;
    }
    else
    {
      if (this.aquasUnderWater.underWater || !this.isUnderWater)
        return;
      if (this.deactivateEnviroFogUnderwater)
      {
        EnviroSky.instance.updateFogDensity = true;
        EnviroSky.instance.fogSettings.distanceFog = this.defaultDistanceFog;
        EnviroSky.instance.fogSettings.heightFog = this.defaultHeightFog;
        RenderSettings.set_fogDensity(EnviroSky.instance.Weather.currentActiveWeatherPreset.fogDensity);
        EnviroSky.instance.customFogColor = this.aquasUnderWater.underWaterParameters.fogColor;
        EnviroSky.instance.customFogIntensity = 0.0f;
      }
      EnviroSky.instance.Audio.ambientSFXVolumeMod = 0.0f;
      EnviroSky.instance.Audio.weatherSFXVolumeMod = 0.0f;
      this.isUnderWater = false;
    }
  }

  public void SetupEnviroWithAQUAS()
  {
    if (Object.op_Inequality((Object) this.waterObject, (Object) null))
    {
      if (this.deactivateAquasReflectionProbe)
        this.DeactivateReflectionProbe(this.waterObject);
      if (!EnviroSky.instance.fogSettings.distanceFog && !EnviroSky.instance.fogSettings.heightFog)
        this.deactivateEnviroFogUnderwater = false;
      if (!Object.op_Inequality((Object) this.aquasUnderWater, (Object) null))
        return;
      this.aquasUnderWater.setAfloatFog = false;
    }
    else
    {
      Debug.Log((object) "AQUAS Object not found! This component will be disabled!");
      ((Behaviour) this).set_enabled(false);
    }
  }

  private void DeactivateReflectionProbe(GameObject aquas)
  {
    GameObject gameObject = GameObject.Find(((Object) aquas).get_name() + "/Reflection Probe");
    if (Object.op_Inequality((Object) gameObject, (Object) null))
      ((Behaviour) gameObject.GetComponent<ReflectionProbe>()).set_enabled(false);
    else
      Debug.Log((object) "Cannot find AQUAS Reflection Probe!");
  }
}
