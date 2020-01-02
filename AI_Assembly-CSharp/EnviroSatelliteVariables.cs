// Decompiled with JetBrains decompiler
// Type: EnviroSatelliteVariables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroSatelliteVariables
{
  [Tooltip("Name of this satellite")]
  public string name;
  [Tooltip("Prefab with model that get instantiated.")]
  public GameObject prefab;
  [Tooltip("This value will influence the satellite orbitpositions.")]
  public float orbit_X;
  [Tooltip("This value will influence the satellite orbitpositions.")]
  public float orbit_Y;
  [Tooltip("The speed of the satellites orbit.")]
  public float speed;
}
