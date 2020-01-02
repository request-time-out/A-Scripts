// Decompiled with JetBrains decompiler
// Type: EnviroLightShaftsSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroLightShaftsSettings
{
  [Header("Quality Settings")]
  [Tooltip("Lightshafts resolution quality setting.")]
  public EnviroLightShafts.SunShaftsResolution resolution = EnviroLightShafts.SunShaftsResolution.Normal;
  [Tooltip("Use cameras depth to hide lightshafts?")]
  public bool useDepthTexture = true;
  [Tooltip("Radius of blurring applied.")]
  public float blurRadius = 6f;
  [Tooltip("Global Lightshafts intensity.")]
  public float intensity = 0.6f;
  [Tooltip("Lightshafts maximum radius.")]
  public float maxRadius = 10f;
  [Tooltip("Lightshafts blur mode.")]
  public EnviroLightShafts.ShaftsScreenBlendMode screenBlendMode;
  [Header("Intensity Settings")]
  [Tooltip("Color gradient for lightshafts based on sun position.")]
  public Gradient lightShaftsColorSun;
  [Tooltip("Color gradient for lightshafts based on moon position.")]
  public Gradient lightShaftsColorMoon;
  [Tooltip("Treshhold gradient for lightshafts based on sun position. This will influence lightshafts intensity!")]
  public Gradient thresholdColorSun;
  [Tooltip("Treshhold gradient for lightshafts based on moon position. This will influence lightshafts intensity!")]
  public Gradient thresholdColorMoon;
}
