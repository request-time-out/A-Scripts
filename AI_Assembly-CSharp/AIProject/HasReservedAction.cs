// Decompiled with JetBrains decompiler
// Type: AIProject.HasReservedAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class HasReservedAction : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.Agent.Partner, (Object) null))
        return (TaskStatus) 1;
      return Object.op_Equality((Object) this.Agent.Partner.CurrentPoint, (Object) null) ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
