// Decompiled with JetBrains decompiler
// Type: AIProject.GetGloomy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  public class GetGloomy : AgentEmotion
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair groomyId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.GroomyID;
      this.PlayAnimation(groomyId.postureID, groomyId.poseID);
    }

    protected override void OnCompletedEmoteTask()
    {
      AgentActor agent = this.Agent;
      agent.ApplySituationResultParameter(21);
      agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
