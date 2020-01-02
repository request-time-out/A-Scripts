// Decompiled with JetBrains decompiler
// Type: AIProject.GetHunger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

namespace AIProject
{
  public class GetHunger : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      Dictionary<int, float> statsTable;
      (statsTable = this.Agent.AgentData.StatsTable)[2] = statsTable[2] - 10f;
      return (TaskStatus) 2;
    }
  }
}
