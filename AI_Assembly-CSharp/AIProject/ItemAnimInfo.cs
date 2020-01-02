// Decompiled with JetBrains decompiler
// Type: AIProject.ItemAnimInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class ItemAnimInfo
  {
    public Animator Animator { get; set; }

    public AnimatorControllerParameter[] Parameters { get; set; }

    public bool Sync { get; set; }
  }
}
