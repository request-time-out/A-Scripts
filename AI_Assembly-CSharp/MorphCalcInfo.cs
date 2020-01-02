// Decompiled with JetBrains decompiler
// Type: MorphCalcInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class MorphCalcInfo
{
  public GameObject TargetObj;
  public Mesh OriginalMesh;
  public Mesh TargetMesh;
  public Vector3[] OriginalPos;
  public Vector3[] OriginalNormal;
  public bool WeightFlags;
  public int[] UpdateIndex;
  public MorphUpdateInfo[] UpdateInfo;
}
