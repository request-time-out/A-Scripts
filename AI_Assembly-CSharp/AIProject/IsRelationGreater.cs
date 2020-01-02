// Decompiled with JetBrains decompiler
// Type: AIProject.IsRelationGreater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsRelationGreater : AgentConditional
  {
    [SerializeField]
    private float _borderValue = 60f;

    public virtual TaskStatus OnUpdate()
    {
      int id = this.Agent.TargetInSightActor.ID;
      int num1;
      if (!this.Agent.AgentData.FriendlyRelationShipTable.TryGetValue(id, out num1))
      {
        int num2 = 50;
        this.Agent.AgentData.FriendlyRelationShipTable[id] = num2;
        num1 = num2;
      }
      return (double) num1 > (double) this._borderValue ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
