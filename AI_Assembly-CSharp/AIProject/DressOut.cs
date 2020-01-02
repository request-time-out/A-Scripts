// Decompiled with JetBrains decompiler
// Type: AIProject.DressOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class DressOut : AgentStateAction
  {
    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.DressOut;
      agent.AgentData.BathCoordinateFileName = (string) null;
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      this.Agent.AgentData.PlayedDressIn = false;
    }
  }
}
