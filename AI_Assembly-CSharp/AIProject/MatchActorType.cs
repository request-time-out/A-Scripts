// Decompiled with JetBrains decompiler
// Type: AIProject.MatchActorType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class MatchActorType : AgentConditional
  {
    [SerializeField]
    private string _typeName = string.Empty;

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.Agent.Partner, (Object) null))
        return (TaskStatus) 1;
      return ((object) this.Agent.Partner).GetType().Name != this._typeName ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
