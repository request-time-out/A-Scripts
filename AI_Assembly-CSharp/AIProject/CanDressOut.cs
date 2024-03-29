﻿// Decompiled with JetBrains decompiler
// Type: AIProject.CanDressOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class CanDressOut : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      return (double) agent.ChaControl.fileGameInfo.flavorState[2] >= (double) Singleton<Resources>.Instance.StatusProfile.CanDressBorder && agent.AgentData.PlayedDressIn ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
