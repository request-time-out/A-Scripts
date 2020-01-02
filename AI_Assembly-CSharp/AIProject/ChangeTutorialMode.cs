// Decompiled with JetBrains decompiler
// Type: AIProject.ChangeTutorialMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ChangeTutorialMode : AgentAction
  {
    [SerializeField]
    private Tutorial.ActionType _changeType;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.ChangeTutorialBehavior(this._changeType);
      return (TaskStatus) 2;
    }
  }
}
