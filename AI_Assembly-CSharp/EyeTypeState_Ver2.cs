// Decompiled with JetBrains decompiler
// Type: EyeTypeState_Ver2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EyeTypeState_Ver2
{
  [Range(0.0f, 10f)]
  public float bendingMultiplier = 0.4f;
  [Range(0.0f, 100f)]
  public float maxAngleDifference = 10f;
  [Range(-100f, 0.0f)]
  [Tooltip("上を向く際の限界値")]
  public float upBendingAngle = -1f;
  [Range(0.0f, 100f)]
  [Tooltip("下を向く際の限界値")]
  public float downBendingAngle = 6f;
  [Range(-100f, 0.0f)]
  [Tooltip("内側を向く際の限界値")]
  public float inBendingAngle = -6f;
  [Range(0.0f, 100f)]
  [Tooltip("外側を向く際の限界値")]
  public float outBendingAngle = 6f;
  [Range(0.0f, 100f)]
  [Tooltip("補間速度")]
  public float leapSpeed = 2.5f;
  [Range(0.0f, 100f)]
  [Tooltip("正面時のターゲットとの距離")]
  public float frontTagDis = 50f;
  [Range(0.0f, 100f)]
  [Tooltip("近距離原価地位(より目対策)")]
  public float nearDis = 2f;
  [Range(0.0f, 180f)]
  [Tooltip("視野角(水平)")]
  public float hAngleLimit = 110f;
  [Range(0.0f, 180f)]
  [Tooltip("視野角(垂直)")]
  public float vAngleLimit = 80f;
  public EYE_LOOK_TYPE_VER2 lookType = EYE_LOOK_TYPE_VER2.TARGET;
  [Range(-10f, 10f)]
  public float thresholdAngleDifference;
}
