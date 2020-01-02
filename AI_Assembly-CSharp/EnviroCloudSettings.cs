// Decompiled with JetBrains decompiler
// Type: EnviroCloudSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroCloudSettings
{
  [Range(10000f, 486000f)]
  [Header("Clouds Scale Settings")]
  [Tooltip("Clouds world scale. This settings will influece rendering of clouds at horizon.")]
  public float cloudsWorldScale = 113081f;
  [Tooltip("Clouds start height.")]
  public float bottomCloudHeight = 3000f;
  [Tooltip("Clouds end height.")]
  public float topCloudHeight = 7000f;
  [Range(-1f, 1f)]
  [Tooltip("Time scale / wind animation speed of clouds.")]
  public float cloudsTimeScale = 1f;
  [Range(0.0f, 1f)]
  [Tooltip("Global clouds wind speed modificator.")]
  public float cloudsWindStrengthModificator = 1f / 1000f;
  [Range(-1f, 1f)]
  [Tooltip("Global clouds wind direction X axes.")]
  public float cloudsWindDirectionX = 1f;
  [Range(-1f, 1f)]
  [Tooltip("Global clouds wind direction Y axes.")]
  public float cloudsWindDirectionY = 1f;
  [Header("Cloud Rendering")]
  [Range(32f, 256f)]
  [Tooltip("Number of raymarching samples.")]
  public int raymarchSteps = 150;
  [Tooltip("Increase performance by using less steps when clouds are hidden by objects.")]
  [Range(0.1f, 1f)]
  public float stepsInDepthModificator = 0.75f;
  [Range(1f, 16f)]
  [Tooltip("Downsampling of clouds rendering. 1 = full res, 2 = half Res, ...")]
  public int cloudsRenderResolution = 4;
  [Header("Clouds Lighting")]
  public float hgPhase = 0.5f;
  public float primaryAttenuation = 3f;
  public float secondaryAttenuation = 3f;
  [Tooltip("Direct Light intensity for clouds based on time of day.")]
  public AnimationCurve directLightIntensity = new AnimationCurve();
  [Tooltip("Direct Light intensity for clouds based on time of day.")]
  public AnimationCurve ambientLightIntensity = new AnimationCurve();
  [Tooltip("Tonemapping exposure")]
  public float cloudsExposure = 1f;
  [Tooltip("LOD Distance for using lower res 3d texture for far away clouds. ")]
  [Range(0.0f, 1f)]
  public float lodDistance = 0.5f;
  [Header("Weather Map")]
  [Tooltip("Tiling of the generated weather map.")]
  public int weatherMapTiling = 5;
  [Range(0.0f, 1f)]
  [Tooltip("Weathermap animation speed.")]
  public float weatherAnimSpeedScale = 0.33f;
  [Header("Clouds Modelling")]
  [Tooltip("The UV scale of base noise. High Values = Low performance!")]
  [Range(2f, 100f)]
  public float baseNoiseUV = 20f;
  [Tooltip("The UV scale of detail noise. High Values = Low performance!")]
  [Range(2f, 100f)]
  public float detailNoiseUV = 50f;
  [Header("Global Clouds Control")]
  [Range(0.0f, 2f)]
  public float globalCloudCoverage = 1f;
  [Range(5f, 15f)]
  [Tooltip("Flat Clouds Altitude")]
  public float cirrusCloudsAltitude = 10f;
  [Tooltip("Resolution of generated flat clouds texture.")]
  public EnviroCloudSettings.FlatCloudResolution flatCloudsResolution = EnviroCloudSettings.FlatCloudResolution.R2048;
  [Tooltip("Scale/Tiling of flat clouds.")]
  public float flatCloudsScale = 2f;
  [Range(1f, 12f)]
  [Tooltip("Flat Clouds texture generation iterations.")]
  public int flatCloudsNoiseOctaves = 6;
  [Range(30f, 100f)]
  [Tooltip("Flat Clouds Altitude")]
  public float flatCloudsAltitude = 70f;
  [Range(0.01f, 1f)]
  [Tooltip("Flat Clouds morphing animation speed.")]
  public float flatCloudsMorphingSpeed = 0.2f;
  [Tooltip("Size of the shadow cookie.")]
  [Range(1000f, 10000f)]
  public int shadowCookieSize = 10000;
  public EnviroCloudSettings.ReprojectionPixelSize reprojectionPixelSize;
  [Tooltip("Choose a clouds rendering quality or setup your own when choosing custom.")]
  public EnviroCloudSettings.CloudQuality cloudsQuality;
  [Header("Clouds Wind Animation")]
  public bool useWindZoneDirection;
  [Tooltip("Global Color for volume clouds based sun positon.")]
  public Gradient volumeCloudsColor;
  [Tooltip("Global Color for clouds based moon positon.")]
  public Gradient volumeCloudsMoonColor;
  [Header("Tonemapping")]
  [Tooltip("Use color tonemapping?")]
  public bool tonemapping;
  [Tooltip("Option to add own weather map. Red Channel = Coverage, Blue = Clouds Height")]
  public Texture2D customWeatherMap;
  [Tooltip("Weathermap sampling offset.")]
  public Vector2 locationOffset;
  [Tooltip("Resolution of Detail Noise Texture.")]
  public EnviroCloudSettings.CloudDetailQuality detailQuality;
  [Tooltip("Texture for cirrus clouds.")]
  public Texture cirrusCloudsTexture;
  [Tooltip("Global Color for flat clouds based sun positon.")]
  public Gradient cirrusCloudsColor;
  [Tooltip("Texture for flat procedural clouds.")]
  public Texture flatCloudsNoiseTexture;
  [Tooltip("Global Color for flat clouds based sun positon.")]
  public Gradient flatCloudsColor;
  [Tooltip("Clouds Shadowcast Intensity. 0 = disabled")]
  [Range(0.0f, 1f)]
  public float shadowIntensity;

  public enum CloudDetailQuality
  {
    Low,
    High,
  }

  public enum CloudQuality
  {
    Lowest,
    Low,
    Medium,
    High,
    Ultra,
    VR_Low,
    VR_Medium,
    VR_High,
    Custom,
  }

  public enum FlatCloudResolution
  {
    R512,
    R1024,
    R2048,
    R4096,
  }

  public enum ReprojectionPixelSize
  {
    Off,
    Low,
    Medium,
    High,
  }
}
