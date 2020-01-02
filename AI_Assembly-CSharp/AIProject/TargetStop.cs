// Decompiled with JetBrains decompiler
// Type: AIProject.TargetStop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class TargetStop : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      Actor targetInSightActor = this.Agent.TargetInSightActor;
      switch (targetInSightActor)
      {
        case AgentActor _:
        case MerchantActor _:
          targetInSightActor.StopNavMeshAgent();
          targetInSightActor.ChangeStaticNavMeshAgentAvoidance();
          return (TaskStatus) 2;
        default:
          return (TaskStatus) 1;
      }
    }
  }
}
