// Decompiled with JetBrains decompiler
// Type: AIProject.ResetSickState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class ResetSickState : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      Sickness sickState = agent.AgentData.SickState;
      if (sickState.ID == 0)
      {
        agent.AgentData.ColdLockInfo.Lock = true;
        if (sickState.Enabled)
          agent.SetStatus(0, 50f);
      }
      sickState.ID = -1;
      return (TaskStatus) 2;
    }
  }
}
