// Decompiled with JetBrains decompiler
// Type: AIProject.IsBadGirl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsBadGirl : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      return agent.ChaControl.fileGameInfo.morality < 50 && (double) agent.AgentData.StatsTable[2] < (double) statusProfile.ShallowSleepHungerLowBorder && (double) Random.Range(0.0f, 100f) < (double) statusProfile.ShallowSleepProb ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
