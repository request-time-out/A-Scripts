// Decompiled with JetBrains decompiler
// Type: AIProject.Bath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class Bath : AgentStateAction
  {
    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Bath;
      base.OnStart();
      AgentProfile.PoseIDCollection poseIdTable = Singleton<Resources>.Instance.AgentProfile.PoseIDTable;
      switch (agent.HPositionSubID)
      {
        case 2:
        case 13:
          agent.SurprisePoseID = new PoseKeyPair?(poseIdTable.SurpriseInBathSitID);
          break;
        case 8:
          agent.SurprisePoseID = new PoseKeyPair?(poseIdTable.SurpriseInBathStandID);
          break;
      }
    }

    protected override void OnCompletedStateTask()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Bath);
      agent.SetDesire(desireKey, 0.0f);
      agent.SurprisePoseID = new PoseKeyPair?();
      if (agent.AgentData.PlayedDressIn)
        return;
      agent.NextPoint = (ActionPoint) null;
      agent.AgentData.Wetness = 100f;
    }
  }
}
