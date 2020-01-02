// Decompiled with JetBrains decompiler
// Type: EnviroWeatherPreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnviroWeatherPreset : ScriptableObject
{
  public string version;
  public string Name;
  [Header("Season Settings")]
  public bool Spring;
  [Range(1f, 100f)]
  public float possibiltyInSpring;
  public bool Summer;
  [Range(1f, 100f)]
  public float possibiltyInSummer;
  public bool Autumn;
  [Range(1f, 100f)]
  public float possibiltyInAutumn;
  public bool winter;
  [Range(1f, 100f)]
  public float possibiltyInWinter;
  [Header("Cloud Settings")]
  public EnviroWeatherCloudsConfig cloudsConfig;
  [Header("Linear Fog")]
  public float fogStartDistance;
  public float fogDistance;
  [Header("Exp Fog")]
  public float fogDensity;
  [Tooltip("Used to modify sky, direct, ambient light and fog color. The color alpha value defines the intensity")]
  public Gradient weatherSkyMod;
  public Gradient weatherLightMod;
  public Gradient weatherFogMod;
  [Range(0.0f, 2f)]
  public float volumeLightIntensity;
  [Range(0.0f, 100f)]
  [Tooltip("The density of height based fog for this weather.")]
  public float heightFogDensity;
  [Range(0.0f, 2f)]
  [Tooltip("Define the height of fog rendered in sky.")]
  public float SkyFogHeight;
  [Tooltip("Define the intensity of fog rendered in sky.")]
  [Range(0.0f, 2f)]
  public float SkyFogIntensity;
  [Range(1f, 10f)]
  [Tooltip("Define the scattering intensity of fog.")]
  public float FogScatteringIntensity;
  [Range(0.0f, 1f)]
  [Tooltip("Block the sundisk with fog.")]
  public float fogSunBlocking;
  [Header("Weather Settings")]
  public List<EnviroWeatherEffects> effectSystems;
  [Range(0.0f, 1f)]
  [Tooltip("Wind intensity that will applied to wind zone.")]
  public float WindStrenght;
  [Range(0.0f, 1f)]
  [Tooltip("The maximum wetness level that can be reached.")]
  public float wetnessLevel;
  [Range(0.0f, 1f)]
  [Tooltip("The maximum snow level that can be reached.")]
  public float snowLevel;
  [Tooltip("Activate this to enable thunder and lightning.")]
  public bool isLightningStorm;
  [Range(0.0f, 2f)]
  [Tooltip("The Intervall of lightning in seconds. Random(lightningInterval,lightningInterval * 2). ")]
  public float lightningInterval;
  [Header("Audio Settings - SFX")]
  [Tooltip("Define an sound effect for this weather preset.")]
  public AudioClip weatherSFX;
  [Header("Audio Settings - Ambient")]
  [Tooltip("This sound wil be played in spring at day.(looped)")]
  public AudioClip SpringDayAmbient;
  [Tooltip("This sound wil be played in spring at night.(looped)")]
  public AudioClip SpringNightAmbient;
  [Tooltip("This sound wil be played in summer at day.(looped)")]
  public AudioClip SummerDayAmbient;
  [Tooltip("This sound wil be played in summer at night.(looped)")]
  public AudioClip SummerNightAmbient;
  [Tooltip("This sound wil be played in autumn at day.(looped)")]
  public AudioClip AutumnDayAmbient;
  [Tooltip("This sound wil be played in autumn at night.(looped)")]
  public AudioClip AutumnNightAmbient;
  [Tooltip("This sound wil be played in winter at day.(looped)")]
  public AudioClip WinterDayAmbient;
  [Tooltip("This sound wil be played in winter at night.(looped)")]
  public AudioClip WinterNightAmbient;

  public EnviroWeatherPreset()
  {
    base.\u002Ector();
  }
}
