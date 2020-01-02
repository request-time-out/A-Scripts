// Decompiled with JetBrains decompiler
// Type: AIProject.ReserveMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ReserveMode : AgentAction
  {
    [SerializeField]
    private Desire.ActionType _actionType;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.ReservedMode = this._actionType;
      return (TaskStatus) 2;
    }
  }
}
