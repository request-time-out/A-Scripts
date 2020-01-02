// Decompiled with JetBrains decompiler
// Type: AIProject.IsAnimalFree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class IsAnimalFree : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      bool? nullable = this.Agent.TargetInSightAnimal?.IsWithAgentFree(this.Agent);
      return (!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0 ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
