// Decompiled with JetBrains decompiler
// Type: AIProject.IsReliabilityActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsReliabilityActive : AgentConditional
  {
    [SerializeField]
    private int _border = 50;
    [SerializeField]
    private bool _checkLow;

    public virtual TaskStatus OnUpdate()
    {
      ChaFileGameInfo fileGameInfo = this.Agent.ChaControl.fileGameInfo;
      if (this._checkLow)
      {
        if (fileGameInfo.phase <= 1)
          return (TaskStatus) 2;
        if (fileGameInfo.flavorState[1] <= this._border)
          return (TaskStatus) 2;
      }
      else
      {
        if (fileGameInfo.phase <= 1)
          return (TaskStatus) 1;
        if (fileGameInfo.flavorState[1] >= this._border)
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
