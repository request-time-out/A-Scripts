// Decompiled with JetBrains decompiler
// Type: EnviroLightSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class EnviroLightSettings
{
  [Tooltip("Direct light sun intensity based on sun position in sky")]
  public AnimationCurve directLightSunIntensity = new AnimationCurve();
  [Tooltip("Direct light moon intensity based on moon position in sky")]
  public AnimationCurve directLightMoonIntensity = new AnimationCurve();
  [Tooltip("Realtime shadow strength of the directional light.")]
  public AnimationCurve shadowIntensity = new AnimationCurve();
  [Header("Ambient")]
  [Tooltip("Ambient Rendering Mode.")]
  public AmbientMode ambientMode = (AmbientMode) 3;
  [Tooltip("Ambientlight intensity based on sun position in sky.")]
  public AnimationCurve ambientIntensity = new AnimationCurve();
  [Header("Global Reflections")]
  [Tooltip("Enable/disable enviro reflection probe..")]
  public bool globalReflections = true;
  [Tooltip("Reflection probe intensity.")]
  public float globalReflectionsIntensity = 0.5f;
  [Tooltip("Reflection probe update rate.")]
  public float globalReflectionsUpdate = 0.025f;
  [Tooltip("Reflection probe intensity.")]
  [Range(0.1f, 10f)]
  public float globalReflectionsScale = 1f;
  [Header("Direct")]
  [Tooltip("Color gradient for sun and moon light based on sun position in sky.")]
  public Gradient LightColor;
  [Tooltip("Direct lighting y-offset.")]
  [Range(0.0f, 5000f)]
  public float directLightAngleOffset;
  [Tooltip("Ambientlight sky color based on sun position in sky.")]
  public Gradient ambientSkyColor;
  [Tooltip("Ambientlight Equator color based on sun position in sky.")]
  public Gradient ambientEquatorColor;
  [Tooltip("Ambientlight Ground color based on sun position in sky.")]
  public Gradient ambientGroundColor;
}
