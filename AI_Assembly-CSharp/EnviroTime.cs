// Decompiled with JetBrains decompiler
// Type: EnviroTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroTime
{
  [Tooltip("None = No time auto time progressing, Simulated = Time calculated with DayLenghtInMinutes, SystemTime = uses your systemTime.")]
  public EnviroTime.TimeProgressMode ProgressTime = EnviroTime.TimeProgressMode.Simulated;
  [Tooltip("Current Time: hours")]
  [Range(0.0f, 24f)]
  public int Hours = 12;
  [Tooltip("Current Time: Days")]
  public int Days = 1;
  [Tooltip("Current Time: Years")]
  public int Years = 1;
  [Space(20f)]
  [Tooltip("Day lenght in realtime minutes.")]
  public float DayLengthInMinutes = 5f;
  [Tooltip("Night lenght in realtime minutes.")]
  public float NightLengthInMinutes = 5f;
  [Tooltip("Current Time: minutes")]
  [Range(0.0f, 60f)]
  public int Seconds;
  [Tooltip("Current Time: minutes")]
  [Range(0.0f, 60f)]
  public int Minutes;
  [Range(-13f, 13f)]
  [Tooltip("Time offset for timezones")]
  public int utcOffset;
  [Range(-90f, 90f)]
  [Tooltip("-90,  90   Horizontal earth lines")]
  public float Latitude;
  [Range(-180f, 180f)]
  [Tooltip("-180, 180  Vertical earth line")]
  public float Longitude;
  [HideInInspector]
  public float solarTime;
  [HideInInspector]
  public float lunarTime;

  public enum TimeProgressMode
  {
    None,
    Simulated,
    OneDay,
    SystemTime,
  }
}
