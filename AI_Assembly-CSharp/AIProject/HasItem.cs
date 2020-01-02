// Decompiled with JetBrains decompiler
// Type: AIProject.HasItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class HasItem : AgentConditional
  {
    [SerializeField]
    private int _targetCategory;

    public virtual TaskStatus OnUpdate()
    {
      foreach (StuffItem stuffItem in this.Agent.AgentData.ItemList)
      {
        if (stuffItem.CategoryID == this._targetCategory)
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
