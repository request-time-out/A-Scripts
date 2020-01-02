// Decompiled with JetBrains decompiler
// Type: AIProject.IsFlavorActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsFlavorActive : AgentConditional
  {
    [SerializeField]
    private int _borderLevel = 1;
    [SerializeField]
    private bool _compareDowner;
    [SerializeField]
    private FlavorSkill.Type _target;

    public virtual TaskStatus OnUpdate()
    {
      bool flag = false;
      int num;
      if (!this.Agent.ChaControl.fileGameInfo.flavorState.TryGetValue((int) this._target, out num))
        return (TaskStatus) 1;
      return (!this._compareDowner ? flag | num >= this._borderLevel : flag | num <= this._borderLevel) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
