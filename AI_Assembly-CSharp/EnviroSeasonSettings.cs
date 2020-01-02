// Decompiled with JetBrains decompiler
// Type: EnviroSeasonSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroSeasonSettings
{
  [Tooltip("How many days in spring?")]
  public float SpringInDays = 90f;
  [Tooltip("How many days in summer?")]
  public float SummerInDays = 93f;
  [Tooltip("How many days in autumn?")]
  public float AutumnInDays = 92f;
  [Tooltip("How many days in winter?")]
  public float WinterInDays = 90f;
}
