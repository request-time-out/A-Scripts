// Decompiled with JetBrains decompiler
// Type: EnviroWeatherPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnviroWeatherPrefab : MonoBehaviour
{
  public EnviroWeatherPreset weatherPreset;
  [HideInInspector]
  public List<ParticleSystem> effectSystems;
  [HideInInspector]
  public List<float> effectEmmisionRates;

  public EnviroWeatherPrefab()
  {
    base.\u002Ector();
  }
}
