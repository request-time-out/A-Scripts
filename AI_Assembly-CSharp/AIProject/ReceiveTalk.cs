// Decompiled with JetBrains decompiler
// Type: AIProject.ReceiveTalk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class ReceiveTalk : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (!(this.Agent.CommandPartner is AgentActor))
        return (TaskStatus) 1;
      return (this.Agent.CommandPartner as AgentActor).LivesTalkSequence ? (TaskStatus) 3 : (TaskStatus) 2;
    }
  }
}
