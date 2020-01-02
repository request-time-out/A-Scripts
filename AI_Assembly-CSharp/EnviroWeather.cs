// Decompiled with JetBrains decompiler
// Type: EnviroWeather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnviroWeather
{
  [Tooltip("If disabled the weather will never change.")]
  public bool updateWeather = true;
  [HideInInspector]
  public List<EnviroWeatherPreset> weatherPresets = new List<EnviroWeatherPreset>();
  [HideInInspector]
  public List<EnviroWeatherPrefab> WeatherPrefabs = new List<EnviroWeatherPrefab>();
  [Tooltip("List of additional zones. Will be updated on startup!")]
  public List<EnviroZone> zones = new List<EnviroZone>();
  public EnviroWeatherPreset startWeatherPreset;
  [Tooltip("The current active zone.")]
  public EnviroZone currentActiveZone;
  [Tooltip("The current active weather conditions.")]
  public EnviroWeatherPrefab currentActiveWeatherPrefab;
  public EnviroWeatherPreset currentActiveWeatherPreset;
  [HideInInspector]
  public EnviroWeatherPrefab lastActiveWeatherPrefab;
  [HideInInspector]
  public EnviroWeatherPreset lastActiveWeatherPreset;
  [HideInInspector]
  public GameObject VFXHolder;
  [HideInInspector]
  public float wetness;
  [HideInInspector]
  public float curWetness;
  [HideInInspector]
  public float snowStrength;
  [HideInInspector]
  public float curSnowStrength;
  [HideInInspector]
  public int thundersfx;
  [HideInInspector]
  public EnviroAudioSource currentAudioSource;
  [HideInInspector]
  public bool weatherFullyChanged;
}
