// Decompiled with JetBrains decompiler
// Type: AIProject.GetAngry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  public class GetAngry : AgentEmotion
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair angryId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.AngryID;
      this.PlayAnimation(angryId.postureID, angryId.poseID);
    }

    protected override void OnCompletedEmoteTask()
    {
      this.Agent.ApplySituationResultParameter(22);
    }

    public override void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
