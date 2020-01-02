// Decompiled with JetBrains decompiler
// Type: AIProject.Toilet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class Toilet : AgentStateAction
  {
    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Toilet;
      base.OnStart();
      AgentProfile.PoseIDCollection poseIdTable = Singleton<Resources>.Instance.AgentProfile.PoseIDTable;
      switch (agent.HPositionSubID)
      {
        case 3:
          agent.SurprisePoseID = new PoseKeyPair?(poseIdTable.SurpriseInToiletSitID);
          break;
        case 5:
          agent.SurprisePoseID = new PoseKeyPair?(poseIdTable.SurpriseInToiletSquatID);
          break;
      }
    }

    protected override void OnCompletedStateTask()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
      agent.SetDesire(desireKey, 0.0f);
      agent.SurprisePoseID = new PoseKeyPair?();
    }
  }
}
