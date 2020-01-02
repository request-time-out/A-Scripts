// Decompiled with JetBrains decompiler
// Type: EnviroSkySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroSkySettings
{
  [Header("Scattering")]
  [Tooltip("Light Wavelength used for atmospheric scattering. Keep it near defaults for earthlike atmospheres, or change for alien or fantasy atmospheres for example.")]
  public Vector3 waveLength = new Vector3(540f, 496f, 437f);
  [Tooltip("Influence atmospheric scattering.")]
  public float rayleigh = 5.15f;
  [Tooltip("Sky turbidity. Particle in air. Influence atmospheric scattering.")]
  public float turbidity = 1f;
  [Tooltip("Influence scattering near sun.")]
  public float mie = 5f;
  [Tooltip("Influence scattering near sun.")]
  public float g = 0.8f;
  [Tooltip("Intensity gradient for atmospheric scattering. Influence atmospheric scattering based on current sun altitude.")]
  public AnimationCurve scatteringCurve = new AnimationCurve();
  [Header("Sun")]
  public EnviroSkySettings.SunAndMoonCalc sunAndMoonPosition = EnviroSkySettings.SunAndMoonCalc.Realistic;
  [Tooltip("Intensity of Sun Influence Scale and Dropoff of sundisk.")]
  public float sunIntensity = 100f;
  [Tooltip("Scale of rendered sundisk.")]
  public float sunDiskScale = 20f;
  [Tooltip("Intenisty of rendered sundisk.")]
  public float sunDiskIntensity = 3f;
  [Header("Moon")]
  [Tooltip("Whether to render the moon.")]
  public bool renderMoon = true;
  [Tooltip("The Moon phase mode. Custom = for customizable phase.")]
  public EnviroSkySettings.MoonPhases moonPhaseMode = EnviroSkySettings.MoonPhases.Realistic;
  [Range(0.0f, 5f)]
  [Tooltip("Brightness of the moon.")]
  public float moonBrightness = 1f;
  [Range(0.0f, 20f)]
  [Tooltip("Size of the moon.")]
  public float moonSize = 10f;
  [Tooltip("Glow around moon.")]
  public AnimationCurve moonGlow = new AnimationCurve();
  [Header("Sky Color Corrections")]
  [Tooltip("Higher values = brighter sky.")]
  public AnimationCurve skyLuminence = new AnimationCurve();
  [Tooltip("Higher values = stronger colors applied BEFORE clouds rendered!")]
  public AnimationCurve skyColorPower = new AnimationCurve();
  [Header("Tonemapping - LDR")]
  [Tooltip("Tonemapping when using LDR")]
  public float skyExposure = 1.5f;
  [Tooltip("Intensity of stars based on time of day.")]
  public AnimationCurve starsIntensity = new AnimationCurve();
  [Tooltip("Intensity of galaxy based on time of day.")]
  public AnimationCurve galaxyIntensity = new AnimationCurve();
  [Header("Sky Dithering")]
  public float noiseScale = 10f;
  public float noiseIntensity = 1.5f;
  [Header("Sky Mode:")]
  [Tooltip("Select if you want to use enviro skybox your custom material.")]
  public EnviroSkySettings.SkyboxModi skyboxMode;
  [Tooltip("If SkyboxMode == CustomSkybox : Assign your skybox material here!")]
  public Material customSkyboxMaterial;
  [Tooltip("If SkyboxMode == CustomColor : Select your sky color here!")]
  public Color customSkyboxColor;
  [Tooltip("Enable to render black skybox at ground level.")]
  public bool blackGroundMode;
  [Tooltip("Color gradient for atmospheric scattering. Influence atmospheric scattering based on current sun altitude.")]
  public Gradient scatteringColor;
  [Tooltip("Color gradient for sundisk. Influence sundisk color based on current sun altitude")]
  public Gradient sunDiskColor;
  [Tooltip("The Moon texture.")]
  public Texture moonTexture;
  [Tooltip("The color of the moon")]
  public Color moonColor;
  [Tooltip("Glow color around moon.")]
  public Color moonGlowColor;
  [Tooltip("Start moon phase when using custom phase mode.(-1f - 1f)")]
  [Range(-1f, 1f)]
  public float startMoonPhase;
  [Header("Stars")]
  [Tooltip("A cubemap for night sky.")]
  public Cubemap starsCubeMap;
  [Header("Galaxy")]
  [Tooltip("A cubemap for night galaxy.")]
  public Cubemap galaxyCubeMap;

  public enum SunAndMoonCalc
  {
    Simple,
    Realistic,
  }

  public enum MoonPhases
  {
    Custom,
    Realistic,
  }

  public enum SkyboxModi
  {
    Default,
    CustomSkybox,
    CustomColor,
  }
}
