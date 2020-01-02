// Decompiled with JetBrains decompiler
// Type: AIProject.Wait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class Wait : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }

    public virtual TaskStatus OnUpdate()
    {
      return (TaskStatus) 3;
    }
  }
}
