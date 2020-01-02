// Decompiled with JetBrains decompiler
// Type: EnviroWeatherCloudsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroWeatherCloudsConfig
{
  [Tooltip("Top color of clouds.")]
  public Color topColor = Color.get_white();
  [Tooltip("Bottom color of clouds.")]
  public Color bottomColor = Color.get_grey();
  [Tooltip("Sky blend factor.")]
  [Range(0.0f, 1f)]
  public float skyBlending = 1f;
  [Tooltip("Light inscattering factor.")]
  [Range(0.0f, 2f)]
  public float alphaCoef = 1f;
  [Tooltip("Light extinction factor.")]
  [Range(0.0f, 2f)]
  public float scatteringCoef = 1f;
  [Tooltip("Density factor of clouds.")]
  [Range(0.0f, 1f)]
  public float density = 1f;
  [Tooltip("Global coverage multiplicator of clouds.")]
  [Range(0.0f, 1f)]
  public float coverage = 1f;
  [Tooltip("Global coverage height multiplicator of clouds.")]
  [Range(0.0f, 1f)]
  public float coverageHeight = 1f;
  [Tooltip("Clouds raynarching step modifier.")]
  [Range(0.25f, 1f)]
  public float raymarchingScale = 1f;
  [Tooltip("Clouds modelling type.")]
  [Range(0.0f, 1f)]
  public float cloudType = 1f;
  [Tooltip("Cirrus Clouds Color Power")]
  [Range(0.0f, 1f)]
  public float cirrusColorPow = 2f;
  [Tooltip("Flat Clouds Softness")]
  [Range(0.0f, 1f)]
  public float flatSoftness = 0.75f;
  [Tooltip("Flat Clouds Brightness")]
  [Range(0.0f, 1f)]
  public float flatBrightness = 0.75f;
  [Tooltip("Flat Clouds Color Power")]
  [Range(0.0f, 1f)]
  public float flatColorPow = 2f;
  [Tooltip("Cirrus Clouds Alpha")]
  [Range(0.0f, 1f)]
  public float cirrusAlpha;
  [Tooltip("Cirrus Clouds Coverage")]
  [Range(0.0f, 1f)]
  public float cirrusCoverage;
  [Tooltip("Flat Clouds Alpha")]
  [Range(0.0f, 1f)]
  public float flatAlpha;
  [Tooltip("Flat Clouds Coverage")]
  [Range(0.0f, 1f)]
  public float flatCoverage;
}
