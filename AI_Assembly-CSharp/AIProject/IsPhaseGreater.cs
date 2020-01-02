// Decompiled with JetBrains decompiler
// Type: AIProject.IsPhaseGreater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsPhaseGreater : AgentConditional
  {
    [SerializeField]
    private int _comparisonValue = 2;

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.ChaControl.fileGameInfo.phase < this._comparisonValue ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
