// Decompiled with JetBrains decompiler
// Type: EnviroVegetationStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroVegetationStage
{
  [Range(0.0f, 100f)]
  public float minAgePercent;
  public EnviroVegetationStage.GrowState growAction;
  public GameObject GrowGameobjectSpring;
  public GameObject GrowGameobjectSummer;
  public GameObject GrowGameobjectAutumn;
  public GameObject GrowGameobjectWinter;
  public bool billboard;

  public enum GrowState
  {
    Grow,
    Stay,
  }
}
