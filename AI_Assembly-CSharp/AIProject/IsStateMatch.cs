// Decompiled with JetBrains decompiler
// Type: AIProject.IsStateMatch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsStateMatch : AgentConditional
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _targetState;

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.StateType != this._targetState ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
