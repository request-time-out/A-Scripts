﻿// Decompiled with JetBrains decompiler
// Type: AIProject.EnterTargetRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterTargetRange : AgentConditional
  {
    private float _arrivedDistance;
    private float _acceptableHeight;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._arrivedDistance = Singleton<Resources>.Instance.AgentProfile.RangeSetting.arrivedDistance;
      this._acceptableHeight = Singleton<Resources>.Instance.AgentProfile.RangeSetting.acceptableHeight;
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      Actor targetInSightActor = agent.TargetInSightActor;
      if (Object.op_Equality((Object) targetInSightActor, (Object) null))
      {
        agent.ChangeBehavior(Desire.ActionType.Normal);
        return (TaskStatus) 1;
      }
      Vector3 position1 = agent.Position;
      position1.y = (__Null) 0.0;
      Vector3 position2 = targetInSightActor.Position;
      position2.y = (__Null) 0.0;
      float num1 = Vector3.Distance(position1, position2);
      float num2 = Mathf.Abs((float) (agent.Position.y - targetInSightActor.Position.y));
      return Object.op_Inequality((Object) targetInSightActor, (Object) null) && (double) num1 <= (double) this._arrivedDistance && (double) num2 <= (double) this._acceptableHeight ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
