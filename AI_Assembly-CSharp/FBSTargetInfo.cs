// Decompiled with JetBrains decompiler
// Type: FBSTargetInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class FBSTargetInfo
{
  public GameObject ObjTarget;
  public FBSTargetInfo.CloseOpen[] PtnSet;
  private SkinnedMeshRenderer smrTarget;

  public void SetSkinnedMeshRenderer()
  {
    if (!Object.op_Implicit((Object) this.ObjTarget))
      return;
    this.smrTarget = (SkinnedMeshRenderer) this.ObjTarget.GetComponent<SkinnedMeshRenderer>();
  }

  public SkinnedMeshRenderer GetSkinnedMeshRenderer()
  {
    return this.smrTarget;
  }

  public void Clear()
  {
    this.ObjTarget = (GameObject) null;
    this.PtnSet = (FBSTargetInfo.CloseOpen[]) null;
    this.smrTarget = (SkinnedMeshRenderer) null;
  }

  [Serializable]
  public class CloseOpen
  {
    public int Close;
    public int Open;
  }
}
