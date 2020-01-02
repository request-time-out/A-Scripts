// Decompiled with JetBrains decompiler
// Type: EnviroFogSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroFogSettings
{
  [Header("Mode")]
  [Tooltip("Unity's fog mode.")]
  public FogMode Fogmode = (FogMode) 2;
  [Header("Distance Fog")]
  [Tooltip("Use distance fog?")]
  public bool distanceFog = true;
  [Tooltip("Use radial distance fog?")]
  public bool useRadialDistance = true;
  [Range(0.0f, 10f)]
  [Tooltip("The intensity of distance fog.")]
  public float distanceFogIntensity = 4f;
  [Range(0.0f, 1f)]
  [Tooltip("The maximum density of fog.")]
  public float maximumFogDensity = 0.9f;
  [Header("Height Fog")]
  [Tooltip("Use heightbased fog?")]
  public bool heightFog = true;
  [Tooltip("The height of heightbased fog.")]
  public float height = 90f;
  [Range(0.0f, 1f)]
  [Tooltip("The intensity of heightbased fog.")]
  public float heightFogIntensity = 1f;
  [HideInInspector]
  public float heightDensity = 0.15f;
  [Header("Height Fog Noise")]
  [Range(0.0f, 1f)]
  [Tooltip("The noise intensity of height based fog.")]
  public float noiseIntensity = 1f;
  [Tooltip("The noise intensity offset of height based fog.")]
  [Range(0.0f, 1f)]
  public float noiseIntensityOffset = 0.3f;
  [Range(0.0f, 0.1f)]
  [Tooltip("The noise scaling of height based fog.")]
  public float noiseScale = 1f / 1000f;
  [Tooltip("The speed and direction of height based fog.")]
  public Vector2 noiseVelocity = new Vector2(3f, 1.5f);
  [Header("Fog Scattering")]
  [Tooltip("Influence scattering near sun.")]
  public float mie = 5f;
  [Tooltip("Influence scattering near sun.")]
  public float g = 5f;
  [Header("Fog Dithering")]
  [Tooltip("Fog dithering settings to reduce color banding.")]
  public float fogDitheringScale = 240f;
  [Tooltip("Fog dithering settings to reduce color banding.")]
  public float fogDitheringIntensity = 0.5f;
  [HideInInspector]
  public float skyFogIntensity = 1f;
  [Tooltip("Simple fog = just plain color without scattering.")]
  public bool useSimpleFog;
  [Tooltip("The distance where fog starts.")]
  public float startDistance;
}
