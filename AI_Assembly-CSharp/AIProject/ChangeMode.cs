// Decompiled with JetBrains decompiler
// Type: AIProject.ChangeMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ChangeMode : AgentAction
  {
    [SerializeField]
    private Desire.ActionType _modeType;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.ChangeBehavior(this._modeType);
      return (TaskStatus) 2;
    }
  }
}
