// Decompiled with JetBrains decompiler
// Type: AIProject.ExitTargetRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ExitTargetRange : AgentConditional
  {
    private float _distance;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._distance = Singleton<Resources>.Instance.AgentProfile.RangeSetting.leaveDistance;
    }

    public virtual TaskStatus OnUpdate()
    {
      Actor targetInSightActor = this.Agent.TargetInSightActor;
      return Object.op_Inequality((Object) targetInSightActor, (Object) null) && (double) Vector3.Distance(this.Agent.Position, targetInSightActor.Position) > (double) this._distance ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
