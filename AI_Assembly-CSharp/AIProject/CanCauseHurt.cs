// Decompiled with JetBrains decompiler
// Type: AIProject.CanCauseHurt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class CanCauseHurt : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.CanCauseHurt ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
