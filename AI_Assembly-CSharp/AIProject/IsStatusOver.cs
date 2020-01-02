// Decompiled with JetBrains decompiler
// Type: AIProject.IsStatusOver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  public class IsStatusOver : AgentConditional
  {
    [SerializeField]
    private float _border;
    [SerializeField]
    private bool _compareDowner;
    [SerializeField]
    private Status.Type _target;

    public virtual TaskStatus OnUpdate()
    {
      bool flag = false;
      float num;
      if (!this.Agent.AgentData.StatsTable.TryGetValue((int) this._target, out num))
        return (TaskStatus) 1;
      return (!this._compareDowner ? flag | (double) num > (double) this._border : flag | (double) num < (double) this._border) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
