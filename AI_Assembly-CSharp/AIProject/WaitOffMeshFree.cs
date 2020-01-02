﻿// Decompiled with JetBrains decompiler
// Type: AIProject.WaitOffMeshFree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  public class WaitOffMeshFree : AgentAction
  {
    private bool _navMeshStopped;
    private ActionPoint _bookingPoint;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._navMeshStopped = this.Agent.NavMeshAgent.get_isStopped();
      if (!this._navMeshStopped)
        this.Agent.NavMeshAgent.set_isStopped(true);
      this._bookingPoint = this.Agent.BookingActionPoint;
    }

    public virtual TaskStatus OnUpdate()
    {
      TaskStatus taskStatus = this.Update();
      if (taskStatus == 2)
        this._bookingPoint.ForceUse((Actor) this.Agent);
      return taskStatus;
    }

    private TaskStatus Update()
    {
      if (Object.op_Equality((Object) this._bookingPoint, (Object) null))
        return (TaskStatus) 1;
      return this._bookingPoint.OffMeshAvailablePoint((Actor) this.Agent) ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      if (!this._navMeshStopped)
        this.Agent.NavMeshAgent.set_isStopped(false);
      ((Task) this).OnEnd();
    }
  }
}
