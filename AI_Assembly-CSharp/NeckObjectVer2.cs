// Decompiled with JetBrains decompiler
// Type: NeckObjectVer2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class NeckObjectVer2
{
  public string name;
  [Tooltip("計算参照オブジェクト こいつから計算している")]
  public Transform referenceCalc;
  [Tooltip("実際動かすオブジェクト")]
  public Transform neckBone;
  [Tooltip("リングオブジェクト")]
  public Transform controlBone;
  [SerializeField]
  [Tooltip("デバッグ用表示")]
  internal Quaternion fixAngle;
  [SerializeField]
  [Tooltip("デバッグ用表示")]
  internal float angleHRate;
  [SerializeField]
  [Tooltip("デバッグ用表示")]
  internal float angleVRate;
  [SerializeField]
  [Tooltip("デバッグ用表示")]
  internal float angleH;
  [SerializeField]
  [Tooltip("デバッグ用表示")]
  internal float angleV;
  internal Quaternion fixAngleBackup;
  internal Quaternion backupLocalRotaionByTarget;
}
