// Decompiled with JetBrains decompiler
// Type: AIProject.LikesRain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

namespace AIProject
{
  [TaskCategory("")]
  public class LikesRain : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      foreach (KeyValuePair<int, int> keyValuePair in this.Agent.ChaControl.fileGameInfo.normalSkill)
      {
        if (keyValuePair.Value == 0)
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
