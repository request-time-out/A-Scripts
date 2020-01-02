// Decompiled with JetBrains decompiler
// Type: EnviroSatellite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroSatellite
{
  [Tooltip("Name of this satellite")]
  public string name;
  [Tooltip("Prefab with model that get instantiated.")]
  public GameObject prefab;
  [Tooltip("Orbit distance.")]
  public float orbit;
  [Tooltip("Orbit modification on x axis.")]
  public float xRot;
  [Tooltip("Orbit modification on y axis.")]
  public float yRot;
}
