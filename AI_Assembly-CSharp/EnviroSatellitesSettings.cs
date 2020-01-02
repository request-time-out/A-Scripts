// Decompiled with JetBrains decompiler
// Type: EnviroSatellitesSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnviroSatellitesSettings
{
  [Tooltip("List of satellites.")]
  public List<EnviroSatellite> additionalSatellites = new List<EnviroSatellite>();
}
