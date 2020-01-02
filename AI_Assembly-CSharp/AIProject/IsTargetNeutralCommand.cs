﻿// Decompiled with JetBrains decompiler
// Type: AIProject.IsTargetNeutralCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject
{
  [TaskCategory("")]
  public class IsTargetNeutralCommand : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      if (!(this.Agent.TargetInSightActor is ICommandable targetInSightActor))
        return (TaskStatus) 1;
      return targetInSightActor.IsNeutralCommand ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
