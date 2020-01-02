// Decompiled with JetBrains decompiler
// Type: AIProject.ActionCameraData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public struct ActionCameraData
  {
    [Header("フリールック")]
    public Vector3 freePos;
    [Space]
    [Header("固定カメラ")]
    public Vector3 nonMovePos;
    public Vector3 nonMoveRot;
  }
}
