// Decompiled with JetBrains decompiler
// Type: AIProject.SetSickDuration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class SetSickDuration : AgentAction
  {
    [SerializeField]
    private float _dailyDuration = 1f;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.AgentData.SickState.Duration = TimeSpan.FromDays((double) this._dailyDuration);
      return (TaskStatus) 2;
    }
  }
}
