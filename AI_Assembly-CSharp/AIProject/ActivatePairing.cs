// Decompiled with JetBrains decompiler
// Type: AIProject.ActivatePairing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ActivatePairing : AgentAction
  {
    [SerializeField]
    private Desire.ActionType _modeToChange;

    public virtual TaskStatus OnUpdate()
    {
      AgentActor targetInSightActor = this.Agent.TargetInSightActor as AgentActor;
      this.Agent.Partner = (Actor) targetInSightActor;
      targetInSightActor.Partner = (Actor) this.Agent;
      targetInSightActor.BehaviorResources.ChangeMode(this._modeToChange);
      targetInSightActor.Mode = this._modeToChange;
      return (TaskStatus) 2;
    }
  }
}
