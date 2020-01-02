// Decompiled with JetBrains decompiler
// Type: AIProject.HasPartner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class HasPartner : AgentConditional
  {
    [SerializeField]
    private bool _condition = true;

    public virtual TaskStatus OnUpdate()
    {
      if (this._condition)
      {
        if (Object.op_Inequality((Object) this.Agent.Partner, (Object) null))
          return (TaskStatus) 2;
      }
      else if (Object.op_Equality((Object) this.Agent.Partner, (Object) null))
        return (TaskStatus) 2;
      return (TaskStatus) 1;
    }
  }
}
