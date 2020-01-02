// Decompiled with JetBrains decompiler
// Type: AIProject.IsMatchEventType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsMatchEventType : AgentConditional
  {
    [SerializeField]
    private EventType _targetKey;

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Inequality((Object) agent.Partner, (Object) null))
      {
        if (!agent.TargetInSightActionPoint.AgentDateEventType.Contains(this._targetKey) || agent.EventKey != this._targetKey)
          return (TaskStatus) 1;
      }
      else if (!agent.TargetInSightActionPoint.AgentEventType.Contains(this._targetKey) || agent.EventKey != this._targetKey)
        return (TaskStatus) 1;
      return (TaskStatus) 2;
    }
  }
}
