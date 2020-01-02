// Decompiled with JetBrains decompiler
// Type: AIProject.Equip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Equip : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      ((Behaviour) this.Agent.Animation.Poser).set_enabled(true);
      ((Behaviour) this.Agent.Animation.ArmAnimator).set_enabled(true);
      return (TaskStatus) 2;
    }
  }
}
