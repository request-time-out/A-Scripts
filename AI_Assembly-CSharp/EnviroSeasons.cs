// Decompiled with JetBrains decompiler
// Type: EnviroSeasons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroSeasons
{
  [Tooltip("When enabled the system will change seasons automaticly when enough days passed.")]
  public bool calcSeasons;
  [Tooltip("The current season.")]
  public EnviroSeasons.Seasons currentSeasons;
  [HideInInspector]
  public EnviroSeasons.Seasons lastSeason;

  public enum Seasons
  {
    Spring,
    Summer,
    Autumn,
    Winter,
  }
}
