// Decompiled with JetBrains decompiler
// Type: AIProject.RandomSelectAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class RandomSelectAction : AgentAction
  {
    [SerializeField]
    private int _randMax = 1;
    [SerializeField]
    private int _randMin;

    public virtual TaskStatus OnUpdate()
    {
      this.Agent.SelectedActionID = Random.Range(this._randMin, this._randMax);
      return (TaskStatus) 2;
    }
  }
}
