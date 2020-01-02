// Decompiled with JetBrains decompiler
// Type: EnviroProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroProfile : ScriptableObject
{
  public string version;
  public EnviroLightSettings lightSettings;
  public EnviroVolumeLightingSettings volumeLightSettings;
  public EnviroSkySettings skySettings;
  public EnviroCloudSettings cloudsSettings;
  public EnviroWeatherSettings weatherSettings;
  public EnviroFogSettings fogSettings;
  public EnviroLightShaftsSettings lightshaftsSettings;
  public EnviroSeasonSettings seasonsSettings;
  public EnviroAudioSettings audioSettings;
  public EnviroSatellitesSettings satelliteSettings;
  public EnviroQualitySettings qualitySettings;
  [HideInInspector]
  public EnviroProfile.settingsMode viewMode;
  [HideInInspector]
  public bool showPlayerSetup;
  [HideInInspector]
  public bool showRenderingSetup;
  [HideInInspector]
  public bool showComponentsSetup;
  [HideInInspector]
  public bool showTimeUI;
  [HideInInspector]
  public bool showWeatherUI;
  [HideInInspector]
  public bool showAudioUI;
  [HideInInspector]
  public bool showEffectsUI;
  [HideInInspector]
  public bool modified;

  public EnviroProfile()
  {
    base.\u002Ector();
  }

  public enum settingsMode
  {
    Lighting,
    Sky,
    Weather,
    Clouds,
    Fog,
    VolumeLighting,
    Lightshafts,
    Season,
    Satellites,
    Audio,
    Quality,
  }
}
