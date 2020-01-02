// Decompiled with JetBrains decompiler
// Type: AIProject.ChangeStateType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ChangeStateType : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _type;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.StateType = this._type;
      return (TaskStatus) 2;
    }
  }
}
