// Decompiled with JetBrains decompiler
// Type: NeckLimitParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class NeckLimitParameter
{
  [Range(-100f, 0.0f)]
  [Tooltip("上を向く際の限界値(値が大きくなればなるほど上を向けられる)")]
  public float upBendingAngle = -1f;
  [Range(0.0f, 100f)]
  [Tooltip("下を向く際の限界値(値が大きくなればなるほど下を向けられる)")]
  public float downBendingAngle = 6f;
  [Range(-100f, 0.0f)]
  [Tooltip("左側を向く際の限界値")]
  public float minBendingAngle = -6f;
  [Range(0.0f, 100f)]
  [Tooltip("右側を向く際の限界値")]
  public float maxBendingAngle = 6f;
  public string name;
}
