// Decompiled with JetBrains decompiler
// Type: AIProject.HasEquipedItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class HasEquipedItem : AgentConditional
  {
    [SerializeField]
    private int _eventItemID;

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.EquipedItem == null)
        return (TaskStatus) 1;
      return this._eventItemID == this.Agent.EquipedItem.EventItemID ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
