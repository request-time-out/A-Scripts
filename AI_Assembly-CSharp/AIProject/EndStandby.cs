// Decompiled with JetBrains decompiler
// Type: AIProject.EndStandby
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class EndStandby : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      this.Agent.IsStandby = false;
      return (TaskStatus) 2;
    }
  }
}
