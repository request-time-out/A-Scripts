// Decompiled with JetBrains decompiler
// Type: AIProject.WakeUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  public class WakeUp : AgentEmotion
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.DeactivateNavMeshAgent();
      PoseKeyPair wakeUpId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.WakeUpID;
      this.PlayAnimation(wakeUpId.postureID, wakeUpId.poseID);
    }

    public override void OnEnd()
    {
      base.OnEnd();
      this.Agent.ActivateNavMeshAgent();
    }
  }
}
