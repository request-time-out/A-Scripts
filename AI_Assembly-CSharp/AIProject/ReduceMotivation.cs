// Decompiled with JetBrains decompiler
// Type: AIProject.ReduceMotivation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ReduceMotivation : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.UpdateMotivation = true;
    }

    public virtual TaskStatus OnUpdate()
    {
      return (TaskStatus) 3;
    }

    public virtual void OnBehaviorComplete()
    {
      this.Agent.UpdateMotivation = false;
    }

    public virtual void OnEnd()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Inequality((Object) agent, (Object) null))
        agent.UpdateMotivation = false;
      ((Task) this).OnEnd();
    }
  }
}
