// Decompiled with JetBrains decompiler
// Type: AIProject.IsHealthManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class IsHealthManager : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      bool flag1 = agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(28);
      bool flag2 = (double) agent.AgentData.StatsTable[3] < (double) Singleton<Resources>.Instance.StatusProfile.HealthyPhysicalBorder;
      if (!flag1 || !flag2)
        return (TaskStatus) 1;
      int desireKey = Desire.GetDesireKey(Desire.Type.Break);
      agent.SetMotivation(desireKey, agent.AgentData.StatsTable[5]);
      return (TaskStatus) 2;
    }
  }
}
