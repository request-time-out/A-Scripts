// Decompiled with JetBrains decompiler
// Type: AIProject.IsAreaMatch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsAreaMatch : AgentConditional
  {
    [SerializeField]
    private MapArea.AreaType _area = MapArea.AreaType.Indoor;

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.AreaType == this._area ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
