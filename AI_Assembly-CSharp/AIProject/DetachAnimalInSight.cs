// Decompiled with JetBrains decompiler
// Type: AIProject.DetachAnimalInSight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class DetachAnimalInSight : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      this.Agent.TargetInSightAnimal = (AnimalBase) null;
      return (TaskStatus) 2;
    }
  }
}
