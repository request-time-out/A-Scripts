// Decompiled with JetBrains decompiler
// Type: AIProject.ElapsedDaysOfSick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class ElapsedDaysOfSick : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.AgentData.SickState.ElapsedTime > this.Agent.AgentData.SickState.Duration ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
